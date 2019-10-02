import { Component, OnInit } from '@angular/core';


/**
 * @description Component that shows the Home Page of the Appliction
 */
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  /**
   * @description Variable that stores which Form is present on the page at the moment
   */
  registerMode = false;
  
  constructor() { }

  ngOnInit() { }

  /**
   * @description Function that changes the login Form into register Form and vice versa
   */
  changeMode() {
    this.registerMode = !this.registerMode;
  }

}
