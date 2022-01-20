export interface TotalsAnalytics
{
    month: number;
    year: number;
    categoryClass: string;
    type: string;
    instance: number;
    totalTimeSum: number;
    nightSum: number;
    instrumentSum: number;
    crossCountrySum: number;
    turbineSum: number;
    picSum: number;
    sicSum: number;
    dualGivenSum: number;
}

export interface LandingsAnalytics
{
    dayLandings: number;
    nightLandings: number;
    approaches: number;
}

export interface AnalyticsCompendium
{
    totalsByMonth: TotalsAnalytics[];
    totalsByCatClass: TotalsAnalytics[];
    totalsByType: TotalsAnalytics[];
    totalsByInstance: TotalsAnalytics[];
    landingsPast90Days: LandingsAnalytics
}
