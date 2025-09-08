import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({ selector: 'app-signup', templateUrl: 'signup.component.html' , imports : [ReactiveFormsModule, CommonModule]})
export class SignupComponent {
  form : FormGroup;
  error = '';

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {
    this.form = this.fb.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });
  }

  submit() {
    if (this.form.invalid) return;
    this.auth.signup(this.form.value).subscribe({
      next: _ => this.router.navigate(['/login']),
      error: err => this.error = err.error || 'Signup failed'
    });
  }
}