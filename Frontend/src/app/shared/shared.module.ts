import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { RouterModule } from '@angular/router';
import { ErrorComponent } from './error/error.component';
import { SearchComponent } from './search/search.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalComponent } from './modal/modal.component';
import { AccountMenuComponent } from './account-menu/account-menu.component';

@NgModule({
  declarations: [
    HeaderComponent, 
    FooterComponent,
    ErrorComponent,
    SearchComponent,
    ModalComponent,
    AccountMenuComponent
  ],
  imports: [
    RouterModule,
    CommonModule,
    ReactiveFormsModule
  ],
  exports: [
    HeaderComponent,
    FooterComponent,
    ErrorComponent,
    ModalComponent
  ]
})
export class SharedModule { }
