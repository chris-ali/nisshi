import { Injectable } from '@angular/core';
import { Observable, of, ReplaySubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { cloneDeep } from 'lodash';
import { ApiService } from '../base/api.service';
import { Navigation } from 'app/core/navigation/navigation.types';
import { compactNavigation, defaultNavigation, futuristicNavigation, horizontalNavigation } from './navigation.data';

@Injectable({
    providedIn: 'root'
})
export class NavigationService
{
    private _navigation: ReplaySubject<Navigation> = new ReplaySubject<Navigation>(1);

    /**
     * Constructor
     */
    constructor(private _api: ApiService)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for navigation
     */
    get navigation$(): Observable<Navigation>
    {
        return this._navigation.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get all navigation data
     */
    get(): Observable<Navigation>
    {
        return of(this.generateNavigation()).pipe(
            tap((navigation) => {
                this._navigation.next(navigation);
            })
        );
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Generates a Navigation object from the default, filling in children
     * from defaultNavigation for each other FuseNavigationItem
     *
     * @returns Navigation object filled in
     */
    private generateNavigation(): Navigation
    {
        let _defaultNavigation = defaultNavigation;
        let _compactNavigation = compactNavigation;
        let _futuristicNavigation = futuristicNavigation;
        let _horizontalNavigation = horizontalNavigation;

        // Fill compact navigation children using the default navigation
        _compactNavigation.forEach((compactNavItem) => {
            _defaultNavigation.forEach((defaultNavItem) => {
                if ( defaultNavItem.id === compactNavItem.id )
                {
                    compactNavItem.children = cloneDeep(defaultNavItem.children);
                }
            });
        });

        // Fill futuristic navigation children using the default navigation
        _futuristicNavigation.forEach((futuristicNavItem) => {
            _defaultNavigation.forEach((defaultNavItem) => {
                if ( defaultNavItem.id === futuristicNavItem.id )
                {
                    futuristicNavItem.children = cloneDeep(defaultNavItem.children);
                }
            });
        });

        // Fill horizontal navigation children using the default navigation
        _horizontalNavigation.forEach((horizontalNavItem) => {
            _defaultNavigation.forEach((defaultNavItem) => {
                if ( defaultNavItem.id === horizontalNavItem.id )
                {
                    horizontalNavItem.children = cloneDeep(defaultNavItem.children);
                }
            });
        });

        let navi: Navigation  = {
            compact: _compactNavigation,
            default: _defaultNavigation,
            futuristic: _futuristicNavigation,
            horizontal: _horizontalNavigation
        };

        return navi;
    }
}
