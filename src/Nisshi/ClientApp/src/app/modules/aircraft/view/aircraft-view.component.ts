import { AfterViewInit, ChangeDetectionStrategy, Component, ElementRef, OnInit, QueryList, Renderer2, ViewChildren, ViewEncapsulation } from '@angular/core';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { ActivatedRoute, Router } from '@angular/router';
import { CardComponent } from '@coreui/angular';
import { TranslocoService } from '@ngneat/transloco';
import { AircraftService } from 'app/core/aircraft/aircraft.service';
import { Aircraft } from 'app/core/aircraft/aircraft.types';
import { ConfirmationAdapter } from 'app/core/confirmation/confirmation.adapter';

/**
 * Component that displays all aircraft available to the user,
 * filterable by instance type and editable/deleteable
 */
@Component({
    selector     : 'aircraft-view',
    templateUrl  : './aircraft-view.component.html',
    styles         : [
        `
            c-card {
                margin: 16px;
            }
        `
    ],
    encapsulation: ViewEncapsulation.None
})
export class AircraftViewComponent implements AfterViewInit, OnInit
{
    @ViewChildren(CardComponent, {read: ElementRef}) private aircraftCardList: QueryList<ElementRef>;
    aircraft: Aircraft[];
    filters: string[] = ['all', 'simulation', 'real'];
    selectedFilter: string = 'all';
    aircraftCount: any = {};

    /**
     * Constructor
     */
    constructor(private aircraftService: AircraftService,
                public translateService: TranslocoService,
                private confirmation: ConfirmationAdapter,
                private router: Router,
                private route: ActivatedRoute)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngAfterViewInit(): void
    {
        this.aircraftCardList.changes.subscribe(() => {
            this.calculateAircraftPerFilter();
        });
    }

    ngOnInit(): void
    {
        this.aircraftService.getAll().subscribe(aircraft => this.aircraft = aircraft);
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
        this.filterAircraftCards();
    }

    /**
     * When the edit menu item is clicked; redirects to edit view
     *
     * @param air
     */
    editClick(air: Aircraft): void
    {
        this.router.navigate([`../edit/${air.id}`], { relativeTo: this.route });
    }

    /**
     * When the delete menu item is clicked; opens confirm dialog and then deletes the aircraft
     *
     * @param air
     */
    deleteClick(air: Aircraft): void
    {
        var message = "Are you sure you want to delete this aircraft? <span class=\"font-medium\">This action will remove all logbook entries associated with it and cannot be undone!</span>";
        const confirmDelete = this.confirmation.confirm("Delete Aircraft", message, "Delete", "Cancel");

        confirmDelete.afterClosed().subscribe(result => {
            if (result == "confirmed")
            {
                this.aircraftService.delete(air.id).subscribe({
                    next: () => {
                        this.aircraft = this.aircraft.filter(x => x.id != air.id);
                        this.aircraftCardList.filter(x => x.nativeElement.id == air.id).pop()
                            .nativeElement.classList.add("hidden");

                        this.confirmation.alert("Aircraft Deleted", "Aircraft was successfully deleted!", true);
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
     * Calculates how many aircraft for the user match each filter type
     */
    private calculateAircraftPerFilter(): void
    {
        this.aircraftCount = {};

        let count = 0;

        this.filters.forEach(filter => {
            if (filter == 'all')
            {
                count = this.aircraftCardList.length;
            }
            else
            {
                count = this.aircraftCount[filter] = this.aircraftCardList
                    .filter(card => card.nativeElement.classList.contains(`filter-${filter}`)).length;
            }

            this.aircraftCount[filter] = count;
        });
    }

    /**
     * Filters aircraft cards by setting hidden class,
     *  depending on the filter selected
     */
    private filterAircraftCards(): void
    {
        this.aircraftCardList.forEach(card =>  {
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
