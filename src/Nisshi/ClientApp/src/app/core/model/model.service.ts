import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ApiService } from '../base/api.service';
import { Model } from './model.types';

const URL = 'models/';

@Injectable({
  providedIn: 'root'
})
export class ModelService
{
    private _model: ReplaySubject<Model> = new ReplaySubject<Model>(1);

    /**
     * Constructor
     */
    constructor(private _api: ApiService) { }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get a single model by its database ID value
     *
     * @param id
     */
    getOne(id: number): Observable<Model>
    {
        return this._api.get(`${URL}${id}`).pipe(
            tap((model) => {

            })
        );
    }

    /**
     * Searches for models by manufacturer id
     *
     * @param id manufacturer id
     */
     getManyByManufacturer(id: number): Observable<Model[]>
     {
         return this._api.get(`${URL}manufacturer/${id}`).pipe(
             tap((model) => {

             })
         );
     }

    /**
     * Searches for models by name
     *
     * @param partialName
     */
    getManyByPartialName(partialName: string): Observable<Model[]>
    {
        return this._api.get(`${URL}search/${partialName}`).pipe(
            tap((model) => {

            })
        );
    }

    /**
     * Update the model
     *
     * @param model
     */
    update(model: Model): Observable<any>
    {
        return this._api.put(URL, {model}).pipe(
            map((response) => {
                this._model.next(response);
            })
        );
    }

    /**
     * Saves a new model
     *
     * @param model
     */
    create(model: Model): Observable<any>
    {
        return this._api.post(URL, {model}).pipe(
            map((response) => {
                this._model.next(response);
            })
        );
    }
}
