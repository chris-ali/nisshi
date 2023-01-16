import { Component, ElementRef, OnInit, QueryList, ViewChildren, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CardComponent } from '@coreui/angular';
import { TranslocoService } from '@ngneat/transloco';
import { VehicleService } from 'app/core/vehicle/vehicle.service';
import { Vehicle } from 'app/core/vehicle/vehicle.types';
import { ConfirmationAdapter } from 'app/core/confirmation/confirmation.adapter';

/**
 * Component that displays all vehicles available to the user,
 * filterable by instance type and editable/deleteable
 */
@Component({
    selector     : 'vehicle-view',
    templateUrl  : './vehicle-view.component.html',
    styles         : [
        `
            c-card {
                margin: 16px;
            }
        `
    ],
    encapsulation: ViewEncapsulation.None
})
export class VehicleViewComponent implements OnInit
{
    @ViewChildren(CardComponent, {read: ElementRef}) private vehicleCardList: QueryList<ElementRef>;
    vehicles: Vehicle[];
    vehicleCount: any = {};

    /**
     * Constructor
     */
    constructor(private vehicleService: VehicleService,
                public translateService: TranslocoService,
                private confirmation: ConfirmationAdapter,
                private router: Router,
                private route: ActivatedRoute)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngOnInit(): void
    {
        this.vehicleService.getAll().subscribe(vehicles => this.vehicles = vehicles);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

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
}
