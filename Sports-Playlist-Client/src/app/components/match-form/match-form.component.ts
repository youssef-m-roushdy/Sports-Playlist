import { Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatchService, Match } from '../../services/match.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';

// Angular Material Form Controls
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { catchError, throwError } from 'rxjs';

/**
 * Match Form Component
 * 
 * A reusable component for creating and editing match entries.
 * Handles both new matches and editing existing ones through the same interface.
 */
@Component({
  selector: 'app-match-form',
  standalone: true,
  templateUrl: './match-form.component.html',
  styleUrls: ['./match-form.component.css'],
  encapsulation: ViewEncapsulation.None, // Allow CSS to affect child components
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatSnackBarModule,
    MatCardModule,
    MatIconModule
  ],
})
export class MatchFormComponent implements OnInit {
  // Input/Output properties
  @Input() matchData: Match | null = null;
  @Output() formSubmitted = new EventEmitter<void>();

  // Component properties
  matchForm!: FormGroup;
  isEditMode = false;
  statusOptions = ['Live', 'Replay'];
  loading = false;
  matchId?: number;
  errorMessage = ''; // For displaying form-level errors

  /**
   * Component constructor
   */
  constructor(
    private fb: FormBuilder,
    private matchService: MatchService,
    private snackBar: MatSnackBar,
    public router: Router, // Public for template access
    private route: ActivatedRoute
  ) {}

  /**
   * Initialize component and setup form
   */
  ngOnInit(): void {
    // Initialize the form with default values
    this.initForm();
    
    // Check if we're editing an existing match
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.matchId = +params['id'];
        this.isEditMode = true;
        this.loading = true;
        
        // Load the existing match data
        this.loadMatchData(this.matchId);
      }
    });
  }

  /**
   * Load match data from the server for editing
   * @param id Match ID to load
   */
  private loadMatchData(id: number): void {
    this.matchService.getMatchById(id).subscribe({
      next: (match) => {
        this.matchData = match;
        this.updateFormWithMatchData(match);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading match data:', err);
        this.errorMessage = `Failed to load match: ${this.getErrorMessage(err)}`;
        this.snackBar.open(`Error loading match data`, 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
        this.loading = false;
        this.router.navigate(['/home']);
      }
    });
  }

  /**
   * Initialize form with empty values and validation
   */
  private initForm(): void {
    this.matchForm = this.fb.group({
      title: ['', Validators.required],
      competition: ['', Validators.required],
      matchDate: ['', Validators.required],
      status: ['Live', Validators.required],
      matchUrl: ['', Validators.required]
    });
  }

  /**
   * Update form values with existing match data
   * @param match Match data to populate the form
   */
  private updateFormWithMatchData(match: Match): void {
    this.matchForm.patchValue({
      title: match.title,
      competition: match.competition,
      matchDate: new Date(match.matchDate),
      status: match.status,
      matchUrl: match.matchUrl
    });
  }

  /**
   * Form submission handler
   * Creates a new match or updates existing one based on isEditMode
   */
  onSubmit(): void {
    // Clear previous error message
    this.errorMessage = '';

    // Validate the form
    if (this.matchForm.invalid) {
      this.markAllFieldsAsTouched();
      this.errorMessage = 'Please fix the form errors before submitting.';
      return;
    }

    // Prepare the data for submission
    const formData = this.matchForm.value;
    const matchDate = new Date(formData.matchDate).toISOString();
    const payload = { ...formData, matchDate };

    this.loading = true;
    
    // Determine if we're creating or updating
    const request = this.isEditMode && this.matchId
      ? this.matchService.updateMatch(this.matchId, payload)
      : this.matchService.createMatch(payload);

    // Send the request to the API
    request.subscribe({
      next: () => {
        // Success handling
        this.snackBar.open(
          `Match ${this.isEditMode ? 'updated' : 'created'} successfully!`,
          'Close',
          { duration: 3000, panelClass: ['success-snackbar'] }
        );
        this.formSubmitted.emit();
        this.loading = false;
        this.router.navigate(['/home']);
      },
      error: (err) => {
        // Error handling
        this.loading = false;
        this.errorMessage = this.getErrorMessage(err);
        this.snackBar.open(
          this.errorMessage,
          'Close',
          { duration: 5000, panelClass: ['error-snackbar'] }
        );
      },
    });
  }

  /**
   * Extract meaningful error message from error response
   * @param error Error object from HTTP request
   * @returns Formatted error message
   */
  private getErrorMessage(error: any): string {
    if (error.error?.errors) {
      // Handle validation errors array
      const errorMessages = [];
      const errors = error.error.errors;
      
      // Extract error messages from different possible formats
      if (errors.Title) errorMessages.push(errors.Title.join(', '));
      if (errors.Competition) errorMessages.push(errors.Competition.join(', '));
      if (errors.MatchDate) errorMessages.push(errors.MatchDate.join(', '));
      if (errors.MatchUrl) errorMessages.push(errors.MatchUrl.join(', '));
      if (errors.Status) errorMessages.push(errors.Status.join(', '));
      
      if (errorMessages.length > 0) {
        return errorMessages.join(' ');
      }
    }
    
    // Handle different error formats
    if (error.error?.message) {
      return error.error.message;
    } else if (error.status === 0) {
      return 'Unable to connect to server. Please check your internet connection.';
    } else {
      return `Error ${this.isEditMode ? 'updating' : 'creating'} match. Please try again later.`;
    }
  }

  /**
   * Mark all form fields as touched to trigger validation errors display
   */
  private markAllFieldsAsTouched(): void {
    Object.keys(this.matchForm.controls).forEach(field => {
      const control = this.matchForm.get(field);
      control?.markAsTouched();
    });
  }

  /**
   * Helper for template to check if a field has errors
   * @param controlName Name of the form control to check
   * @param errorType Optional specific error type to check for
   * @returns Boolean indicating if the field has the specified error
   */
  hasError(controlName: string, errorType?: string): boolean {
    const control = this.matchForm.get(controlName);
    if (errorType) {
      return !!control?.hasError(errorType) && !!control?.touched;
    }
    return !!control?.invalid && !!control?.touched;
  }
}
