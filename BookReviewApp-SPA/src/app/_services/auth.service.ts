import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) {}

  register(user: any) {
    return this.http.post(this.baseUrl + 'register', user);
  }

  login(user: any) {
    return this.http.post(this.baseUrl + 'login', user).pipe(
      map((res: any) => {
        if (res) {
          localStorage.setItem('token', res.token);
          this.decodedToken= this.jwtHelper.decodeToken(res.token);
          console.log(this.decodedToken);
        }
      })
    );
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

}
