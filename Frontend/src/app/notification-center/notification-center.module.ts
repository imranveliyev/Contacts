import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationCenterComponent } from './notification-center/notification-center.component';
import { NotificationMessageComponent } from './notification-message/notification-message.component';



@NgModule({
  declarations: [
    NotificationCenterComponent,
    NotificationMessageComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    NotificationCenterComponent
  ]
})
export class NotificationCenterModule { }
