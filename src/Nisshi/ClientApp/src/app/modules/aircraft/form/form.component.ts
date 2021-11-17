import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AircraftService } from 'app/core/aircraft/aircraft.service';
import { first } from 'rxjs/operators';

/**
 * Form that adds/edits an aircraft
 */
@Component({
    selector: 'aircraft-form',
    templateUrl: './form.component.html'
})
export class FormComponent implements OnInit {

    id: string;
    isAddMode: boolean;
    form: FormGroup;

    /**
     * Constructor
     */
    constructor(private formBuilder: FormBuilder,
                private aircraftService: AircraftService,
                private route: ActivatedRoute,
                private router: Router)
    {}

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngOnInit(): void
    {
        this.id = this.route.snapshot.params['id'];
        this.isAddMode = !this.id;

        this.formBuilder.group({
            id: [this.id],
            tailNumber: ['', [Validators.required, Validators.maxLength(20)]],
            instanceType: [1],
            lastAnnual: [''],
            lastPitotStatic: [''],
            lastVOR: [''],
            lastAltimeter: [''],
            lastTransponder: [''],
            lastELT: [''],
            last100Hobbs: [''],
            lastOilHobbs: [''],
            lastEngineHobbs: [''],
            registrationDue: [''],
            notes: ['', Validators.maxLength(200)],
            model: ['', Validators.nullValidator]
        });

        if (!this.isAddMode)
        {
            this.aircraftService.getOne(parseInt(this.id))
                .pipe(first())
                .subscribe(aircraft => this.form.patchValue(aircraft));
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    onSubmit(): void
    {

    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    private addAircraft(): void
    {

    }

    private editAircraft(): void
    {

    }
}
