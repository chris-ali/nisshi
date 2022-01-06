import { Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FuseNavigationItem } from '@fuse/components/navigation/navigation.types';
import { AircraftService } from 'app/core/aircraft/aircraft.service';
import { Aircraft } from 'app/core/aircraft/aircraft.types';
import { LogbookOptions } from 'app/core/config/app.config';


@Component({
    selector     : 'logbook-sidebar',
    templateUrl  : './logbook-sidebar.component.html',
    styles       : [
        `
            logbook-sidebar fuse-vertical-navigation .fuse-vertical-navigation-wrapper {
                box-shadow: none !important;
            }
        `
    ],
    encapsulation: ViewEncapsulation.None
})
export class LogbookSidebarComponent implements OnInit
{
    @Input() logbookOptions: LogbookOptions;
    @Output() preferencesChanged = new EventEmitter();
    @Output() filterChanged = new EventEmitter();

    menuData: FuseNavigationItem[];
    form: FormGroup;
    filter: FormGroup;
    aircraft: Aircraft[];

    /**
     * Constructor
     */
    constructor(private formBuilder: FormBuilder,
                private aircraftService: AircraftService)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngOnInit(): void
    {
        this.form = this.formBuilder.group({
            showTailNumber: [true],
            showTypeName: [true],
            showApproaches: [true],
            showLandings: [true],
            showNightLandings: [true],
            showFullStopLandings: [true],
            showNight: [true],
            showCrossCountry: [true],
            showPIC: [true],
            showSIC: [true],
            showMultiEngine: [true],
            showTurbine: [true],
            showSimulatedInstrument: [true],
            showIMC: [true],
            showDualReceived: [true],
            showDualGiven: [true],
            showGroundSim: [true],
            showTotalTime: [true],
            showComments: [true]
        });

        this.form.patchValue(this.logbookOptions);

        this.filter = this.formBuilder.group({
            fromDate: [''],
            toDate: [''],
            idAircraft: ['']
        });

        this.aircraftService.getAll().subscribe(airs => {
            this.aircraft = airs;
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    onSubmit(): void
    {
        this.preferencesChanged.emit(this.form.value);
    }

    onFilter(): void
    {
        this.filterChanged.emit(this.filter.value);
    }
}
