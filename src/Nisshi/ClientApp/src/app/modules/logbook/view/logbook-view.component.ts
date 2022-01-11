import { Component, OnInit, ViewEncapsulation, ViewChild, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { ColumnMode } from '@swimlane/ngx-datatable';
import { ConfirmationService } from 'app/core/confirmation/confirmation.service';
import { LogbookEntryService } from 'app/core/logbookentry/logbookentry.service';
import { LogbookEntry } from 'app/core/logbookentry/logbookentry.types';
import { AppConfig, LogbookOptions } from 'app/core/config/app.config';
import { FuseConfigService } from '@fuse/services/config';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { entries } from 'lodash';

@Component({
  selector: 'logbook-view',
  templateUrl: './logbook-view.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./logbook-view.scss']
})
export class LogbookViewComponent implements OnInit, OnDestroy
{
    @ViewChild('logbookTable') table: any;
    enableSummary = true;
    summaryPosition = 'bottom';
    ColumnMode = ColumnMode;
    logbookEntries: LogbookEntry[];

    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = false;

    appConfig: AppConfig;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(private logbookEntryService: LogbookEntryService,
                public translateService: TranslocoService,
                private fuseConfigService: FuseConfigService,
                private confirmation: ConfirmationService,
                private router: Router,
                private route: ActivatedRoute)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngOnInit(): void
    {
        this.logbookEntryService.getAll().subscribe(entries => {
            this.logbookEntries = entries;
        });

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
     * Toggles the display of details for a row
     *
     * @param row
     */
    toggleExpandRow(row: any): void
    {
        this.table.rowDetail.toggleExpandRow(row);
    }

    /**
     * Sums the total of all rows for a single column
     *
     * @param cells
     * @returns sum of column
     */
    sumColumn(cells: number[]): number
    {
        const filteredCells = cells.filter(cell => !!cell);
        return filteredCells.reduce((sum, cell) => (sum += cell), 0);
    }

    /**
     * Toggles the display of details for a row when the row is clicked
     *
     * @param selected
     */
    rowClick(selected: any) {
        if (selected.type == 'click')
            this.toggleExpandRow(selected.row);
    }

    /**
     * When the edit menu item is clicked; redirects to edit view
     *
     * @param row
     */
     editClick(row: any): void
     {
         this.router.navigate([`../edit/${row.id}`], { relativeTo: this.route });
     }

    /**
     * When the delete button for a row is clicked; opens confirm dialog and then deletes the logbook entry
     *
     * @param row
     */
    deleteClick(row: any): void
    {
        var message = "Are you sure you want to delete this logbook entry?";
        const confirmDelete = this.confirmation.confirm("Delete Logbook Entry", message, "Delete", "Cancel");

        confirmDelete.afterClosed().subscribe(result => {
            if (result == "confirmed")
            {
                this.logbookEntryService.delete(row.id).subscribe({
                    next: () => {
                        this.logbookEntries = this.logbookEntries.filter(x => x.id != row.id);
                        this.confirmation.alert("Aircraft Deleted", "Aircraft was successfully deleted!", true);
                    },
                    error: error => {
                        this.confirmation.alert("An error was encountered!", error);
                    }
                });
            }
        });
    }

    /**
     * When the sidebar updates logbook options, update them here as well
     *
     * @param options
     */
    onAppConfigChanged(options: LogbookOptions): void
    {
        this.fuseConfigService.config = {logbookOptions: options};
    }

    /**
     * When the sidebar updates the filter, update the list of logbook entries
     *
     * @param filter
     */
    onFilterChanged(filter): void
    {
        var filterArray = [];

        if (filter.fromDate)
            filterArray.push(` flightDate gt ${filter.fromDate.toISOString()}`)
        if (filter.toDate)
            filterArray.push(` flightDate lt ${filter.toDate.toISOString()}`)
        if (filter.idAircraft)
            filterArray.push(` idAircraft eq ${filter.idAircraft}`)
        if (filter.instanceType)
            filterArray.push(` aircraft/instanceType eq Nisshi.Infrastructure.Enums.InstanceType'${filter.instanceType}'`)

        var filterQuery = filterArray.length > 0 ? `filter=${filterArray.join(' and ')}` : '';

        this.logbookEntryService.getAll(filterQuery).subscribe(entries => {
            this.logbookEntries = entries;
        });
    }
}
