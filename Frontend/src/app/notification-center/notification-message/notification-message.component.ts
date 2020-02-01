import { Component, OnInit, Input } from '@angular/core';
import { Notification } from '../notification';

@Component({
  selector: 'app-notification-message',
  templateUrl: './notification-message.component.html',
  styleUrls: ['./notification-message.component.scss']
})
export class NotificationMessageComponent implements OnInit {

  @Input() message: Notification;

  constructor() { }

  ngOnInit() {
  }

}
