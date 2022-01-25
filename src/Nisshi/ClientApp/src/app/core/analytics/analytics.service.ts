import { Inject, Injectable, LOCALE_ID } from '@angular/core';
import { DecimalPipe,formatNumber } from '@angular/common';
import { forkJoin, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ApiService } from '../base/api.service';
import { TotalsAnalytics, LandingsAnalytics, AnalyticsCompendium, ChartData } from './analytics.types';

const URL = 'analytics/';

@Injectable({
    providedIn: 'root'
})
export class AnalyticsService
{
    /**
     * Constructor
     */
    constructor(private _api: ApiService,
                @Inject(LOCALE_ID) private locale: string) { }


    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Gets a formatted version of all available analytics types for use on Apex Charts
     * on dashboard
     */
    getAllAnalytics(): Observable<AnalyticsCompendium>
    {
        return forkJoin([
            this.getTotals(),
            this.getTotalsByMonth(),
            this.getTotalsByCatClass(),
            this.getTotalsByType(),
            this.getTotalsByInstanceType(),
            this.getLandingsApproachesPast90Days()
        ]).pipe(
            map(([totals, byMonth, byCatClass, byType, byInstanceType, landings]) => {
                var analytics: AnalyticsCompendium =
                {
                    summedTotals: totals,
                    totalsByMonth: byMonth,
                    totalsByCatClass: byCatClass,
                    totalsByType: byType,
                    totalsByInstance: byInstanceType,
                    landingsPast90Days: landings
                };

                return analytics;
            }
        ));
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Gets summed totals of all logbook entries
     */
     getTotals(): Observable<TotalsAnalytics>
     {
         return this._api.get(`${URL}totals`).pipe();
     }

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

                return this.mapIntoLineChartData(analytics, labels);
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

                return this.mapIntoPolarChartData(analytics, labels);
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

                return this.mapIntoPolarChartData(analytics, labels);
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

                return this.mapIntoPolarChartData(analytics, labels);
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
     * Formats analytics data into a format apex line chart can use correctly
     *
     * @param analytics
     * @param labels
     * @returns formatted chart data
     */
    private mapIntoLineChartData(analytics: TotalsAnalytics[], labels: string[]): ChartData
    {
        var type = 'column';
        var chartData: ChartData =
        {
            labels: labels,
            series: {
                'total-time': [{
                    name: 'Total Time',
                    type: type,
                    data: analytics.map(x => this.truncDecimal(x.totalTimeSum))
                }],
                'imc': [{
                    name: 'Instrument',
                    type: type,
                    data: analytics.map(x => this.truncDecimal(x.instrumentSum))
                }],
                'multi': [{
                    name: 'Multi',
                    type: type,
                    data: analytics.map(x => this.truncDecimal(x.multiSum))
                }],
                'dual-given': [{
                    name: 'Dual Given',
                    type: type,
                    data: analytics.map(x => this.truncDecimal(x.dualGivenSum))
                }],
                'turbine': [{
                    name: 'Turbine',
                    type: type,
                    data: analytics.map(x => this.truncDecimal(x.turbineSum))
                }],
                'sic': [{
                    name: 'SIC',
                    type: type,
                    data: analytics.map(x => this.truncDecimal(x.sicSum))
                }],
                'pic': [{
                    name: 'PIC',
                    type: type,
                    data: analytics.map(x => this.truncDecimal(x.picSum))
                }],
                'night': [{
                    name: 'Night',
                    type: type,
                    data: analytics.map(x => this.truncDecimal(x.nightSum))
                }],
                'cross-country': [{
                    name: 'Cross Country',
                    type: type,
                    data: analytics.map(x => this.truncDecimal(x.crossCountrySum))
                }]
            }
        };

        return chartData;
    }

    /**
     * Formats analytics data into a format apex polar chart can use correctly
     *
     * @param analytics
     * @param chartName
     * @param type
     * @param labels
     * @returns formatted chart data
     */
    private mapIntoPolarChartData(analytics: TotalsAnalytics[], labels: string[]): ChartData
    {
        var chartData: ChartData =
        {
            labels: labels,
            series: {
                'total-time': analytics.map(x => this.truncDecimal(x.totalTimeSum)),
                'instrument': analytics.map(x => this.truncDecimal(x.instrumentSum)),
                'multi': analytics.map(x => this.truncDecimal(x.multiSum)),
                'dual-given': analytics.map(x => this.truncDecimal(x.dualGivenSum)),
                'turbine': analytics.map(x => this.truncDecimal(x.turbineSum)),
                'sic': analytics.map(x => this.truncDecimal(x.sicSum)),
                'pic': analytics.map(x => this.truncDecimal(x.picSum)),
                'night': analytics.map(x => this.truncDecimal(x.nightSum)),
                'cross-country': analytics.map(x => this.truncDecimal(x.crossCountrySum)),
            }
        };

        return chartData;
    }

    /**
     * Given a number argument, truncate to 2 decimal places
     *
     * @param aDecimal
     * @returns Truncated decimal as string
     */
    private truncDecimal(aDecimal: number): string
    {
        return formatNumber(aDecimal, this.locale, '1.2-2');
    }

    // private mapIntoChartData(analytics: TotalsAnalytics[], propertyToApply: keyof TotalsAnalytics, labels: string[])
    // {
    //     var series: Record<string, SeriesData[]> = {};
    //     var type = 'line';
    //     analytics.forEach(analytic => {
    //         series[analytic[propertyToApply]] =
    //         [
    //             ...
    //         ];
    //     });
    //     ...
    // }
}
