import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ApiService } from '../base/api.service';
import { CategoryClass } from './categoryclass.types';

const URL = 'categoryclasses/';

@Injectable({
    providedIn: 'root'
})
export class CategoryClassService
{
    private _categoryclass: ReplaySubject<CategoryClass> = new ReplaySubject<CategoryClass>(1);

    /**
     * Constructor
     */
    constructor(private _api: ApiService) { }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Searches for categoryclasses by name
     *
     * @param partialName
     */
    getManyByPartialName(partialName: string): Observable<CategoryClass[]>
    {
        return this._api.get(`${URL}search/${partialName}`).pipe(
            tap((categoryclass) => {

            })
        );
    }

    /**
     * Saves a new categoryclass
     *
     * @param categoryclass
     */
    create(categoryclass: CategoryClass): Observable<any>
    {
        return this._api.post(URL, {categoryclass}).pipe(
            map((response) => {
                this._categoryclass.next(response);
            })
        );
    }
}
