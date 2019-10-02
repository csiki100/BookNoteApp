import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';


/**
 * @description Class that wraps AlertifyJs as a service
 */
@Injectable({
  providedIn: 'root'
})
export class AlertifyService {
  constructor() {}

  /**
   * @description Function that shows a confirmation window
   * @param message message that will be shown in the confirmation window
   * @param okCallback callback that will be executed if the User clicks on "Ok"
   */
  confirm(message: string, okCallback: () => any) {
    alertify.confirm('Confirmation', message, okCallback, {});
  }

  /**
   * @description Function that shows a Success Alert
   * @param message message that will be shown in the Alert
   */
  success(message: string) {
    alertify.success(message);
  }

  /**
   * @description Function that shows an Error Alert
   * @param message message that will be shown in the Alert
   */
  error(message: string) {
    alertify.error(message);
  }
}
