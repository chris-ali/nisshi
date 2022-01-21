import { Injectable } from '@angular/core';
import { forkJoin, Observable, Subject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ApiService } from '../base/api.service';
import { TotalsAnalytics, LandingsAnalytics, AnalyticsCompendium, ChartData, SeriesData } from './analytics.types';

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
    getTotalsByMonth(): Observable<ChartData>
    {
        return this._api.get(`${URL}totals/month`).pipe(
            map((analytics: TotalsAnalytics[]) => {
                var labels: string[] = [];
                analytics.forEach(analytic => {
                    labels.push(`${analytic.month}/${analytic.year}`);
                });

                return this.mapIntoChartData(analytics, 'totals-by-month', 'column', labels);
            }
        ));
    }

    /**
     * Gets totals of all logbook entries grouped by category and class
     */
    getTotalsByCatClass(): Observable<ChartData>
    {
        return this._api.get(`${URL}totals/catclass`).pipe(
            map((analytics: TotalsAnalytics[]) => {
                var labels = analytics.map(x => x.categoryClass);

                return this.mapIntoChartData(analytics, 'totals-by-catclass', 'column', labels);
            }
        ));
    }

    /**
     * Gets totals of all logbook entries grouped by instance type (real vs sim)
     */
    getTotalsByInstanceType(): Observable<ChartData>
    {
        return this._api.get(`${URL}totals/instance`).pipe(
            map((analytics: TotalsAnalytics[]) => {
                var labels = analytics.map(x => x.instance);

                return this.mapIntoChartData(analytics, 'totals-by-instance', 'column', labels);
            }
        ));
    }

    /**
     * Gets totals of all logbook entries grouped by aircraft type
     */
    getTotalsByType(): Observable<ChartData>
    {
        return this._api.get(`${URL}totals/type`).pipe(
            map((analytics: TotalsAnalytics[]) => {
                var labels = analytics.map(x => x.type);

                return this.mapIntoChartData(analytics, 'totals-by-type', 'column', labels);
            }
        ));
    }

    /**
     * Gets totals of all logbook entries grouped by month and year
     */
    getLandingsApproachesPast90Days(): Observable<LandingsAnalytics>
    {
        return this._api.get(`${URL}currency/landings`).pipe();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Formats analytics data into format charting library can use correctly
     *
     * @param analytics
     * @param chartName
     * @param type
     * @param labels
     * @returns formatted chart data
     */
    private mapIntoChartData(analytics: TotalsAnalytics[],
        chartName: string,
        type: string,
        labels: string[]): ChartData
    {
        var series: Record<string, SeriesData[]> = {};
        series[chartName] =
            [
                {
                    name: 'total-time',
                    type: type,
                    data: analytics.map(x => x.totalTimeSum)
                },
                {
                    name: 'imc',
                    type: type,
                    data: analytics.map(x => x.instrumentSum)
                },
                {
                    name: 'multi',
                    type: type,
                    data: analytics.map(x => x.multiSum)
                },
                {
                    name: 'dual-given',
                    type: type,
                    data: analytics.map(x => x.dualGivenSum)
                },
                {
                    name: 'turbine',
                    type: type,
                    data: analytics.map(x => x.turbineSum)
                },
                {
                    name: 'sic',
                    type: type,
                    data: analytics.map(x => x.sicSum)
                },
                {
                    name: 'pic',
                    type: type,
                    data: analytics.map(x => x.picSum)
                },
                {
                    name: 'night',
                    type: type,
                    data: analytics.map(x => x.nightSum)
                },
                {
                    name: 'cross-country',
                    type: type,
                    data: analytics.map(x => x.crossCountrySum)
                }
            ];

        var chartData: ChartData =
        {
            labels: labels,
            series: series
        };

        return chartData;
    }
}
