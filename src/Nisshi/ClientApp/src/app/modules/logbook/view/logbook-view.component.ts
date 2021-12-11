import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { ColumnMode } from '@swimlane/ngx-datatable';
import { ConfirmationService } from 'app/core/confirmation/confirmation.service';
import { LogbookEntryService } from 'app/core/logbookentry/logbookentry.service';
import { LogbookEntry } from 'app/core/logbookentry/logbookentry.types';
import { AppConfigService } from 'app/core/config/appconfig.service';
import { AppConfig, LogbookOptions } from 'app/core/config/app.config';

@Component({
  selector: 'logbook-view',
  templateUrl: './logbook-view.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./logbook-view.scss']
})
export class LogbookViewComponent implements OnInit
{
    @ViewChild('logbookTable') table: any;
    enableSummary = true;
    summaryPosition = 'bottom';
    ColumnMode = ColumnMode;
    logbookEntries: LogbookEntry[];

    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = false;

    appConfig: AppConfig;

    /**
     * Constructor
     */
    constructor(private logbookEntryService: LogbookEntryService,
                public translateService: TranslocoService,
                private appConfigService: AppConfigService,
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

        this.appConfig = this.appConfigService.appConfig$;
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
        this.appConfig.logbookOptions = options;
        this.appConfigService.appConfig = this.appConfig;
    }
}
