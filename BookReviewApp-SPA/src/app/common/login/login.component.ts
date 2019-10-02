import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { Router } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import { User } from 'src/app/_models/user';

/**
 * @description Component that shows the login Form
 */
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  /**
   * @description EventEmitter that fires an event when the User wants to see the Register Form,
   * by clicking the Register Button.
   */
  @Output() enableRegister = new EventEmitter<void>();

  /**
   * @description Variable that stores the User's data
   */
  userInfo: User = { username: '', password: '' };
  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {}

  /**
   * @description Function that checks User credentials
   */
  login() {
    this.authService.login(this.userInfo).subscribe(
      next => {
        // Showing success message and navigating the User
        this.alertify.success('Successful Login');
        this.router.navigate(['user']);
      },
      error => {
        // Showing error message
        this.alertify.error(error);
      }
    );
  }

  /**
   * @description Function that fires the event that shows the User the Register Form
   */
  enableRegisterMode() {
    this.enableRegister.emit();
  }
}
