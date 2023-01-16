import { Component, Inject, LOCALE_ID, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ApexOptions } from 'ng-apexcharts';
import { AnalyticsCompendium, ChartData, LandingsAnalytics, TotalsAnalytics } from 'app/core/analytics/analytics.types';
import { UserService } from 'app/core/user/user.service';
import { User } from 'app/core/user/user.types';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { formatNumber } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector       : 'dashboard',
    templateUrl    : './dashboard.component.html',
    encapsulation  : ViewEncapsulation.None
})
export class DashboardComponent implements OnInit, OnDestroy
{
    chartTotalsByMonth: ApexOptions = {};
    chartTotalsByType: ApexOptions = {};
    chartTotalsByInstance: ApexOptions = {};
    chartTotalsByCatClass: ApexOptions = {};

    summedTotals: TotalsAnalytics;
    landingsPast90Days: LandingsAnalytics;

    private _unsubscribeAll: Subject<any> = new Subject<any>();
    user: User;

    /**
     * Constructor
     */
    constructor(private userService: UserService,
                private route: ActivatedRoute,
                @Inject(LOCALE_ID) private locale: string
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        // From Dashboard Resolver
        this.route.data.subscribe(x => {
            var analytics: AnalyticsCompendium = x.analytics;

            var catClassTotals = this.mapIntoPolarChartData(analytics.byCatClass, analytics.byCatClass.map(x => x.categoryClass));
            var instanceTotals = this.mapIntoPolarChartData(analytics.byInstance, analytics.byInstance.map(x => x.instance));
            var typeTotals = this.mapIntoPolarChartData(analytics.byType, analytics.byType.map(x => x.type));
            var monthTotals = this.mapIntoLineChartData(analytics.byMonth, analytics.byMonth.map(x => { return `${x.month}/${x.year}`; }));

            this.chartTotalsByCatClass = this.preparePolarChart(catClassTotals);
            this.chartTotalsByInstance = this.preparePolarChart(instanceTotals);
            this.chartTotalsByType = this.preparePolarChart(typeTotals);
            this.chartTotalsByMonth = this.prepareLineChart(monthTotals);

            this.summedTotals = analytics.summedTotals;
            this.landingsPast90Days = analytics.landingsPast90Days;
        });

        this.userService.user$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((user: User) => {
                this.user = user;
            });
    }

    /**
     * On destroy
     */
     ngOnDestroy(): void
     {
         // Unsubscribe from all subscriptions
         this._unsubscribeAll.next(1);
         this._unsubscribeAll.complete();
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
                 'instrument': [{
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

    /**
     * Configures an Apex line chart
     *
     * @param lineData
     * @returns Apex chart configuration
     */
    private prepareLineChart(lineData: any): ApexOptions
    {
        return {
            chart      : {
                fontFamily: 'inherit',
                foreColor : 'inherit',
                height    : '100%',
                type      : 'line',
                toolbar   : {
                    show: false
                },
                zoom      : {
                    enabled: false
                }
            },
            colors     : ['#64748B', '#94A3B8'],
            dataLabels : {
                enabled        : true,
                enabledOnSeries: [0],
                background     : {
                    borderWidth: 0
                }
            },
            grid       : {
                borderColor: 'var(--fuse-border)'
            },
            labels     : lineData.labels,
            legend     : {
                show: false
            },
            plotOptions: {
                bar: {
                    columnWidth: '50%'
                }
            },
            series     : lineData.series,
            states     : {
                hover: {
                    filter: {
                        type : 'darken',
                        value: 0.75
                    }
                }
            },
            stroke     : {
                width: [3, 0]
            },
            tooltip    : {
                followCursor: true,
                theme       : 'dark'
            },
            xaxis      : {
                axisBorder: {
                    show: false
                },
                axisTicks : {
                    color: 'var(--fuse-border)'
                },
                labels    : {
                    style: {
                        colors: 'var(--fuse-text-secondary)'
                    }
                },
                tooltip   : {
                    enabled: false
                }
            },
            yaxis      : {
                labels: {
                    offsetX: -16,
                    style  : {
                        colors: 'var(--fuse-text-secondary)'
                    }
                }
            }
        };
    }

    /**
     * Configures an Apex polar chart
     *
     * @param polarData
     * @returns Apex chart configuration
     */
     private preparePolarChart(polarData: any): ApexOptions
     {
        return {
            chart      : {
                fontFamily: 'inherit',
                foreColor : 'inherit',
                height    : '100%',
                type      : 'polarArea',
                toolbar   : {
                    show: false
                },
                zoom      : {
                    enabled: false
                }
            },
            labels     : polarData.labels,
            legend     : {
                position: 'bottom'
            },
            plotOptions: {
                polarArea: {
                    spokes: {
                        connectorColors: 'var(--fuse-border)'
                    },
                    rings : {
                        strokeColor: 'var(--fuse-border)'
                    }
                }
            },
            series     : polarData.series,
            states     : {
                hover: {
                    filter: {
                        type : 'darken',
                        value: 0.75
                    }
                }
            },
            stroke     : {
                width: 2
            },
            theme      : {
                monochrome: {
                    enabled       : true,
                    color         : '#93C5FD',
                    shadeIntensity: 0.75,
                    shadeTo       : 'dark'
                }
            },
            tooltip    : {
                followCursor: true,
                theme       : 'dark'
            },
            yaxis      : {
                labels: {
                    style: {
                        colors: 'var(--fuse-text-secondary)'
                    }
                }
            }
        };
    }
}
