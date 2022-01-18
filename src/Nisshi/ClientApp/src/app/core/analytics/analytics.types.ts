export interface TotalsAnalytics
{
    Month: number;
    Year: number;
    CategoryClass: string;
    Type: string;
    Instance: number;
    TotalTimeSum: number;
    NightSum: number;
    InstrumentSum: number;
    CrossCountrySum: number;
    TurbineSum: number;
    PICSum: number;
    SICSum: number;
    DualGivenSum: number;
}

export interface LandingsAnalytics
{
    DayLandings: number;
    NightLandings: number;
    Approaches: number;
}
