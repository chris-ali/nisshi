import { ChangeDetectionStrategy, Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { ApexOptions } from 'ng-apexcharts';
import { AnalyticsService } from 'app/core/analytics/analytics.service';
import { takeUntil } from 'rxjs/operators';
import { AnalyticsCompendium } from 'app/core/analytics/analytics.types';

@Component({
    selector       : 'dashboard',
    templateUrl    : './dashboard.component.html',
    encapsulation  : ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent implements OnInit, OnDestroy
{
    chartTotalsByMonth: ApexOptions = {};
    chartTotalsByType: ApexOptions = {};
    chartTotalsByInstance: ApexOptions = {};
    chartTotalsByCatClass: ApexOptions = {};

    analytics: AnalyticsCompendium;
    private _unsubscribeAll: Subject<AnalyticsCompendium> = new Subject<AnalyticsCompendium>();

    /**
     * Constructor
     */
    constructor(private _analyticsService: AnalyticsService,
                private _router: Router
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
        this._analyticsService.analytics$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((response) => {

                // Store the data
                this.analytics = response;

                // Prepare the chart data
                this._prepareChartData();
            });

        // Attach SVG fill fixer to all ApexCharts
        window['Apex'] = {
            chart: {
                events: {
                    mounted: (chart: any, options?: any): void => {
                        this._fixSvgFill(chart.el);
                    },
                    updated: (chart: any, options?: any): void => {
                        this._fixSvgFill(chart.el);
                    }
                }
            }
        };
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any
    {
        return item.id || index;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Fix the SVG fill references. This fix must be applied to all ApexCharts
     * charts in order to fix 'black color on gradient fills on certain browsers'
     * issue caused by the '<base>' tag.
     *
     * Fix based on https://gist.github.com/Kamshak/c84cdc175209d1a30f711abd6a81d472
     *
     * @param element
     * @private
     */
    private _fixSvgFill(element: Element): void
    {
        // Current URL
        const currentURL = this._router.url;

        // 1. Find all elements with 'fill' attribute within the element
        // 2. Filter out the ones that doesn't have cross reference so we only left with the ones that use the 'url(#id)' syntax
        // 3. Insert the 'currentURL' at the front of the 'fill' attribute value
        Array.from(element.querySelectorAll('*[fill]'))
             .filter(el => el.getAttribute('fill').indexOf('url(') !== -1)
             .forEach((el) => {
                 const attrVal = el.getAttribute('fill');
                 el.setAttribute('fill', `url(${currentURL}${attrVal.slice(attrVal.indexOf('#'))}`);
             });
    }

    /**
     * Prepare the chart data from the data
     *
     * @private
     */
    private _prepareChartData(): void
    {
        // Totals by month
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

        // Totals by type
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

        // Totals by instance
        this.chartTotalsByInstance = {
            chart      : {
                fontFamily: 'inherit',
                foreColor : 'inherit',
                height    : '100%',
                type      : 'polarArea',
                sparkline : {
                    enabled: true
                }
            },
            colors     : ['#818CF8'],
            dataLabels : {
                enabled   : true,
                textAnchor: 'start',
                style     : {
                    fontSize  : '13px',
                    fontWeight: 500
                },
                background: {
                    borderWidth: 0,
                    padding    : 4
                },
                offsetY   : -15
            },
            markers    : {
                strokeColors: '#818CF8',
                strokeWidth : 4
            },
            plotOptions: {
                radar: {
                    polygons: {
                        strokeColors   : 'var(--fuse-border)',
                        connectorColors: 'var(--fuse-border)'
                    }
                }
            },
            series     : this.analytics.totalsByInstance.series,
            stroke     : {
                width: 2
            },
            tooltip    : {
                theme: 'dark',
                y    : {
                    formatter: (val: number): string => `${val}%`
                }
            },
            xaxis      : {
                labels    : {
                    show : true,
                    style: {
                        fontSize  : '12px',
                        fontWeight: '500'
                    }
                },
                categories: this.analytics.totalsByInstance.categories
            },
            yaxis      : {
                max       : (max: number): number => parseInt((max + 10).toFixed(0), 10),
                tickAmount: 7
            }
        };

        // Weekly expenses
        this.chartTotalsByCatClass = {
            chart  : {
                animations: {
                    enabled: false
                },
                fontFamily: 'inherit',
                foreColor : 'inherit',
                height    : '100%',
                type      : 'polarArea',
                sparkline : {
                    enabled: true
                }
            },
            colors : ['#22D3EE'],
            series : this.analytics.totalsByCatClass.series,
            stroke : {
                curve: 'smooth'
            },
            tooltip: {
                theme: 'dark'
            },
            xaxis  : {
                type      : 'category',
                categories: this.analytics.totalsByCatClass.labels
            },
            yaxis  : {
                labels: {
                    formatter: (val): string => `$${val}`
                }
            }
        };
    }
}
