import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';

/**
 * @description Component that shows the Register Form
 */
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  /**
   * @description EventEmitter that fires an event when the User wants to see the Login Form,
   * by clicking the Cancel Button.
   */
  @Output() cancelRegister = new EventEmitter<undefined>();
  /**
   * @description the Register Form
   */
  registerForm: FormGroup;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.createRegisterForm();
  }

  /**
   * @description Function that creates the Register Form
   */
  createRegisterForm() {
    this.registerForm = this.fb.group(
      {
        username: [
          '',
          [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(25)
          ]
        ],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(20)
          ]
        ],
        confirmPassword: ['', Validators.required]
      },
      {
        validator: [this.passwordMatchValidator, this.specialCharacterValidator]
      }
    );
  }

  /**
   * @description Validator that checks if the password and confirm password fields match
   * @param g the Form
   */
  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value
      ? null
      : { mismatch: true };
  }

  /**
   * @description Validator that checks if the password field has any special characters
   * @param g the Form
   */
  specialCharacterValidator(g: FormGroup) {
    const chars = ['!', '@', '#', '$', '%', '^', '&', '*', '?', '|'];
    const password = String(g.get('password').value);
    let match = false;

    for (const char of chars) {
      if (password.includes(char)) {
        match = true;
      }
    }
    if (match) {
      return null;
    }
    return { noSpecialChar: true };
  }

  /**
   * @description Function that registers a new User
   */
  register() {
    if (this.registerForm.valid) {
      const user = Object.assign({}, this.registerForm.value);

      this.authService.register(user).subscribe(
        () => {
          // Logging in user
          this.authService.login(user).subscribe(() => {
            // Showing success message and navigating the User
            this.alertify.success('Registration successful');
            this.router.navigate(['user']);
          });
        },
        error => {
          // Showing error message
          this.alertify.error(error);
        }
      );
    }
  }

  /**
   * @description Function that fires the event that shows the User the Login Form
   */
  cancelRegisterMode() {
    this.cancelRegister.emit();
  }
}
