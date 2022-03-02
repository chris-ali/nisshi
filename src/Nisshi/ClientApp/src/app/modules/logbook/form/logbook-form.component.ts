import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatAutocomplete} from '@angular/material/autocomplete';
import { ActivatedRoute, Router } from '@angular/router';
import { LogbookEntryService } from 'app/core/logbookentry/logbookentry.service';
import { ConfirmationService } from 'app/core/confirmation/confirmation.service';
import { AircraftService } from 'app/core/aircraft/aircraft.service';
import { AirportService } from 'app/core/airport/airport.service';
import { Aircraft } from 'app/core/aircraft/aircraft.types';
import { Airport } from 'app/core/airport/airport.types';
import { AppConfig } from 'app/core/config/app.config';
import { Subject } from 'rxjs';
import { FuseConfigService } from '@fuse/services/config';
import { takeUntil } from 'rxjs/operators';

/**
 * Form that adds/edits an logbook
 */
@Component({
    selector: 'logbook-form',
    templateUrl: './logbook-form.component.html'
})
export class LogbookFormComponent implements OnInit, OnDestroy
{
    id: number;
    isAddMode: boolean;
    form: FormGroup;
    aircraft: Aircraft[];
    airports: Airport[];
    appConfig: AppConfig;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    routeControl = new FormControl();
    @ViewChild('airportInput') airportInput: ElementRef<HTMLInputElement>;
    @ViewChild('autocomplete') autocomplete: MatAutocomplete;

    /**
     * Constructor
     */
    constructor(private formBuilder: FormBuilder,
                private logbookEntryService: LogbookEntryService,
                private aircraftService: AircraftService,
                private airportService: AirportService,
                private fuseConfigService: FuseConfigService,
                private route: ActivatedRoute,
                private router: Router,
                private confirmation: ConfirmationService)
    {}

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngOnInit(): void
    {
        this.id = parseInt(this.route.snapshot.params['id'] ?? '0');
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
            turbine: [0, [Validators.min(0), Validators.max(1000)]],
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

        // Subscribe to config changes
        this.fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                this.appConfig = config;
            });
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
     * On destroy
     */
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Autocomplete chip events
    // -----------------------------------------------------------------------------------------------------

    // add(event: MatChipInputEvent): void {
    //     // Add fruit only when MatAutocomplete is not open
    //     // To make sure this does not conflict with OptionSelected Event
    //     if (!this.autocomplete.isOpen) {
    //       const input = event.input;
    //       const value = event.value;

    //       // Add our fruit
    //       if ((value || '').trim()) {
    //         this.fruits.push(value.trim());
    //       }

    //       // Reset the input value
    //       if (input) {
    //         input.value = '';
    //       }

    //       this.fruitCtrl.setValue(null);
    //     }
    //   }

    //   remove(fruit: string): void {
    //     const index = this.fruits.indexOf(fruit);

    //     if (index >= 0) {
    //       this.fruits.splice(index, 1);
    //     }
    //   }

    //   selected(event: MatAutocompleteSelectedEvent): void {
    //     this.fruits.push(event.option.viewValue);
    //     this.fruitInput.nativeElement.value = '';
    //     this.fruitCtrl.setValue(null);
    //   }

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
                    this.confirmation.alert('An error has occurred', this.confirmation.formatErrors(error));
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
                    this.confirmation.alert('An error has occurred', this.confirmation.formatErrors(error));
                }
            });
    }
}
