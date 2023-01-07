import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ApiService } from '../base/api.service';
import { MaintenanceEntry } from './maintenanceentry.types';

const URL = 'maintenanceentries/';

@Injectable({
    providedIn: 'root'
})
export class MaintenanceEntryService
{
    private _maintenanceentry: ReplaySubject<MaintenanceEntry> = new ReplaySubject<MaintenanceEntry>(1);

    /**
     * Constructor
     */
    constructor(private _api: ApiService) { }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get a single maintenance entry for the current logged in user
     *
     * @param id
     */
    getOne(id: number): Observable<MaintenanceEntry>
    {
        return this._api.get(`${URL}${id}`).pipe(
            tap((maintenanceentry) => {

            })
        );
    }

    /**
     * Get the current logged in user's maintenance entries
     *
     * @param filter Optional - OData filter query; question mark not needed
     */
    getAll(filter?: string): Observable<MaintenanceEntry[]>
    {
        var filterQuery = filter ? `?${filter}`: "";
        return this._api.get(`${URL}all${filterQuery}`).pipe(
            tap((maintenanceentry) => {

            })
        );
    }

    /**
     * Update the maintenance entry
     *
     * @param maintenanceentry
     */
    update(maintenanceentry: MaintenanceEntry): Observable<any>
    {
        return this._api.put(URL, maintenanceentry).pipe(
            map((response) => {
                this._maintenanceentry.next(response);
            })
        );
    }

    /**
     * Saves a new maintenance entry
     *
     * @param maintenanceentry
     */
    create(maintenanceentry: MaintenanceEntry): Observable<any>
    {
        return this._api.post(URL, maintenanceentry).pipe(
            map((response) => {
                this._maintenanceentry.next(response);
            })
        );
    }

    /**
     * Deletes the maintenance entry
     *
     * @param maintenanceentry
     */
     delete(id: number): Observable<MaintenanceEntry>
     {
        return this._api.delete(`${URL}${id}`);
     }
}
