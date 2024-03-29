import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MaintenanceEntryService } from 'app/core/maintenanceentry/maintenanceentry.service';
import { VehicleService } from 'app/core/vehicle/vehicle.service';
import { Vehicle } from 'app/core/vehicle/vehicle.types';
import { AppConfig } from 'app/core/config/app.config';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ConfigService } from 'app/core/config/config.service';
import { ConfirmationAdapter } from 'app/core/confirmation/confirmation.adapter';

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
    idVehicle: number;
    isAddMode: boolean;
    form: FormGroup;
    vehicle: Vehicle;
    appConfig: AppConfig;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(private formBuilder: FormBuilder,
                private maintenanceEntryService: MaintenanceEntryService,
                private vehicleService: VehicleService,
                private configService: ConfigService,
                private route: ActivatedRoute,
                private router: Router,
                private confirmation: ConfirmationAdapter)
    {}

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngOnInit(): void
    {
        this.id = parseInt(this.route.snapshot.params['id'] ?? '0');
        this.idVehicle = parseInt(this.route.snapshot.params['idVehicle'] ?? '0');
        this.isAddMode = !this.id;

        this.vehicleService.getOne(this.idVehicle)
            .subscribe(veh => this.vehicle = veh);

        this.form = this.formBuilder.group({
            id: [this.id],
            datePerformed: [null],
            milesPerformed: [0, [Validators.min(0), Validators.max(999999)]],
            type: [0, [Validators.min(0), Validators.max(2)]],
            workDescription: ['', Validators.maxLength(500)],
            performedBy: ['', Validators.maxLength(500)],
            duration: [0, [Validators.min(0), Validators.max(1000)]],
            repairPrice: [0, [Validators.min(0), Validators.max(100000)]],
            idVehicle: [this.idVehicle],
        });

        if (!this.isAddMode)
        {
            this.maintenanceEntryService.getOne(this.id)
                .subscribe(entry => {
                    this.form.patchValue(entry);
                });
        }

        // Subscribe to config changes
        this.configService.config$
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
         this._unsubscribeAll.next(1);
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
                    this.router.navigate([`/maintenance/view/${this.vehicle.id}`]);
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
                    this.router.navigate([`/maintenance/view/${this.vehicle.id}`]);
                    this.confirmation.alert('Updated Successfully', 'Maintenance entry was updated successfully!', true);
                },
                error: error => {
                    this.confirmation.alert('An error has occurred', this.confirmation.formatErrors(error));
                }
            });
    }
}
