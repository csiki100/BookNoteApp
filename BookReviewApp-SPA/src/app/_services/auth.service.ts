import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { User } from '../_models/user';


/**
 * @description Service that manages User Authentication.
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  /**
   * @description the base of the Url that will be used.
   */
  baseUrl = environment.apiUrl + 'auth/';

  /**
   * @description Service that helps int JWT handling.
   */
  jwtHelper = new JwtHelperService();

  /**
   * @description the user's decoded JWT token.
   */
  decodedToken: any;

  /**
   * @description Object that stores the user's authentication status and if any changes occur,
   * notifies the rest of the Application.
   */
  ifLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    this.checkLoggedIn()
  );

  constructor(private http: HttpClient) {}

  /**
   * @description Function that registers a User through a POST request to the API.
   * @param user the User's Username and Password.
   */
  register(user: User) {
    return this.http.post(this.baseUrl + 'register', user);
  }

  /**
   * @description Function that logs in a User through a POST request to the API.
   * @param user the User's Username and Password.
   */
  login(user: User) {
    return this.http.post(this.baseUrl + 'login', user).pipe(
      map((res: any) => {
        if (res) {
          localStorage.setItem('token', res.token);
          this.decodedToken = this.jwtHelper.decodeToken(res.token);
          this.ifLoggedIn.next(true);
        }
      })
    );
  }


  /**
   * @description Function that checks if a User is logged in.
   */
  private checkLoggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
