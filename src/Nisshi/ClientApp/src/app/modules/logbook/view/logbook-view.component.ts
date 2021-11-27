import { Component, OnInit, OnDestroy, ViewEncapsulation, ViewChild } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { ColumnMode, SelectionType } from '@swimlane/ngx-datatable';
import { ConfirmationService } from 'app/core/confirmation/confirmation.service';
import { LogbookEntryService } from 'app/core/logbookentry/logbookentry.service';
import { LogbookEntry } from 'app/core/logbookentry/logbookentry.types';

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
    SelectionType = SelectionType;
    logbookEntries: LogbookEntry[];

    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = false;

    /**
     * Constructor
     */
    constructor(private logbookEntryService: LogbookEntryService,
                public translateService: TranslocoService,
                private confirmation: ConfirmationService,)
    { }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngOnInit(): void
    {
        this.logbookEntryService.getAll().subscribe(entries => {
            this.logbookEntries = entries;
        });
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

    onSelect({ selected }) {
        this.toggleExpandRow(selected.pop());
    }
}
