import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ConfirmationService } from 'app/core/confirmation/confirmation.service';
import { UserService } from 'app/core/user/user.service';

@Component({
    selector       : 'settings-profile',
    templateUrl    : './profile.component.html',
    encapsulation: ViewEncapsulation.None
})
export class SettingsAccountComponent implements OnInit
{
    form: FormGroup;
    showCFIExpiration = false;

    /**
     * Constructor
     */
    constructor(
        private formBuilder: FormBuilder,
        private userService: UserService,
        private confirmation: ConfirmationService,
        private router: Router
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    //   -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        // Create the form
        this.form = this.formBuilder.group({
            username: ['', Validators.required],
            firstName: [''],
            lastName: [''],
            lastBFR: [''],
            lastMedical: [''],
            license: [''],
            certificateNumber: [''],
            isInstructor: [false],
            cfiExpiration: ['']
        });

        // Populate form with current user profile
        this.userService.get().subscribe(user => {
            this.form.patchValue(user);
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    onSubmit(): void
    {
        this.userService.update(this.form.value).subscribe({
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
