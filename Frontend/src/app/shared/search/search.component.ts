import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ContactFacadeService } from 'src/app/core/services/contact-facade.service';
import { fromEvent, of, timer } from 'rxjs';
import { map, debounceTime, filter, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { Contact } from 'src/app/core/models/contact';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {

  @ViewChild('searchBox', null) searchBox: ElementRef;
  searchForm: FormGroup;
  searchResults: Array<Contact> = [];

  constructor(private contactFacade: ContactFacadeService) { }

  get phone() { return this.searchForm.get('phone') };

  ngOnInit() {
    fromEvent<any>(this.searchBox.nativeElement, 'input').pipe(
      debounceTime(500),
      map(event => (event.target as HTMLInputElement).value),
      distinctUntilChanged(),
      switchMap(phone => phone.length > 0 ? this.contactFacade.searchByPhone(phone) : of([]))
    ).subscribe(result => {
      this.searchResults = result;
    });

    this.searchForm = new FormGroup({
      phone: new FormControl(null)
    });
  }

  onBlur() {
    timer(500).subscribe(() => this.searchResults = []);
  }
}
