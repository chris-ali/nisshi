import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'app/core/auth/auth.service';
import { ConfirmationService } from 'app/core/confirmation/confirmation.service';

@Component({
    selector       : 'settings-security',
    templateUrl    : './security.component.html',
    encapsulation  : ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class SettingsSecurityComponent implements OnInit
{
    form: FormGroup;

    /**
     * Constructor
     */
    constructor(
        private formBuilder: FormBuilder,
        private authService: AuthService,
        private confirmation: ConfirmationService,
        private router: Router
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        var pattern = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,20}$";

        // Create the form
        this.form = this.formBuilder.group({
            oldPassword: ['', [Validators.required]],
            newPassword: ['', [Validators.required, Validators.pattern(pattern)]],
            repeatPassword: ['', [Validators.required, Validators.pattern(pattern)]]
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    onSubmit(): void
    {
        this.authService.changePassword(this.form.value).subscribe({
            next: () => {
                this.router.navigate(['/']);
                this.confirmation.alert('Updated Successfully', 'Profile was updated successfully!', true);
            },
            error: error => {
                this.confirmation.alert('An error has occurred', this.confirmation.formatErrors(error));
            }
        })
    }
}
