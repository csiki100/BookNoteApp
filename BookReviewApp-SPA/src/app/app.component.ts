import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  /**
   * @description Object that enables token manipulation
   */
  jwtHelper = new JwtHelperService();
  /**
   * @description Value that stores if a User is logged in
   */
  loggedIn: boolean;
  constructor(private authService: AuthService) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    this.authService.ifLoggedIn.subscribe(value => {
      this.loggedIn = value;
    });
  }
}
