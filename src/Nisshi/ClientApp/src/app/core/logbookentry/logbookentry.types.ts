export interface LogbookEntry
{
    id: number;
    flightDate: Date;
    numInstrumentApproaches: number;
    numLandings: number;
    numNightLandings: number;
    numFullStopLandings: number;
    crossCountry: number;
    multiEngine: number;
    Night: number;
    IMC: number;
    simulatedInstrument: number;
    dualReceived: number;
    dualGiven: number;
    PIC: number;
    SIC: number;
    groundSim: number;
    hobbsStart: number;
    hobbsEnd: number;
    totalFlightTime: number;
    route: string;
    comments: string;
    idAircraft: number;
}