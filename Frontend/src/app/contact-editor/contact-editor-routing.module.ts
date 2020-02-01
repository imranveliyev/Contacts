import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ContactFormReactiveComponent } from './contact-form-reactive/contact-form-reactive.component';


const routes: Routes = [
  { path: '', component: ContactFormReactiveComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContactEditorRoutingModule { }
