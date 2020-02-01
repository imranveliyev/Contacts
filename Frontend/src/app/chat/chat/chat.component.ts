import { Component, OnInit, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { AccountFacadeService } from 'src/app/account/services/account-facade.service';
import { Account } from 'src/app/account/models/account';
import { TokenStorageService } from 'src/app/account/services/token-storage.service';
import { NotificationService } from 'src/app/notification-center/notification.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnDestroy {

  connection: HubConnection;
  message: string;
  account: Account;

  messages: Array<string> = [];

  constructor(
    private accountFacade: AccountFacadeService,
    private tokenStorage: TokenStorageService,
    private notifications: NotificationService) { }

  async ngOnInit() {
    this.accountFacade.account$.subscribe(account => this.account = account);

    this.connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/hubs/chat', 
      { accessTokenFactory: () => this.tokenStorage.accessToken })
      .build();

    this.connection.on('serverMessage', (username, message) => {
      this.messages.push(`${username}: ${message}`);
      this.notifications.push(username, message);
    });

    try {
      await this.connection.start();
      this.messages.push('Connected!');
    } catch (error) {
      console.error(error);
    }
  }

  ngOnDestroy(): void {
    this.connection.stop();
  }

  async onSendClick() {
    try {
      let username = this.account ? this.account.username : 'Guest';
      await this.connection.send('NewMessage', username, this.message);
      this.message = ''; 
    } catch (error) {
      console.error(error);
    }
  }
}
