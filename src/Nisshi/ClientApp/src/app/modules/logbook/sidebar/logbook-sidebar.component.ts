import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FuseNavigationItem } from '@fuse/components/navigation/navigation.types';
import { UserService } from 'app/core/user/user.service';

@Component({
    selector     : 'logbook-sidebar',
    template     : `
        <div class="py-10">

            <div class="mx-6 text-3xl font-bold tracking-tighter">Logbook Options</div>

            <div class="flex-auto p-6 sm:p-10">
                <form
                    [formGroup]="form"
                    (ngSubmit)="onPreferencesChanged()"
                    class="flex flex-col mt-4 px-8 pt-10 bg-card shadow rounded overflow-hidden">
                    <div class="flex flex-col gt-sm:flex-row">
                        <span class="font-semibold mb-2">Show/Hide Columns</span>
                        <div class="flex flex-col">
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showTailNumber'"
                                [color]="'primary'">
                                Tail Number
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showTypeName'"
                                [color]="'primary'">
                                Type
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showApproaches'"
                                [color]="'primary'">
                                Approaches
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showLandings'"
                                [color]="'primary'">
                                Landings
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showNightLandings'"
                                [color]="'primary'">
                                Night Landings
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showFullStopLandings'"
                                [color]="'primary'">
                                Full Stop Landings
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showPIC'"
                                [color]="'primary'">
                                PIC
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showSIC'"
                                [color]="'primary'">
                                SIC
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showMultiEngine'"
                                [color]="'primary'">
                                Multi
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showSimulatedInstrument'"
                                [color]="'primary'">
                                Simulated IMC
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showIMC'"
                                [color]="'primary'">
                                Actual IMC
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showDualReceived'"
                                [color]="'primary'">
                                Dual Received
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showDualGiven'"
                                [color]="'primary'">
                                Dual Given
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showGroundSim'"
                                [color]="'primary'">
                                Ground Sim
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showTotalTime'"
                                [color]="'primary'">
                                Total Time
                            </mat-checkbox>
                            <mat-checkbox
                                class="mb-2"
                                [formControlName]="'logbook.showComments'"
                                [color]="'primary'">
                                Comments
                            </mat-checkbox>
                        </div>
                    </div>

                    <div class="flex items-center justify-end border-t -mx-8 mt-8 px-8 py-5 bg-gray-50 dark:bg-gray-700">
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
    menuData: FuseNavigationItem[];
    form: FormGroup;

    /**
     * Constructor
     */
    constructor(private userService: UserService,
                private formBuilder: FormBuilder)
    {
        this.menuData = [
            {
                title   : 'Actions',
                subtitle: 'Task, project & team',
                type    : 'group',
                children: [
                    {
                        title: 'Create task',
                        type : 'basic',
                        icon : 'heroicons_outline:plus-circle'
                    },
                    {
                        title: 'Create team',
                        type : 'basic',
                        icon : 'heroicons_outline:user-group'
                    },
                    {
                        title: 'Create project',
                        type : 'basic',
                        icon : 'heroicons_outline:briefcase'
                    },
                    {
                        title: 'Create user',
                        type : 'basic',
                        icon : 'heroicons_outline:user-add'
                    },
                    {
                        title   : 'Assign user or team',
                        subtitle: 'Assign to a task or a project',
                        type    : 'basic',
                        icon    : 'heroicons_outline:badge-check'
                    }
                ]
            },
            {
                title   : 'Tasks',
                type    : 'group',
                children: [
                    {
                        title: 'All tasks',
                        type : 'basic',
                        icon : 'heroicons_outline:clipboard-list',
                        badge: {
                            title  : '49',
                            classes: 'px-2 bg-primary text-on-primary rounded-full'
                        }
                    },
                    {
                        title: 'Ongoing tasks',
                        type : 'basic',
                        icon : 'heroicons_outline:clipboard-copy'
                    },
                    {
                        title: 'Completed tasks',
                        type : 'basic',
                        icon : 'heroicons_outline:clipboard-check'
                    },
                    {
                        title: 'Abandoned tasks',
                        type : 'basic',
                        icon : 'heroicons_outline:clipboard'
                    },
                    {
                        title: 'Assigned to me',
                        type : 'basic',
                        icon : 'heroicons_outline:user'
                    },
                    {
                        title: 'Assigned to my team',
                        type : 'basic',
                        icon : 'heroicons_outline:users'
                    }
                ]
            },
            {
                type: 'divider'
            }
        ];
    }

    ngOnInit(): void
    {
        this.formBuilder.group({

        });

        this.userService.get().subscribe(user => {
            //this.form.patchValue(user.preferences);
        })
    }

    onPreferencesChanged(): void
    {

    }
}
