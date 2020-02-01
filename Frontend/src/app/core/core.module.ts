import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContactApiService } from './services/contact-api.service';
import { ContactStateService } from './services/contact-state.service';
import { ContactFacadeService } from './services/contact-facade.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    ContactApiService,
    ContactStateService,
    ContactFacadeService
  ]
})
export class CoreModule { }
