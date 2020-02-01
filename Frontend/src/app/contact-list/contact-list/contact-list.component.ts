import { Component, OnInit } from '@angular/core';
import { ContactApiService } from 'src/app/core/services/contact-api.service';
import { Contact } from 'src/app/core/models/contact';
import { filter, map, delay, timeout } from 'rxjs/operators';
import { timer } from 'rxjs';
import { ContactFacadeService } from 'src/app/core/services/contact-facade.service';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.scss']
})
export class ContactListComponent {

  deleteConfirm: boolean = false;
  contactToDelete: Contact;

  constructor(
    public contactFacade: ContactFacadeService) { }

  onDelete(contact: Contact) {
    this.deleteConfirm = true;
    this.contactToDelete = contact;
  }

  onModalClosed(value: boolean) {
    this.deleteConfirm = false;
    if (value && this.contactToDelete) {
      this.contactFacade.remove(this.contactToDelete.id).subscribe();
      this.contactToDelete = null;
    }
  }
}
