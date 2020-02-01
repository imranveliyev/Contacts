import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: [ 'app.component.scss' ]
})
export class AppComponent implements OnInit {

  connection: HubConnection;

  ngOnInit(): void {
    this.connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/hubs/notifications')
      .build();

    this.connection.on('notification', (type, message) => {
      console.log(`${type}: ${message}`); 
    });

    this.connection.start();
  }

}
