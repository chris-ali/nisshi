import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ApiService } from '../base/api.service';
import { Aircraft } from './aircraft.types';

const URL = 'aircraft/';

@Injectable({
    providedIn: 'root'
})
export class AircraftService
{
    private _aircraft: ReplaySubject<Aircraft> = new ReplaySubject<Aircraft>(1);

    /**
     * Constructor
     */
    constructor(private _api: ApiService) { }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get a single aircraft for the current logged in user
     *
     * @param id
     */
    getOne(id: number): Observable<Aircraft>
    {
        return this._api.get(`${URL}${id}`).pipe();
    }

    /**
     * Get the current logged in user's aircraft
     *
     * @param filter Optional - OData filter query; question mark not needed
     */
    getAll(filter?: string): Observable<Aircraft[]>
    {
        var filterQuery = filter ? `?${filter}`: "";
        return this._api.get(`${URL}all${filterQuery}`);
    }

    /**
     * Update the aircraft
     *
     * @param aircraft
     */
    update(aircraft: Aircraft): Observable<any>
    {
        return this._api.put(URL, aircraft).pipe(
            map((response) => {
                this._aircraft.next(response);
            })
        );
    }

    /**
     * Saves a new aircraft
     *
     * @param aircraft
     */
    create(aircraft: Aircraft): Observable<any>
    {
        return this._api.post(URL, aircraft).pipe(
            map((response) => {
                this._aircraft.next(response);
            })
        );
    }

    /**
     * Deletes the aircraft
     *
     * @param aircraft
     */
     delete(id: number): Observable<Aircraft>
     {
        return this._api.delete(`${URL}${id}`);
     }
}
