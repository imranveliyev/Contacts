import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Contact } from 'src/app/core/models/contact';

@Component({
  selector: 'app-contact-list-item',
  templateUrl: './contact-list-item.component.html',
  styleUrls: ['./contact-list-item.component.scss']
})
export class ContactListItemComponent {

  @Input() contact: Contact;
  @Output() delete: EventEmitter<Contact> = new EventEmitter<Contact>();

  onDeleteClick() {
    this.delete.emit(this.contact);
  }

}
