import { Injectable } from '@angular/core';
import { Preferences } from './preferences.types';

@Injectable({
    providedIn: 'root'
})
export class PreferencesService
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
     * Setter & getter for preferences
     *
     * @param value
     */
    set preferences(value: Preferences)
    {
        localStorage.setItem('preferences', JSON.stringify(value));
    }

    get preferences$(): Preferences
    {
        var fromStorage = localStorage.getItem('preferences');

        if (fromStorage != null)
        {
            return <Preferences>JSON.parse(fromStorage);
        }
        else
        {
            var defaultPrefs = this.createDefaultPreferences();
            this.preferences = defaultPrefs;
            return defaultPrefs;
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    private createDefaultPreferences(): Preferences
    {
        return {
            layout: 'modern',
            scheme: 'default',
            theme: 'light',
            language: 'en',
            logbookOptions: {
                showTailNumber: true,
                showTypeName: true,
                showApproaches: true,
                showLandings: true,
                showNightLandings: true,
                showFullStopLandings: true,
                showCrossCountry: true,
                showNight: true,
                showPIC: true,
                showSIC: true,
                showMultiEngine: true,
                showSimulatedInstrument: true,
                showIMC: true,
                showDualReceived: true,
                showDualGiven: true,
                showGroundSim: true,
                showTotalTime: true,
                showComments: true,
            }
        };
    }
}
