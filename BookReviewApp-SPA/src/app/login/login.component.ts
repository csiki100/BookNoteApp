import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  @Output() enableRegister = new EventEmitter();
  userInfo: any = {};
  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.userInfo).subscribe(next => {
      console.log('sikeres');
    }, error => {
        console.log(error);
      }, () => {
        this.router.navigate(['user']);
    }
    );
  }

  enableRegisterMode() {
    this.enableRegister.emit(null);
  }

}
