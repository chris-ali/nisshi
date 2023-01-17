import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { merge } from 'lodash-es';
import { APP_CONFIG } from './config.constants';

@Injectable({
    providedIn: 'root'
})
export class ConfigService
{
    private _config: BehaviorSubject<any>;

    /**
     * Constructor
     */
    constructor(@Inject(APP_CONFIG) config: any)
    {
        // Private
        this._config = new BehaviorSubject(config);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for config
     */
    set config(value: any)
    {
        // Merge the new config over to the current config
        const config = merge({}, this._config.getValue(), value);

        // Execute the observable
        this._config.next(config);

        // Persist the config in localStorage
        localStorage.setItem('appConfig', JSON.stringify(config));
    }

    get config$(): Observable<any>
    {
        // Get config from localStorage and update observable if found
        var fromStorage = localStorage.getItem('appConfig');

        if (fromStorage != null && fromStorage != "undefined")
            this._config.next(JSON.parse(fromStorage));

        return this._config.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Resets the config to the default
     */
    reset(): void
    {
        // Set the config
        this._config.next(this.config);
    }
}
