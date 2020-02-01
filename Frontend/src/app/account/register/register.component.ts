import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AccountFacadeService } from '../services/account-facade.service';
import { Router } from '@angular/router';
import { CustomValidators } from 'src/app/core/validators/custom-validators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;

  get email() { return this.registerForm.get('email'); }
  get password() { return this.registerForm.get('password'); }
  get passwordConfirmation() { return this.registerForm.get('passwordConfirmation'); }

  constructor(
    private accountFacade: AccountFacadeService,
    private router: Router) { }

  ngOnInit() {
    this.registerForm = new FormGroup({
      email: new FormControl(null, {
        validators: [Validators.email, Validators.required, CustomValidators.forbidden(/admin/i)],
        asyncValidators: [CustomValidators.emailTaken(this.accountFacade)]
      }),
      password: new FormControl(null, [Validators.required]),
      passwordConfirmation: new FormControl(null, [Validators.required]),
    }, {
      validators: [CustomValidators.sameValue('password', 'passwordConfirmation')],
      updateOn: 'blur'
    });
  }

  onSubmit() {
    this.accountFacade.register(this.registerForm.value).subscribe(
      () => {
        console.log(`Success!`);
        this.router.navigate(['account/login']);
      },
      error => {
        console.error(error);
      }
    );
  }

}
