import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { AnalyticsService } from 'app/core/analytics/analytics.service';
import { AnalyticsCompendium } from 'app/core/analytics/analytics.types';
import { forkJoin, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class DashboardResolver implements Resolve<Observable<AnalyticsCompendium>>
{
    constructor(private analyticsService: AnalyticsService)
    {}

    resolve(): Observable<AnalyticsCompendium>
    {
        return forkJoin([
            this.analyticsService.getTotals(),
            this.analyticsService.getTotalsByMonth(),
            this.analyticsService.getTotalsByCatClass(),
            this.analyticsService.getTotalsByType(),
            this.analyticsService.getTotalsByInstanceType(),
            this.analyticsService.getLandingsApproachesPast90Days()
        ]).pipe(
            map(([totals, byMonth, byCatClass, byType, byInstanceType, landings]) => {
                var analytics: AnalyticsCompendium =
                {
                    summedTotals: totals,
                    byMonth: byMonth,
                    byCatClass: byCatClass,
                    byType: byType,
                    byInstance: byInstanceType,
                    landingsPast90Days: landings
                };

                return analytics;
            }
        ));
    }
}
