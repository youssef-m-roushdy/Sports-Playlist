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
import { catchError } from 'rxjs';

@Component({
  selector: 'app-match-form',
  standalone: true,
  templateUrl: './match-form.component.html',
  styleUrls: ['./match-form.component.css'],
  encapsulation: ViewEncapsulation.None, // This allows your CSS to affect child components
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
  @Input() matchData: Match | null = null;
  @Output() formSubmitted = new EventEmitter<void>();

  matchForm!: FormGroup;
  isEditMode = false;
  statusOptions = ['Live', 'Replay'];
  loading = false;
  matchId?: number;

  constructor(
    private fb: FormBuilder,
    private matchService: MatchService,
    private snackBar: MatSnackBar,
    public router: Router, // Changed to public for template access
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.initForm();
    
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.matchId = +params['id'];
        this.isEditMode = true;
        this.loading = true;
        
        this.matchService.getMatchById(this.matchId).subscribe({
          next: (match) => {
            this.matchData = match;
            this.updateFormWithMatchData(match);
            this.loading = false;
          },
          error: (err) => {
            console.error('Error loading match data:', err);
            this.snackBar.open(`Error loading match data: ${err.message}`, 'Close', {
              duration: 5000
            });
            this.loading = false;
            this.router.navigate(['/home']);
          }
        });
      }
    });
  }

  private initForm(): void {
    this.matchForm = this.fb.group({
      title: ['', Validators.required],
      competition: ['', Validators.required],
      matchDate: ['', Validators.required],
      status: ['Live', Validators.required],
      matchUrl: ['', Validators.required]
    });
  }

  private updateFormWithMatchData(match: Match): void {
    this.matchForm.patchValue({
      title: match.title,
      competition: match.competition,
      matchDate: new Date(match.matchDate),
      status: match.status,
      matchUrl: match.matchUrl
    });
  }

  onSubmit(): void {
    if (this.matchForm.invalid) {
      this.matchForm.markAllAsTouched();
      return;
    }

    const formData = this.matchForm.value;
    const matchDate = new Date(formData.matchDate).toISOString();
    const payload = { ...formData, matchDate };

    this.loading = true;
    const request = this.isEditMode && this.matchId
      ? this.matchService.updateMatch(this.matchId, payload).pipe(
        catchError((err) => {
          let errorMsgs: string[] = [];
          console.error('Error updating match:', err?.error?.errors?.MatchUrl?.join(', '));
          errorMsgs.push(err?.error?.errors?.MatchUrl?.join(', ')|| null);
          errorMsgs.push(err?.error?.errors?.Title?.join(', ')|| null);
          errorMsgs.push(err?.error?.errors?.Competition?.join(', ')|| null);
          errorMsgs.push(err?.error?.errors?.MatchDate?.join(', ') || null);
          errorMsgs = errorMsgs.filter((msg) => msg !== null);
          for (const msg of errorMsgs) {
            this.snackBar.open(msg, 'Close', {
              duration: 5000
            });
          }
          this.loading = false;
          return [];
          })
      )
      : this.matchService.createMatch(payload).pipe(
          catchError((err) => {
            let errorMsgs: string[] = [];
            console.error('Error updating match:', err?.error?.errors?.MatchUrl?.join(', '));
            errorMsgs.push(err?.error?.errors?.MatchUrl?.join(', ')|| null);
            errorMsgs.push(err?.error?.errors?.Title?.join(', ')|| null);
            errorMsgs.push(err?.error?.errors?.Competition?.join(', ')|| null);
            errorMsgs.push(err?.error?.errors?.MatchDate?.join(', ') || null);
            errorMsgs = errorMsgs.filter((msg) => msg !== null);
            for (const msg of errorMsgs) {
              this.snackBar.open(msg, 'Close', {
                duration: 5000
              });
            }
            this.loading = false;
            return [];
          })
      );

    request.subscribe({
      next: () => {
        this.snackBar.open(
          `Match ${this.isEditMode ? 'updated' : 'created'} successfully!`,
          'Close',
          { duration: 3000 }
        );
        this.formSubmitted.emit();
        this.loading = false;
        this.router.navigate(['/home']);
      },
      error: (err) => {
        this.loading = false;
        this.snackBar.open(
          `Error ${this.isEditMode ? 'updating' : 'creating'} match: ${
            err.message
          }`,
          'Close',
          { duration: 5000 }
        );
      },
    });
  }
}
