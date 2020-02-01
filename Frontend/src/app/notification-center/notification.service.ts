import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Notification } from './notification';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private notifications: BehaviorSubject<Array<Notification>>;

  get notifications$() { return this.notifications.asObservable(); }

  constructor() {
    this.notifications = new BehaviorSubject<Array<Notification>>([]);
  }

  push(title: string, content: string) {
    let newNotification = { title, content };
    this.notifications.next([...this.notifications.getValue(), newNotification]);
  }
}
