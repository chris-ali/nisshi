import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ApiService } from '../base/api.service';
import { LogbookEntry } from './logbookentry.types';

const URL = 'logbookentries/';

@Injectable({
    providedIn: 'root'
})
export class LogbookEntryService
{
    private _logbookentry: ReplaySubject<LogbookEntry> = new ReplaySubject<LogbookEntry>(1);

    /**
     * Constructor
     */
    constructor(private _api: ApiService) { }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get a single logbook entry for the current logged in user
     *
     * @param id
     */
    getOne(id: number): Observable<LogbookEntry>
    {
        return this._api.get(`${URL}${id}`).pipe(
            tap((logbookentry) => {

            })
        );
    }

    /**
     * Get the current logged in user's logbook entries
     *
     * @param filter Optional - OData filter query; question mark not needed
     */
    getAll(filter?: string): Observable<LogbookEntry[]>
    {
        var filterQuery = filter ? `?${filter}`: "";
        return this._api.get(`${URL}all${filterQuery}`).pipe(
            tap((logbookentry) => {

            })
        );
    }

    /**
     * Update the logbook entry
     *
     * @param logbookentry
     */
    update(logbookentry: LogbookEntry): Observable<any>
    {
        return this._api.put(URL, logbookentry).pipe(
            map((response) => {
                this._logbookentry.next(response);
            })
        );
    }

    /**
     * Saves a new logbook entry
     *
     * @param logbookentry
     */
    create(logbookentry: LogbookEntry): Observable<any>
    {
        return this._api.post(URL, logbookentry).pipe(
            map((response) => {
                this._logbookentry.next(response);
            })
        );
    }

    /**
     * Deletes the logbook entry
     *
     * @param logbookentry
     */
     delete(id: number): Observable<LogbookEntry>
     {
        return this._api.delete(`${URL}${id}`);
     }
}
