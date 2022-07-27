import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ApiService } from '../base/api.service';
import { Vehicle } from './vehicle.types';

const URL = 'vehicle/';

@Injectable({
    providedIn: 'root'
})
export class VehicleService
{
    private _vehicle: ReplaySubject<Vehicle> = new ReplaySubject<Vehicle>(1);

    /**
     * Constructor
     */
    constructor(private _api: ApiService) { }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get a single vehicle for the current logged in user
     *
     * @param id
     */
    getOne(id: number): Observable<Vehicle>
    {
        return this._api.get(`${URL}${id}`).pipe();
    }

    /**
     * Get the current logged in user's vehicle
     *
     * @param filter Optional - OData filter query; question mark not needed
     */
    getAll(filter?: string): Observable<Vehicle[]>
    {
        var filterQuery = filter ? `?${filter}`: "";
        return this._api.get(`${URL}all${filterQuery}`);
    }

    /**
     * Update the vehicle
     *
     * @param vehicle
     */
    update(vehicle: Vehicle): Observable<any>
    {
        return this._api.put(URL, vehicle).pipe(
            map((response) => {
                this._vehicle.next(response);
            })
        );
    }

    /**
     * Saves a new vehicle
     *
     * @param vehicle
     */
    create(vehicle: Vehicle): Observable<any>
    {
        return this._api.post(URL, vehicle).pipe(
            map((response) => {
                this._vehicle.next(response);
            })
        );
    }

    /**
     * Deletes the vehicle
     *
     * @param vehicle
     */
     delete(id: number): Observable<Vehicle>
     {
        return this._api.delete(`${URL}${id}`);
     }
}
