import { Component, OnInit, OnDestroy, ViewEncapsulation, ViewChild } from '@angular/core';
import { ColumnMode } from '@swimlane/ngx-datatable';
import { LogbookEntryService } from 'app/core/logbookentry/logbookentry.service';
import { LogbookEntry } from 'app/core/logbookentry/logbookentry.types';

@Component({
  selector: 'logbook-view',
  templateUrl: './logbook-view.component.html',
  encapsulation: ViewEncapsulation.None
})
export class LogbookViewComponent implements OnInit
{
    @ViewChild('logbookTable') table: any;

    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;

    ColumnMode = ColumnMode;

    logbookEntries: LogbookEntry[];

    constructor(private logbookEntryService: LogbookEntryService) { }

    ngOnInit(): void
    {
        this.logbookEntryService.getAll().subscribe(entries => {
            this.logbookEntries = entries;
        });
    }

    toggleExpandRow(row: any): void
    {
        this.table.rowDetail.toggleExpandRow(row);
    }
}
