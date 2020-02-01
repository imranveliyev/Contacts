import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContactEditorRoutingModule } from './contact-editor-routing.module';
import { ContactFormReactiveComponent } from './contact-form-reactive/contact-form-reactive.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    ContactFormReactiveComponent
  ],
  imports: [
    CommonModule,
    ContactEditorRoutingModule,
    ReactiveFormsModule
  ]
})
export class ContactEditorModule { }
