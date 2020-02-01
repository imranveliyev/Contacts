import { Component, OnInit } from '@angular/core';
import { AccountFacadeService } from '../services/account-facade.service';
import { Account } from "../models/account";

@Component({
  selector: 'app-account-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.scss']
})
export class AccountListComponent implements OnInit {

  accounts: Array<Account> = [];

  constructor(private accountFacade: AccountFacadeService) { }

  ngOnInit() {
    this.accountFacade.getAll().subscribe(accounts => {
      this.accounts = accounts;
    });
  }

}
