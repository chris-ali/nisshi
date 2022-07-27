import { AfterViewInit, ChangeDetectionStrategy, Component, ElementRef, OnInit, QueryList, Renderer2, ViewChildren, ViewEncapsulation } from '@angular/core';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseCardComponent } from '@fuse/components/card';
import { TranslocoService } from '@ngneat/transloco';
import { VehicleService } from 'app/core/vehicle/vehicle.service';
import { Vehicle } from 'app/core/vehicle/vehicle.types';
import { ConfirmationService } from 'app/core/confirmation/confirmation.service';

/**
 * Component that displays all vehicles available to the user,
 * filterable by instance type and editable/deleteable
 */
@Component({
    selector     : 'vehicle-view',
    templateUrl  : './vehicle-view.component.html',
    styles         : [
        `
            fuse-card {
                margin: 16px;
            }
        `
    ],
    encapsulation: ViewEncapsulation.None
})
export class VehicleViewComponent implements AfterViewInit, OnInit
{
    @ViewChildren(FuseCardComponent, {read: ElementRef}) private vehicleCardList: QueryList<ElementRef>;
    vehicles: Vehicle[];
    filters: string[] = ['all', 'simulation', 'real'];
    selectedFilter: string = 'all';
    vehicleCount: any = {};

    /**
     * Constructor
     */
    constructor(private vehicleService: VehicleService,
                public translateService: TranslocoService,
                private confirmation: ConfirmationService,
                private router: Router,
                private route: ActivatedRoute)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngAfterViewInit(): void
    {
        this.vehicleCardList.changes.subscribe(() => {
            this.calculateVehiclePerFilter();
        });
    }

    ngOnInit(): void
    {
        this.vehicleService.getAll().subscribe(vehicles => this.vehicles = vehicles);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Filters cards when a filter badge is clicked
     *
     * @param change
     */
    onFilterChange(change: MatButtonToggleChange): void
    {
        this.selectedFilter = change.value;
        this.filterVehicleCards();
    }

    /**
     * When the edit menu item is clicked; redirects to edit view
     *
     * @param air
     */
    editClick(air: Vehicle): void
    {
        this.router.navigate([`../edit/${air.id}`], { relativeTo: this.route });
    }

    /**
     * When the delete menu item is clicked; opens confirm dialog and then deletes the vehicle
     *
     * @param veh
     */
    deleteClick(veh: Vehicle): void
    {
        var message = "Are you sure you want to delete this vehicle? <span class=\"font-medium\">This action will remove all logbook entries associated with it and cannot be undone!</span>";
        const confirmDelete = this.confirmation.confirm("Delete Vehicle", message, "Delete", "Cancel");

        confirmDelete.afterClosed().subscribe(result => {
            if (result == "confirmed")
            {
                this.vehicleService.delete(veh.id).subscribe({
                    next: () => {
                        this.vehicles = this.vehicles.filter(x => x.id != veh.id);
                        this.vehicleCardList.filter(x => x.nativeElement.id == veh.id).pop()
                            .nativeElement.classList.add("hidden");

                        this.confirmation.alert("Vehicle Deleted", "Vehicle was successfully deleted!", true);
                    },
                    error: error => {
                        this.confirmation.alert("An error was encountered!", error);
                    }
                });
            }
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Calculates how many vehicle for the user match each filter type
     */
    private calculateVehiclePerFilter(): void
    {
        this.vehicleCount = {};

        let count = 0;

        this.filters.forEach(filter => {
            if (filter == 'all')
            {
                count = this.vehicleCardList.length;
            }
            else
            {
                count = this.vehicleCount[filter] = this.vehicleCardList
                    .filter(card => card.nativeElement.classList.contains(`filter-${filter}`)).length;
            }

            this.vehicleCount[filter] = count;
        });
    }

    /**
     * Filters vehicle cards by setting hidden class, depending on the filter selected
     */
    private filterVehicleCards(): void
    {
        this.vehicleCardList.forEach(card =>  {
            var classList = card.nativeElement.classList;
            if (this.selectedFilter == 'all')
            {
                classList.remove('hidden');
            }
            else
            {
                if (classList.contains(`filter-${this.selectedFilter}`))
                    classList.remove('hidden');
                else
                    classList.add('hidden');
            }
        });
    }
}
