import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { AnalyticsService } from 'app/core/analytics/analytics.service';

@Injectable({
    providedIn: 'root'
})
export class DashboardResolver implements Resolve<any>
{
    /**
     * Constructor
     */
    constructor(private _analyticsService: AnalyticsService)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Resolver
     *
     * @param route
     * @param state
     */
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any>
    {
        return this._analyticsService.get();
    }
}
