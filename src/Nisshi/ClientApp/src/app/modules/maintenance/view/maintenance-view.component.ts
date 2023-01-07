import { Component, OnInit, ViewEncapsulation, ViewChild, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { ColumnMode } from '@swimlane/ngx-datatable';
import { ConfirmationService } from 'app/core/confirmation/confirmation.service';
import { MaintenanceEntryService } from 'app/core/maintenanceentry/maintenanceentry.service';
import { MaintenanceEntry } from 'app/core/maintenanceentry/maintenanceentry.types';
import { AppConfig, MaintenanceOptions } from 'app/core/config/app.config';
import { FuseConfigService } from '@fuse/services/config';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { VehicleService } from 'app/core/vehicle/vehicle.service';
import { MaintenanceFilter } from 'app/core/ui/maintenancefilter.types';
import { Vehicle } from 'app/core/vehicle/vehicle.types';

@Component({
  selector: 'maintenance-view',
  templateUrl: './maintenance-view.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./maintenance-view.scss']
})
export class MaintenanceViewComponent implements OnInit, OnDestroy
{
    @ViewChild('maintenanceTable') table: any;
    enableSummary = true;
    summaryPosition = 'bottom';
    ColumnMode = ColumnMode;
    id: number;
    maintenanceEntries: MaintenanceEntry[];
    vehicle: Vehicle;
    activeFilters: string[];

    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = false;

    appConfig: AppConfig;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(private maintenanceEntryService: MaintenanceEntryService,
                private vehicleService: VehicleService,
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
        this.id = parseInt(this.route.snapshot.params['id'] ?? '0');

        this.maintenanceEntryService.getAll(` idVehicle eq ${this.id} `).subscribe(entries => {
            this.maintenanceEntries = entries;
        });

        this.vehicleService.getOne(this.id).subscribe (veh => {
            this.vehicle = veh;
        });

        this.activeFilters = [];

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
    rowClick(selected: any)
    {
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
        this.router.navigate([`../../${this.vehicle.id}/edit/${row.id}`], { relativeTo: this.route });
    }

    /**
     * When the add new menu item is clicked; redirects to add new view
     */
    addClick(): void
    {
        this.router.navigate([`../../${this.vehicle.id}/add`], { relativeTo: this.route });
    }

    /**
     * When the delete button for a row is clicked; opens confirm dialog and then deletes the maintenance entry
     *
     * @param row
     */
    deleteClick(row: any): void
    {
        var message = "Are you sure you want to delete this maintenance entry?";
        const confirmDelete = this.confirmation.confirm("Delete Maintenance Entry", message, "Delete", "Cancel");

        confirmDelete.afterClosed().subscribe(result => {
            if (result == "confirmed")
            {
                this.maintenanceEntryService.delete(row.id).subscribe({
                    next: () => {
                        this.maintenanceEntries = this.maintenanceEntries.filter(x => x.id != row.id);
                        this.confirmation.alert("Maintenance Entry Deleted", "Maintenance entry was successfully deleted!", true);
                    },
                    error: error => {
                        this.confirmation.alert("An error was encountered!", error);
                    }
                });
            }
        });
    }

    /**
     * When the sidebar updates maintenance options, update them here as well
     *
     * @param options
     */
    onAppConfigChanged(options: MaintenanceOptions): void
    {
        this.fuseConfigService.config = {maintenanceOptions: options};
    }

    /**
     * When the sidebar updates the filter, update the list of maintenance entries
     *
     * @param filter
     */
    onFilterChanged(filter: MaintenanceFilter): void
    {
        var filterArray = [];
        this.activeFilters = [];

        filterArray.push(` idVehicle eq ${this.id}`);

        if (filter?.fromDate)
        {
            filterArray.push(` datePerformed gt ${filter.fromDate.toISOString()}`);
            this.activeFilters.push(`After: ${filter.fromDate.toDateString()}`);
        }
        if (filter?.toDate)
        {
            filterArray.push(` datePerformed lt ${filter.toDate.toISOString()}`);
            this.activeFilters.push(`Before: ${filter.toDate.toDateString()}`);
        }

        var filterQuery = filterArray.length > 0 ? `filter=${filterArray.join(' and ')}` : '';

        this.maintenanceEntryService.getAll(filterQuery).subscribe(entries => {
            this.maintenanceEntries = entries;
        });
    }
}
