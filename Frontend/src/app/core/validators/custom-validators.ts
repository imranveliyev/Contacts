import { ValidatorFn, FormControl, ValidationErrors, FormGroup, AsyncValidatorFn } from '@angular/forms';
import { AccountFacadeService } from 'src/app/account/services/account-facade.service';
import { Observable, of } from 'rxjs';
import { catchError, map, delay } from 'rxjs/operators';

export class CustomValidators {

    static forbidden(regexp: RegExp): ValidatorFn {
        return (control: FormControl): ValidationErrors => {
            var email = control.value;
            if (regexp.test(email))
                return { forbidden: `Word ${regexp.source} is forbidden!` };
            return null;
        }
    }

    static sameValue(first: string, second: string): ValidatorFn {
        return (group: FormGroup): ValidationErrors => {
            var firstValue = group.get(first).value;
            var secondValue = group.get(second).value;
            if (firstValue != secondValue)
                return { sameValue: `${first} and ${second} not match!` };
            return null;
        }
    }

    static emailTaken(accountFacade: AccountFacadeService): AsyncValidatorFn {
        return (control: FormControl): Observable<ValidationErrors> => {
            let username = control.value;
            return accountFacade.search(username).pipe(
                catchError(error => of(null)),
                delay(2000),
                map(response => response ? { emailTaken: 'This email is already taken!' } : null)
            );
        }
    }

}