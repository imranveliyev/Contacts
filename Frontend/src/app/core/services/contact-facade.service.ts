import { Injectable } from '@angular/core';
import { ContactApiService } from './contact-api.service';
import { ContactStateService } from './contact-state.service';
import { Contact } from '../models/contact';
import { tap, delay } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable()
export class ContactFacadeService {

  constructor(
    private contactApi: ContactApiService,
    private contactState: ContactStateService) { 
      this.load();
  }

  get contacts$() {
    return this.contactState.contacts$;
  }

  get(id: string): Observable<Contact> {
    return this.contactApi.getById(id);
  }

  create(contact: Contact) : Observable<Contact> {
    return this.contactApi.create(contact).pipe(
      tap(data => this.contactState.create(data)));
  }

  remove(id: string) : Observable<void> {
    return this.contactApi.remove(id).pipe(
      tap(() => this.contactState.remove(id)));
  }

  searchByPhone(phone: string): Observable<Array<Contact>> {
    return this.contactApi.searchByPhone(phone);
  }

  private load() {
    this.contactApi.getAll()
      .pipe(delay(1000))
      .subscribe(data => this.contactState.set(data));
  }
}
