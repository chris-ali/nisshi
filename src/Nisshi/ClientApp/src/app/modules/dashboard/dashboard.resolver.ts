import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { AnalyticsService } from 'app/core/analytics/analytics.service';
import { forkJoin, Observable } from 'rxjs';

@Injectable()
export class DashboardResolver implements Resolve<Observable<any>>
{
    constructor(private analyticsService: AnalyticsService)
    {}

    resolve(): Observable<any>
    {
        return forkJoin([
            this.analyticsService.getTotals(),
            this.analyticsService.getTotalsByMonth(),
            this.analyticsService.getTotalsByCatClass(),
            this.analyticsService.getTotalsByType(),
            this.analyticsService.getTotalsByInstanceType(),
            this.analyticsService.getLandingsApproachesPast90Days()
        ]);
    }
}
