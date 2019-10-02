import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';
import { Router } from '@angular/router';


/**
 * @description Component that shows the navbar
 */
@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  constructor(private authService: AuthService, private alertify: AlertifyService,
              private router: Router) { }

  ngOnInit() {}

  /**
   * @description Function that logs out the User
   */
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.alertify.success('Successful Logout');
    this.router.navigate(['home']);
    this.authService.ifLoggedIn.next(false);
  }
}
