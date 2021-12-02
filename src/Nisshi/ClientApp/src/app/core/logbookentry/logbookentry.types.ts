import { Aircraft } from "../aircraft/aircraft.types";

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
    night: number;
    imc: number;
    simulatedInstrument: number;
    dualReceived: number;
    dualGiven: number;
    pic: number;
    sic: number;
    groundSim: number;
    hobbsStart: number;
    hobbsEnd: number;
    totalFlightTime: number;
    route: string;
    comments: string;
    idAircraft: number;
    aircraft: Aircraft;
}
