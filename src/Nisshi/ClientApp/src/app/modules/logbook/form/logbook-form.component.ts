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
import { Preferences } from 'app/core/preferences/preferences.types';
import { PreferencesService } from 'app/core/preferences/preferences.service';

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
    preferences: Preferences;

    /**
     * Constructor
     */
    constructor(private formBuilder: FormBuilder,
                private logbookEntryService: LogbookEntryService,
                private aircraftService: AircraftService,
                private airportService: AirportService,
                private preferencesService: PreferencesService,
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
            flightDate: [null],
            numInstrumentApproaches: [0, [Validators.min(0), Validators.max(999999)]],
            numLandings: [0, [Validators.min(0), Validators.max(999999)]],
            numNightLandings: [0, [Validators.min(0), Validators.max(999999)]],
            numFullStopLandings: [0, [Validators.min(0), Validators.max(999999)]],
            crossCountry: [0, [Validators.min(0), Validators.max(1000)]],
            multiEngine: [0, [Validators.min(0), Validators.max(1000)]],
            night: [0, [Validators.min(0), Validators.max(1000)]],
            imc: [0, [Validators.min(0), Validators.max(1000)]],
            simulatedInstrument: [0, [Validators.min(0), Validators.max(1000)]],
            dualReceived: [0, [Validators.min(0), Validators.max(1000)]],
            dualGiven: [0, [Validators.min(0), Validators.max(1000)]],
            pic: [0, [Validators.min(0), Validators.max(1000)]],
            sic: [0, [Validators.min(0), Validators.max(1000)]],
            groundSim: [0, [Validators.min(0), Validators.max(1000)]],
            hobbsStart: [0, [Validators.min(0), Validators.max(999999)]],
            hobbsEnd: [0, [Validators.min(0), Validators.max(999999)]],
            totalFlightTime: [0, [Validators.min(0), Validators.max(1000)]],
            route: ['', [Validators.required, Validators.maxLength(60)]],
            comments: ['', Validators.maxLength(500)],
            idAircraft: ['', Validators.nullValidator]
        });

        this.aircraftService.getAll()
            .subscribe(airs => this.aircraft = airs);

        if (!this.isAddMode)
        {
            this.logbookEntryService.getOne(this.id)
                .subscribe(entry => {
                    this.form.patchValue(entry);
                });
        }

        this.preferences = this.preferencesService.preferences$;
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
