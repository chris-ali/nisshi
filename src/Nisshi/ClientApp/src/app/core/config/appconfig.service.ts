import { Injectable } from '@angular/core';
import { appConfig, AppConfig } from './app.config';

@Injectable({
    providedIn: 'root'
})
export class AppConfigService
{
    /**
     * Constructor
     */
    constructor()
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for AppConfig
     *
     * @param value
     */
    set appConfig(value: AppConfig)
    {
        localStorage.setItem('appConfig', JSON.stringify(value));
    }

    get appConfig$(): AppConfig
    {
        var fromStorage = localStorage.getItem('appConfig');

        if (fromStorage != null && fromStorage != "undefined")
        {
            return <AppConfig>JSON.parse(fromStorage);
        }
        else
        {
            var defaultPrefs = appConfig;
            this.appConfig = defaultPrefs;
            return defaultPrefs;
        }
    }
}
