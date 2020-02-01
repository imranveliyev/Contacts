import { Component } from '@angular/core';
import { AccountFacadeService } from 'src/app/account/services/account-facade.service';

@Component({
  selector: 'app-account-menu',
  templateUrl: './account-menu.component.html',
  styleUrls: ['./account-menu.component.scss']
})
export class AccountMenuComponent {

  constructor(
    public accountFacade: AccountFacadeService) { }

}
