import { AfterViewInit, ChangeDetectionStrategy, Component, ElementRef, QueryList, Renderer2, ViewChildren, ViewEncapsulation } from '@angular/core';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { FuseCardComponent } from '@fuse/components/card';
import { AircraftService } from 'app/core/aircraft/aircraft.service';
import { Aircraft } from 'app/core/aircraft/aircraft.types';

@Component({
    selector     : 'aircraft',
    templateUrl  : './view.component.html',
    styles         : [
        `
            cards fuse-card {
                margin: 16px;
            }
        `
    ],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ViewComponent implements AfterViewInit
{
    @ViewChildren(FuseCardComponent, {read: ElementRef}) private aircraftCardList: QueryList<ElementRef>;
    aircraft: Aircraft[];
    filters: string[] = ['all', 'sim', 'real'];
    aircraftCount: any = {};
    selectedFilter: string = 'all';

    /**
     * Constructor
     */
    constructor(private renderer2: Renderer2,
                private aircraftService: AircraftService)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngAfterViewInit(): void
    {
        this.calculateAircraftPerFilter();
        this.aircraftService.getAll().subscribe(aircraft => this.aircraft = aircraft);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * When a filter badge is clicked, refresh the list
     *
     * @param change
     */
    onFilterChange(change: MatButtonToggleChange): void
    {
        this.selectedFilter = change.value;
        this.aircraftService.getAll().subscribe(aircraft => this.aircraft = aircraft);
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
                    .filter(card => card.nativeElement.classList.contains('filter-' + filter)).length;
            }

            this.aircraftCount[filter] = count;
        });
    }
}
