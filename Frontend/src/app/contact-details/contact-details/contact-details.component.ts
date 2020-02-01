import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Contact } from 'src/app/core/models/contact';
import { ContactFacadeService } from 'src/app/core/services/contact-facade.service';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-contact-details',
  templateUrl: './contact-details.component.html',
  styleUrls: ['./contact-details.component.scss']
})
export class ContactDetailsComponent implements OnInit {

  contact: Contact;

  constructor(
    private contactFacade: ContactFacadeService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.route.paramMap.pipe(
      switchMap(params => this.contactFacade.get(params.get('id')))
    ).subscribe(contact => {
      this.contact = contact;
    });
  }

  onDelete() {
    this.contactFacade.remove(this.contact.id).subscribe(
      () => this.router.navigate(['']) 
    );
  }
}
