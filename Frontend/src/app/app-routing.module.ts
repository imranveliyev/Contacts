import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ErrorComponent } from './shared/error/error.component';
import { AuthGuard } from './account/guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'list', pathMatch: 'full' },
  { 
    path: 'list',
    loadChildren: () => import('./contact-list/contact-list.module').then(m => m.ContactListModule)
  },
  { 
    path: 'contact',
    loadChildren: () => import('./contact-details/contact-details.module').then(m => m.ContactDetailsModule)
  },
  { 
    path: 'editor',
    canLoad: [AuthGuard],
    loadChildren: () => import('./contact-editor/contact-editor.module').then(m => m.ContactEditorModule)
  },
  { 
    path: 'chat',
     canActivate: [AuthGuard],
    loadChildren: () => import('./chat/chat.module').then(m => m.ChatModule)
  },
  { 
    path: 'account',
    loadChildren: () => import('./account/account.module').then(m => m.AccountModule)
  },
  { path: '**', component: ErrorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
