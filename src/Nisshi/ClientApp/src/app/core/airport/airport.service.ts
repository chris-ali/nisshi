import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../base/api.service';
import { Airport } from './airport.types';

const URL = 'airports/';

@Injectable({
  providedIn: 'root'
})
export class AirportService {

    /**
     * Constructor
     */
    constructor(private _api: ApiService) { }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Gets a list of airports by the partial ICAO code provided
     *
     * @param partialCode
     */
    getManyByPartialCode(partialCode: string): Observable<Airport[]>
    {
        return this._api.get(`${URL}${partialCode}`);
    }
}
