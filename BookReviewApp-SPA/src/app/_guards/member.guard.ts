import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';


/**
 * @description Guard that redirects logged in Users from pages they shouldn't go to
 */
@Injectable({
  providedIn: 'root'
})
export class MemberGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) { }


    /**
     * Function that determines the if the User should be redirected or not.
     */
  canActivate(): boolean {
    if (!this.authService.ifLoggedIn.value) {
      return true;
    }

    this.router.navigate(['user']);
    return false;
  }
}
