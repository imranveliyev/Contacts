import { environment } from '../../../environments/environment';

import { Injectable } from '@angular/core';
import { Contact } from '../models/contact';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class ContactApiService {

  readonly url: string = environment.contactsApiUrl;

  constructor(private httpClient: HttpClient) {}

  getAll(): Observable<Array<Contact>> {
    return this.httpClient.get<Array<Contact>>(this.url);      
  }

  getById(id: string): Observable<Contact> {
    return this.httpClient.get<Contact>(`${this.url}/${id}`);      
  }

  create(contact: Contact): Observable<Contact> {
    return this.httpClient.post<Contact>(this.url, contact);      
  }

  remove(id: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.url}/${id}`);      
  }

  searchByPhone(phone: string) : Observable<Array<Contact>> {
    return this.httpClient.get<Array<Contact>>(`${this.url}/search/${phone}`);      
  }
}
