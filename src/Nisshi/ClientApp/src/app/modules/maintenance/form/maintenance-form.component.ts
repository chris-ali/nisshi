import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MaintenanceEntryService } from 'app/core/maintenanceentry/maintenanceentry.service';
import { ConfirmationService } from 'app/core/confirmation/confirmation.service';
import { VehicleService } from 'app/core/vehicle/vehicle.service';
import { Vehicle } from 'app/core/vehicle/vehicle.types';
import { AppConfig } from 'app/core/config/app.config';
import { Subject } from 'rxjs';
import { FuseConfigService } from '@fuse/services/config';
import { takeUntil } from 'rxjs/operators';

/**
 * Form that adds/edits an maintenance
 */
@Component({
    selector: 'maintenance-form',
    templateUrl: './maintenance-form.component.html'
})
export class MaintenanceFormComponent implements OnInit, OnDestroy
{
    id: number;
    isAddMode: boolean;
    form: FormGroup;
    vehicle: Vehicle[];
    appConfig: AppConfig;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(private formBuilder: FormBuilder,
                private maintenanceEntryService: MaintenanceEntryService,
                private vehicleService: VehicleService,
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
            idVehicle: ['', Validators.nullValidator]
        });

        this.vehicleService.getAll()
            .subscribe(veh => this.vehicle = veh);

        if (!this.isAddMode)
        {
            this.maintenanceEntryService.getOne(this.id)
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
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * When submit button is clicked, either create or update maintenance entry object
     */
    onSubmit(): void
    {
        if (this.form.invalid)
            return;

        if (this.isAddMode)
            this.addMaintenanceEntry();
        else
            this.editMaintenanceEntry();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    private addMaintenanceEntry(): void
    {
        this.maintenanceEntryService.create(this.form.value)
            .subscribe({
                next: () => {
                    this.router.navigate(['/maintenance/view']);
                    this.confirmation.alert('Updated Successfully', 'Maintenance entry was created successfully!', true);
                },
                error: error => {
                    this.confirmation.alert('An error has occurred', this.confirmation.formatErrors(error));
                }
            });
    }

    private editMaintenanceEntry(): void
    {
        this.maintenanceEntryService.update(this.form.value)
            .subscribe({
                next: () => {
                    this.router.navigate(['/maintenance/view']);
                    this.confirmation.alert('Updated Successfully', 'Maintenance entry was updated successfully!', true);
                },
                error: error => {
                    this.confirmation.alert('An error has occurred', this.confirmation.formatErrors(error));
                }
            });
    }
}
