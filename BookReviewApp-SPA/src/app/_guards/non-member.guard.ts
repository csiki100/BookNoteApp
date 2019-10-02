import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';


/**
 * @description Guard that redirects not logged in Users from pages they shouldn't go to
 */
@Injectable({
  providedIn: 'root'
})
export class NonMemberGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  /**
   * Function that determines the if the not logged in User should be redirected or not.
   */
  canActivate(): boolean {
    if (this.authService.ifLoggedIn.value) {
      return true;
    }

    this.alertify.error('You need to be logged in for that!');
    this.router.navigate(['home']);
    return false;
  }
}
