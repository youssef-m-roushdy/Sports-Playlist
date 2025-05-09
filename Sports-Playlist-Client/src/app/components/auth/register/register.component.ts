import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, MatSnackBarModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private fb: FormBuilder, 
    private router: Router,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      username: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]]
    }, { validator: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      this.successMessage = '';
      
      const { firstName, lastName, username, email, password } = this.registerForm.value;
      
      this.authService.register(firstName, lastName, username, email, password).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.successMessage = response.message || 'Registration successful! You can now login.';
          this.registerForm.reset();
          
          // Show success snackbar
          this.snackBar.open(
            'Registration successful! Redirecting to login...', 
            'Close', 
            { 
              duration: 3000,
              panelClass: ['success-snackbar']
            }
          );
          
          // Redirect to login after a short delay
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 1500);
        },
        error: (error) => {
          this.isLoading = false;
          const errMsg = error.error?.error[0]?.description;
          this.errorMessage = errMsg || 'Registration failed. Please try again.';
          console.error('Registration error:', error);
          
          // Show error snackbar
          this.snackBar.open(
            this.errorMessage, 
            'Close', 
            { 
              duration: 5000,
              panelClass: ['error-snackbar'] 
            }
          );
        }
      });
    }
  }
}