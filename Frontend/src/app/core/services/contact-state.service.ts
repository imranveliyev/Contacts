import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Contact } from '../models/contact';

@Injectable()
export class ContactStateService {

  private contacts: BehaviorSubject<Array<Contact>>;
  
  constructor() {
    this.contacts = new BehaviorSubject(null);
  }

  get contacts$() {
    return this.contacts.asObservable();
  }

  set(contacts: Array<Contact>) {
    this.contacts.next(contacts);
  }

  create(contact: Contact) {
    this.contacts.next([...this.contacts.getValue(), contact]);
  }  

  remove(id: string) {
    this.contacts.next(this.contacts.getValue().filter(x => x.id != id));
  }
}
