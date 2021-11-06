import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { Aircraft } from './aircraft.types';

const API_URL = 'api/aircraft/';

@Injectable({
  providedIn: 'root'
})
export class AircraftService {

    private _aircraft: ReplaySubject<Aircraft> = new ReplaySubject<Aircraft>();

    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient) { }

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
        return this._httpClient.get<Aircraft>(API_URL + id).pipe(
            tap((aircraft) => {
                this._aircraft.next(aircraft);
            })
        );
    }

    /**
     * Get the current logged in user's aircraft
     */
    getAll(): Observable<Aircraft[]>
    {
        return this._httpClient.get<Aircraft[]>(API_URL).pipe(
            tap((aircraft) => {
                aircraft.forEach(function(air) {
                    this._aircraft.next(air);
                })
            })
        );
    }

    /**
     * Update the aircraft
     *
     * @param aircraft
     */
    update(aircraft: Aircraft): Observable<any>
    {
        return this._httpClient.put<Aircraft>(API_URL, {aircraft}).pipe(
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
    create(aircraft: Aircraft): Observable<Aircraft>
    {
        return this._httpClient.post<Aircraft>(API_URL, {aircraft});
    }

    /**
     * Deletes the aircraft
     *
     * @param aircraft
     */
     delete(id: number): Observable<Aircraft>
     {
        return this._httpClient.delete<Aircraft>(API_URL + id);
     }
}
