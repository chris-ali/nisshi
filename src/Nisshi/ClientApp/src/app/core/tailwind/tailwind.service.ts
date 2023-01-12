import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';

@Injectable()
export class TailwindService
{
    private _tailwindConfig: ReplaySubject<any> = new ReplaySubject<any>(1);

    /**
     * Constructor
     */
    constructor()
    {
        // Prepare the config object
        const config: any = {};

        // Execute the observable with the config
        this._tailwindConfig.next(config);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for _tailwindConfig
     */
    get tailwindConfig$(): Observable<any>
    {
        return this._tailwindConfig.asObservable();
    }
}
