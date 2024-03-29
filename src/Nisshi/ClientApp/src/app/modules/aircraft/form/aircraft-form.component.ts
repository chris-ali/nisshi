import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AircraftService } from 'app/core/aircraft/aircraft.service';
import { ConfirmationAdapter } from 'app/core/confirmation/confirmation.adapter';
import { ManufacturerService } from 'app/core/manufacturer/manufacturer.service';
import { Manufacturer } from 'app/core/manufacturer/manufacturer.types';
import { ModelService } from 'app/core/model/model.service';
import { Model } from 'app/core/model/model.types';

/**
 * Form that adds/edits an aircraft
 */
@Component({
    selector: 'aircraft-form',
    templateUrl: './aircraft-form.component.html'
})
export class AircraftFormComponent implements OnInit
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
                private aircraftService: AircraftService,
                private manufacturerService: ManufacturerService,
                private modelService: ModelService,
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
            idManufacturer: [0] // Not a field on aircraft, but needed to set dropdown on edit
        });

        this.manufacturerService.getAll()
            .subscribe(manus => this.manufacturers = manus);

        if (!this.isAddMode)
        {
            this.aircraftService.getOne(this.id)
                .subscribe(aircraft => {
                    this.form.patchValue(aircraft);

                    this.models = [];
                    this.modelService.getManyByManufacturer(aircraft.model.idManufacturer)
                        .subscribe(models => {
                            this.models = models
                            this.form.patchValue({idManufacturer: aircraft.model.idManufacturer});
                        });
                });
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * When submit button is clicked, either create or update aircraft object
     */
    onSubmit(): void
    {
        if (this.form.invalid)
            return;

        if (this.isAddMode)
            this.addAircraft();
        else
            this.editAircraft();
    }

    /**
     * On manufacturer dropdown selection change, get
     * models for manufacturers and then fill in models dropdown
     *
     * @param id
     */
    selectedManufacturerChanged(id: number): void
    {
        this.models = [];
        this.modelService.getManyByManufacturer(id)
            .subscribe(models => this.models = models);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    private addAircraft(): void
    {
        this.aircraftService.create(this.form.value)
            .subscribe({
                next: () => {
                    this.router.navigate(['/aircraft/view']);
                    this.confirmation.alert('Updated Successfully', 'Aircraft was created successfully!', true);
                },
                error: error => {
                    this.confirmation.alert('An error has occurred', this.confirmation.formatErrors(error));
                }
            });
    }

    private editAircraft(): void
    {
        this.aircraftService.update(this.form.value)
            .subscribe({
                next: () => {
                    this.router.navigate(['/aircraft/view']);
                    this.confirmation.alert('Updated Successfully', 'Aircraft was updated successfully!', true);
                },
                error: error => {
                    this.confirmation.alert('An error has occurred', this.confirmation.formatErrors(error));
                }
            });
    }
}
