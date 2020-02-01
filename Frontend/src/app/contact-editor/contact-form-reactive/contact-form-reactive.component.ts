import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { Router } from '@angular/router';
import { ContactFacadeService } from 'src/app/core/services/contact-facade.service';

@Component({
  selector: 'app-contact-form-reactive',
  templateUrl: './contact-form-reactive.component.html',
  styleUrls: ['./contact-form-reactive.component.scss']
})
export class ContactFormReactiveComponent implements OnInit {

  contactForm: FormGroup;

  constructor(
    private contactFacade: ContactFacadeService,
    private router: Router,

  ) { }

  get name() { return this.contactForm.get('name'); }
  get surname() { return this.contactForm.get('surname'); }
  get phone() { return this.contactForm.get('phone'); }
  get email() { return this.contactForm.get('email'); }
  get additionalPhones() { return this.contactForm.get('additionalPhones') as FormArray; }

  ngOnInit() {
    this.contactForm = new FormGroup({
      name: new FormControl(null, {
        validators: [Validators.required, Validators.maxLength(100)]
      }),
      surname: new FormControl(null, {
        validators: [Validators.maxLength(100)]
      }),
      phone: new FormControl(null, {
        validators: [Validators.pattern('[0-9,-]{2,100}'), Validators.required, Validators.maxLength(100)]
      }),
      email: new FormControl(null, {
        validators: [Validators.email, Validators.maxLength(100)]
      }),
      additionalPhones: new FormArray([])
    });
  }

  onAddPhoneClick() {
    this.additionalPhones.push(new FormControl());
  }

  onPhoneDeleteClick(index: number) {
    this.additionalPhones.removeAt(index);
  }

  onSubmit() {
    this.contactFacade.create(this.contactForm.value).subscribe(
      contact => this.router.navigate(['/contact', contact.id])
    );
  }
}
