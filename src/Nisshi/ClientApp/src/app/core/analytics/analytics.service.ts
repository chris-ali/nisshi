import { Injectable } from '@angular/core';
import { forkJoin, Observable, Subject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ApiService } from '../base/api.service';
import { TotalsAnalytics, LandingsAnalytics, AnalyticsCompendium } from './analytics.types';

const URL = 'analytics/';

@Injectable({
    providedIn: 'root'
})
export class AnalyticsService
{
    private _analyticsData: Subject<AnalyticsCompendium> = new Subject<AnalyticsCompendium>();

    /**
     * Constructor
     */
    constructor(private _api: ApiService) { }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    get analytics$(): Observable<AnalyticsCompendium>
    {
        return this._analyticsData.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get the current logged in user data
     */
    get(): any
    {
        forkJoin([
            this.getTotalsByMonth(),
            this.getTotalsByCatClass(),
            this.getTotalsByType(),
            this.getTotalsByInstanceType(),
            this.getLandingsApproachesPast90Days()
        ]).subscribe(([byMonth, byCatClass, byType, byInstanceType, landings]) => {
            var analytics: AnalyticsCompendium =
            {
                totalsByMonth: byMonth,
                totalsByCatClass: byCatClass,
                totalsByType: byType,
                totalsByInstance: byInstanceType,
                landingsPast90Days: landings
            };

            this._analyticsData.next(analytics);
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Gets totals of all logbook entries grouped by month and year
     */
    getTotalsByMonth(): Observable<TotalsAnalytics[]>
    {
        return this._api.get(`${URL}totals/month`).pipe();
    }

    /**
     * Gets totals of all logbook entries grouped by category and class
     */
    getTotalsByCatClass(): Observable<TotalsAnalytics[]>
    {
        return this._api.get(`${URL}totals/catclass`).pipe();
    }

    /**
     * Gets totals of all logbook entries grouped by instance type (real vs sim)
     */
    getTotalsByInstanceType(): Observable<TotalsAnalytics[]>
    {
        return this._api.get(`${URL}totals/instance`).pipe();
    }

    /**
     * Gets totals of all logbook entries grouped by aircraft type
     */
    getTotalsByType(): Observable<TotalsAnalytics[]>
    {
        return this._api.get(`${URL}totals/type`).pipe();
    }

    /**
     * Gets totals of all logbook entries grouped by month and year
     */
    getLandingsApproachesPast90Days(): Observable<LandingsAnalytics>
    {
        return this._api.get(`${URL}currency/landings`).pipe();
    }
}
