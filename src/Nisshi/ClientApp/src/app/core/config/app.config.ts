/**
 * AppConfig interface. Update this interface to strictly type your config
 * object.
 */
export interface AppConfig
{
    language: string;
    logbookOptions: LogbookOptions;
    maintenanceOptions: MaintenanceOptions;
}

/**
 * LogbookOptions interface. Contains configuration for columns to display on the
 * logbook view
 */
export interface LogbookOptions
{
    showTailNumber: boolean;
    showTypeName: boolean;
    showApproaches: boolean;
    showLandings: boolean;
    showNightLandings: boolean;
    showFullStopLandings: boolean;
    showPIC: boolean;
    showSIC: boolean;
    showCrossCountry: boolean;
    showNight: boolean;
    showMultiEngine: boolean;
    showTurbine: boolean;
    showSimulatedInstrument: boolean;
    showIMC: boolean;
    showDualReceived: boolean;
    showDualGiven: boolean;
    showGroundSim: boolean;
    showTotalTime: boolean;
    showComments: boolean;
}

/**
 * MaintenanceOptions interface. Contains configuration for columns to display on the
 * maintenance view
 */
export interface MaintenanceOptions
{
    showMilesPerformed: boolean;
    showType: boolean;
    showWorkDescription: boolean;
    showPerformedBy: boolean;
    showDuration: boolean;
    showRepairPrice: boolean;
}

/**
 * Default configuration for the entire application. This object is used by
 * FuseConfigService to set the default configuration.
 *
 * If you need to store global configuration for your app, you can use this
 * object to set the defaults. To access, update and reset the config, use
 * FuseConfigService and its methods.
 */
export const appConfig: AppConfig = {
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
        showTurbine: true,
        showSimulatedInstrument: true,
        showIMC: true,
        showDualReceived: true,
        showDualGiven: true,
        showGroundSim: true,
        showTotalTime: true,
        showComments: true,
    },
    maintenanceOptions: {
        showMilesPerformed: true,
        showType: true,
        showWorkDescription: true,
        showPerformedBy: true,
        showDuration: true,
        showRepairPrice: true,
    }
};
