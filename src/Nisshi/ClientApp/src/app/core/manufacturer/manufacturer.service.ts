import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ApiService } from '../base/api.service';
import { Manufacturer } from './manufacturer.types';

const URL = 'manufacturers/';

@Injectable({
  providedIn: 'root'
})
export class ManufacturerService
{
    private _manufacturer: ReplaySubject<Manufacturer> = new ReplaySubject<Manufacturer>(1);

    /**
     * Constructor
     */
    constructor(private _api: ApiService) { }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Searches for manufacturers by name
     *
     * @param partialName
     */
    getManyByPartialName(partialName: string): Observable<Manufacturer[]>
    {
        return this._api.get(`${URL}search/${partialName}`).pipe(
            tap((manufacturer) => {

            })
        );
    }

    /**
     * Saves a new manufacturer
     *
     * @param manufacturer
     */
    create(manufacturer: Manufacturer): Observable<any>
    {
        return this._api.post(URL, {manufacturer}).pipe(
            map((response) => {
                this._manufacturer.next(response);
            })
        );
    }
}
