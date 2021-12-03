import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LogbookEntryService } from 'app/core/logbookentry/logbookentry.service';
import { ConfirmationService } from 'app/core/confirmation/confirmation.service';
import { Manufacturer } from 'app/core/manufacturer/manufacturer.types';
import { Model } from 'app/core/model/model.types';
import { AircraftService } from 'app/core/aircraft/aircraft.service';
import { AirportService } from 'app/core/airport/airport.service';
import { Aircraft } from 'app/core/aircraft/aircraft.types';
import { Airport } from 'app/core/airport/airport.types';

/**
 * Form that adds/edits an logbook
 */
@Component({
    selector: 'logbook-form',
    templateUrl: './logbook-form.component.html'
})
export class LogbookFormComponent implements OnInit
{
    id: number;
    isAddMode: boolean;
    form: FormGroup;
    aircraft: Aircraft[];
    airports: Airport[];

    /**
     * Constructor
     */
    constructor(private formBuilder: FormBuilder,
                private logbookEntryService: LogbookEntryService,
                private aircraftService: AircraftService,
                private airportService: AirportService,
                private route: ActivatedRoute,
                private router: Router,
                private confirmation: ConfirmationService)
    {}

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngOnInit(): void
    {
        this.id = parseInt(this.route.snapshot.params['id']) ?? 0;
        this.isAddMode = !this.id;

        this.form = this.formBuilder.group({
            id: [this.id],
            tailNumber: ['', [Validators.required, Validators.maxLength(20)]],
            instanceType: [1],
            lastAnnual: [null],
            lastPitotStatic: [null],
            lastVOR: [null],
            lastAltimeter: [null],
            lastTransponder: [null],
            lastELT: [null],
            last100Hobbs: [0, [Validators.min(0), Validators.max(999999)]],
            lastOilHobbs: [0, [Validators.min(0), Validators.max(999999)]],
            lastEngineHobbs: [0, [Validators.min(0), Validators.max(999999)]],
            registrationDue: [null],
            notes: [''],
            idModel: ['', Validators.nullValidator],
            idManufacturer: [0] // Not a field on logbook, but needed to set dropdown on edit
        });

        this.aircraftService.getAll()
            .subscribe(airs => this.aircraft = airs);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * When submit button is clicked, either create or update logbook entry object
     */
    onSubmit(): void
    {
        if (this.form.invalid)
            return;

        if (this.isAddMode)
            this.addLogbookEntry();
        else
            this.editLogbookEntry();
    }

    /**
     * On manufacturer dropdown selection change, get
     * models for manufacturers and then fill in models dropdown
     *
     * @param id
     */
    selectedManufacturerChanged(id: number): void
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    private addLogbookEntry(): void
    {
        this.logbookEntryService.create(this.form.value)
            .subscribe({
                next: () => {
                    this.router.navigate(['/logbook/view']);
                    this.confirmation.alert('Updated Successfully', 'Logbook entry was created successfully!', true);
                },
                error: error => {
                    this.confirmation.alert('An error has occurred', error);
                }
            });
    }

    private editLogbookEntry(): void
    {
        this.logbookEntryService.update(this.form.value)
            .subscribe({
                next: () => {
                    this.router.navigate(['/logbook/view']);
                    this.confirmation.alert('Updated Successfully', 'Logbook entry was updated successfully!', true);
                },
                error: error => {
                    this.confirmation.alert('An error has occurred', error);
                }
            });
    }
}
