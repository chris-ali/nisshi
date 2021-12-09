import { Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FuseNavigationItem } from '@fuse/components/navigation/navigation.types';
import { LogbookOptions } from 'app/core/preferences/preferences.types';

@Component({
    selector     : 'logbook-sidebar',
    template     : `
        <div class="py-10">

            <div class="mx-6 text-3xl font-bold tracking-tighter">Options</div>

            <div class="flex-auto p-6 sm:p-5">
                <form
                    [formGroup]="form"
                    (ngSubmit)="onSubmit()"
                    class="flex flex-col mt-4 px-8 pt-4 overflow-hidden">
                    <div class="flex flex-col gt-sm:flex-row">
                        <span class="font-semibold mb-2">Show/Hide Columns</span>
                        <div class="flex flex-col">
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showTailNumber'"
                                [color]="'primary'">
                                Tail Number
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showTypeName'"
                                [color]="'primary'">
                                Type
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showApproaches'"
                                [color]="'primary'">
                                Approaches
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showLandings'"
                                [color]="'primary'">
                                Landings
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showNightLandings'"
                                [color]="'primary'">
                                Night Landings
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showFullStopLandings'"
                                [color]="'primary'">
                                Full Stop Landings
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showNight'"
                                [color]="'primary'">
                                Night
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showCrossCountry'"
                                [color]="'primary'">
                                Cross Country
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showPIC'"
                                [color]="'primary'">
                                PIC
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showSIC'"
                                [color]="'primary'">
                                SIC
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showMultiEngine'"
                                [color]="'primary'">
                                Multi
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showSimulatedInstrument'"
                                [color]="'primary'">
                                Simulated IMC
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showIMC'"
                                [color]="'primary'">
                                Actual IMC
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showDualReceived'"
                                [color]="'primary'">
                                Dual Received
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showDualGiven'"
                                [color]="'primary'">
                                Dual Given
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showGroundSim'"
                                [color]="'primary'">
                                Ground Sim
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showTotalTime'"
                                [color]="'primary'">
                                Total Time
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'showComments'"
                                [color]="'primary'">
                                Comments
                            </mat-checkbox>
                        </div>
                    </div>

                    <div class="flex items-center -mx-8 px-8 py-4">
                        <button
                            class="px-6 ml-3"
                            mat-flat-button
                            [color]="'primary'">
                            Save
                        </button>
                    </div>
                </form>
            </div>

            <fuse-vertical-navigation
                [appearance]="'default'"
                [navigation]="menuData"
                [inner]="true"
                [mode]="'side'"
                [name]="'logbook-sidebar-navigation'"
                [opened]="true"></fuse-vertical-navigation>
        </div>
    `,
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

    menuData: FuseNavigationItem[];
    form: FormGroup;

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
            showSimulatedInstrument: [true],
            showIMC: [true],
            showDualReceived: [true],
            showDualGiven: [true],
            showGroundSim: [true],
            showTotalTime: [true],
            showComments: [true]
        });

        this.form.patchValue(this.logbookOptions);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    onSubmit(): void
    {
        this.preferencesChanged.emit(this.form.value);
    }
}
