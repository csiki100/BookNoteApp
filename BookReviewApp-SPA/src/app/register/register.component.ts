import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  constructor(private authService: AuthService, private fb: FormBuilder,
              private router: Router) { }

  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;

  ngOnInit() {
    this.createRegisterForm();
  }

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

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value
      ? null
      : { mismatch: true };
  }

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
  register() {
    if (this.registerForm.valid) {
      const user = Object.assign({}, this.registerForm.value);
      this.authService.register(user).subscribe(() => {
        this.router.navigate(['user']);
      }, error => {
        console.log(error);
      });
    }
  }

  cancelRegisterMode() {
    this.cancelRegister.emit(false);
  }
}
