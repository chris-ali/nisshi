export interface Preferences
{
    theme: string;
    layout: string;
    language: string;
    logbookOptions: LogbookOptions;
}

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
    showSimulatedInstrument: boolean;
    showIMC: boolean;
    showDualReceived: boolean;
    showDualGiven: boolean;
    showGroundSim: boolean;
    showTotalTime: boolean;
    showComments: boolean;
}
