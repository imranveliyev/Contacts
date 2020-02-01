import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContactListRoutingModule } from './contact-list-routing.module';

import { ContactListComponent } from './contact-list/contact-list.component';
import { ContactListItemComponent } from './contact-list-item/contact-list-item.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    ContactListComponent,
    ContactListItemComponent
  ],
  imports: [
    CommonModule,
    ContactListRoutingModule,
    SharedModule
  ]
})
export class ContactListModule { }
