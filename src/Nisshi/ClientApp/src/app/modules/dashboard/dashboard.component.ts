import { Component, Inject, LOCALE_ID, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ApexOptions } from 'ng-apexcharts';
import { AnalyticsService } from 'app/core/analytics/analytics.service';
import { AnalyticsCompendium, ChartData, TotalsAnalytics } from 'app/core/analytics/analytics.types';
import { UserService } from 'app/core/user/user.service';
import { User } from 'app/core/user/user.types';
import { forkJoin, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { formatNumber } from '@angular/common';

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

    analytics: AnalyticsCompendium;

    private unsubscribeAll: Subject<any> = new Subject<any>();
    user: User;

    /**
     * Constructor
     */
    constructor(private analyticsService: AnalyticsService,
                private userService: UserService,
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
        // Get the data
        forkJoin([
            this.analyticsService.getTotals(),
            this.analyticsService.getTotalsByMonth(),
            this.analyticsService.getTotalsByCatClass(),
            this.analyticsService.getTotalsByType(),
            this.analyticsService.getTotalsByInstanceType(),
            this.analyticsService.getLandingsApproachesPast90Days()
        ]).subscribe(([totals, byMonth, byCatClass, byType, byInstanceType, landings]) => {
            this.analytics = {
                summedTotals: totals,
                totalsByMonth: this.mapIntoLineChartData(byMonth, byMonth.map(x => { return `${x.month}/${x.year}`; })),
                totalsByCatClass: this.mapIntoPolarChartData(byCatClass, byCatClass.map(x => x.categoryClass)),
                totalsByType: this.mapIntoPolarChartData(byType, byType.map(x => x.type)),
                totalsByInstance: this.mapIntoPolarChartData(byInstanceType, byInstanceType.map(x => x.instance)),
                landingsPast90Days: landings
            };

            this.prepareChartData();
        });

        this.userService.user$
            .pipe(takeUntil(this.unsubscribeAll))
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
         this.unsubscribeAll.next();
         this.unsubscribeAll.complete();
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
      * @private
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
      * @private
      */
     private truncDecimal(aDecimal: number): string
     {
         return formatNumber(aDecimal, this.locale, '1.2-2');
     }

    /**
     * Prepare the chart data from the data
     *
     * @private
     */
    private prepareChartData(): void
    {
        this.chartTotalsByMonth = {
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
            labels     : this.analytics.totalsByMonth.labels,
            legend     : {
                show: false
            },
            plotOptions: {
                bar: {
                    columnWidth: '50%'
                }
            },
            series     : this.analytics.totalsByMonth.series,
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

        this.chartTotalsByType = {
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
            labels     : this.analytics.totalsByType.labels,
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
            series     : this.analytics.totalsByType.series,
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

        this.chartTotalsByInstance = {
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
            labels     : this.analytics.totalsByInstance.labels,
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
            series     : this.analytics.totalsByInstance.series,
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

        this.chartTotalsByCatClass = {
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
            labels     : this.analytics.totalsByCatClass.labels,
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
            series     : this.analytics.totalsByCatClass.series,
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
