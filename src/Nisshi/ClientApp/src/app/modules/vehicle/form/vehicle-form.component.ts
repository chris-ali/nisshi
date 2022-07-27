import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { VehicleService } from 'app/core/vehicle/vehicle.service';
import { ConfirmationService } from 'app/core/confirmation/confirmation.service';
import { ManufacturerService } from 'app/core/manufacturer/manufacturer.service';
import { Manufacturer } from 'app/core/manufacturer/manufacturer.types';
import { ModelService } from 'app/core/model/model.service';
import { Model } from 'app/core/model/model.types';

/**
 * Form that adds/edits an vehicle
 */
@Component({
    selector: 'vehicle-form',
    templateUrl: './vehicle-form.component.html'
})
export class VehicleFormComponent implements OnInit
{
    id: number;
    isAddMode: boolean;
    form: FormGroup;
    models: Model[];
    manufacturers: Manufacturer[];
    showDates = true;

    /**
     * Constructor
     */
    constructor(private formBuilder: FormBuilder,
                private vehicleService: VehicleService,
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
            vin: ['', [Validators.maxLength(17)]],
            make: ['', [Validators.required, Validators.maxLength(40)]],
            model: ['', [Validators.required, Validators.maxLength(40)]],
            trim: ['', [Validators.required, Validators.maxLength(40)]],
            year: [2000, [Validators.required, Validators.min(0), Validators.max(9999)]],
            lastRegistration: [null],
            registrationDue: [null],
            lastInspection: [null],
            inspectionDue: [null],
            miles: [0, [Validators.min(0), Validators.max(9999999)]],
            notes: [''],
        });

        if (!this.isAddMode)
        {
            this.vehicleService.getOne(this.id)
                .subscribe(vehicle => {
                    this.form.patchValue(vehicle);
                });
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * When submit button is clicked, either create or update vehicle object
     */
    onSubmit(): void
    {
        if (this.form.invalid)
            return;

        if (this.isAddMode)
            this.addVehicle();
        else
            this.editVehicle();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    private addVehicle(): void
    {
        this.vehicleService.create(this.form.value)
            .subscribe({
                next: () => {
                    this.router.navigate(['/vehicle/view']);
                    this.confirmation.alert('Updated Successfully', 'Vehicle was created successfully!', true);
                },
                error: error => {
                    this.confirmation.alert('An error has occurred', this.confirmation.formatErrors(error));
                }
            });
    }

    private editVehicle(): void
    {
        this.vehicleService.update(this.form.value)
            .subscribe({
                next: () => {
                    this.router.navigate(['/vehicle/view']);
                    this.confirmation.alert('Updated Successfully', 'Vehicle was updated successfully!', true);
                },
                error: error => {
                    this.confirmation.alert('An error has occurred', this.confirmation.formatErrors(error));
                }
            });
    }
}
