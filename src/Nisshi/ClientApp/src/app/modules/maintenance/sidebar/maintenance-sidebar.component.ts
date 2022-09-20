import { Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FuseNavigationItem } from '@fuse/components/navigation/navigation.types';
import { MaintenanceOptions } from 'app/core/config/app.config';

@Component({
    selector     : 'maintenance-sidebar',
    templateUrl  : './maintenance-sidebar.component.html',
    styles       : [
        `
            maintenance-sidebar fuse-vertical-navigation .fuse-vertical-navigation-wrapper {
                box-shadow: none !important;
            }
        `
    ],
    encapsulation: ViewEncapsulation.None
})
export class MaintenanceSidebarComponent implements OnInit
{
    @Input() maintenanceOptions: MaintenanceOptions;
    @Output() preferencesChanged = new EventEmitter();
    @Output() filterChanged = new EventEmitter();

    menuData: FuseNavigationItem[];
    form: FormGroup;
    filter: FormGroup;

    /**
     * Constructor
     */
    constructor(private formBuilder: FormBuilder)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngOnInit(): void
    {
        this.form = this.formBuilder.group({
            showMilesPerformed: [true],
            showType: [true],
            showWorkDescription: [true],
            showPerformedBy: [true],
            showDuration: [true],
            showRepairPrice: [true],
        });

        this.form.patchValue(this.maintenanceOptions);

        this.filter = this.formBuilder.group({
            fromDate: [''],
            toDate: [''],
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Called when options form is submitted
     */
    onSubmit(): void
    {
        this.preferencesChanged.emit(this.form.value);
    }

    /**
     * Called when filter form is submitted
     */
    onFilter(): void
    {
        this.filterChanged.emit(this.filter.value);
    }

    /**
     * Called when filter form is cleared
     */
    onClearFilter(): void
    {
        this.filter.reset();
        this.filterChanged.emit(this.filter.value);
    }
}
