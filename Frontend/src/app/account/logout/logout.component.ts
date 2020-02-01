import { Component, OnInit } from '@angular/core';
import { AccountFacadeService } from '../services/account-facade.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements OnInit {

  constructor(
    private accountFacade: AccountFacadeService
  ) { }

  ngOnInit() {
    this.accountFacade.logout().subscribe();
  }

}
