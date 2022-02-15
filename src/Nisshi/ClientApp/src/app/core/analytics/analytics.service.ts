import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../base/api.service';
import { TotalsAnalytics, LandingsAnalytics } from './analytics.types';

const URL = 'analytics/';

@Injectable({
    providedIn: 'root'
})
export class AnalyticsService
{
    /**
     * Constructor
     */
    constructor(private _api: ApiService) { }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Gets summed totals of all logbook entries
     */
     getTotals(): Observable<TotalsAnalytics>
     {
         return this._api.get(`${URL}totals`);
     }

    /**
     * Gets totals of all logbook entries grouped by month and year
     */
    getTotalsByMonth(): Observable<TotalsAnalytics[]>
    {
        return this._api.get(`${URL}totals/month`);
    }

    /**
     * Gets totals of all logbook entries grouped by category and class
     */
    getTotalsByCatClass(): Observable<TotalsAnalytics[]>
    {
        return this._api.get(`${URL}totals/catclass`);
    }

    /**
     * Gets totals of all logbook entries grouped by instance type (real vs sim)
     */
    getTotalsByInstanceType(): Observable<TotalsAnalytics[]>
    {
        return this._api.get(`${URL}totals/instance`);
    }

    /**
     * Gets totals of all logbook entries grouped by aircraft type
     */
    getTotalsByType(): Observable<TotalsAnalytics[]>
    {
        return this._api.get(`${URL}totals/type`);
    }

    /**
     * Gets totals of all logbook entries grouped by month and year
     */
    getLandingsApproachesPast90Days(): Observable<LandingsAnalytics>
    {
        return this._api.get(`${URL}currency/landings`);
    }
}
