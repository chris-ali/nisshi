import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AircraftService } from 'app/core/aircraft/aircraft.service';
import { ConfirmationService } from 'app/core/confirmation/confirmation.service';
import { ManufacturerService } from 'app/core/manufacturer/manufacturer.service';
import { Manufacturer } from 'app/core/manufacturer/manufacturer.types';
import { ModelService } from 'app/core/model/model.service';
import { Model } from 'app/core/model/model.types';

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
    models: Model[];
    manufacturers: Manufacturer[];

    /**
     * Constructor
     */
    constructor(private formBuilder: FormBuilder,
                private aircraftService: AircraftService,
                private manufacturerService: ManufacturerService,
                private modelService: ModelService,
                private route: ActivatedRoute,
                private router: Router,
                private confirmation: ConfirmationService)
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
            last100Hobbs: ['', [Validators.min(0), Validators.max(999999)]],
            lastOilHobbs: ['', [Validators.min(0), Validators.max(999999)]],
            lastEngineHobbs: ['', [Validators.min(0), Validators.max(999999)]],
            registrationDue: [''],
            notes: [''],
            idModel: ['', Validators.nullValidator]
        });

        this.manufacturerService.getAll()
            .subscribe(manus => this.manufacturers = manus);

        if (!this.isAddMode)
        {
            this.aircraftService.getOne(parseInt(this.id))
                .subscribe(aircraft => this.form.patchValue(aircraft));
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
                    this.router.navigate(['../view'], { relativeTo: this.route });
                    this.confirmation.alert('Updated Successfully', 'Aircraft was created successfully!', true);
                },
                error: error => {
                    this.confirmation.alert('An error has occurred', error);
                }
            });
    }

    private editAircraft(): void
    {
        this.aircraftService.update(this.form.value)
            .subscribe({
                next: () => {
                    this.router.navigate(['../view'], { relativeTo: this.route });
                    this.confirmation.alert('Updated Successfully', 'Aircraft was updated successfully!', true);
                },
                error: error => {
                    this.confirmation.alert('An error has occurred', error);
                }
            });
    }
}
