export interface TotalsAnalytics
{
    month: number;
    year: number;
    categoryClass: string;
    type: string;
    instance: string;
    totalTimeSum: number;
    nightSum: number;
    instrumentSum: number;
    crossCountrySum: number;
    turbineSum: number;
    picSum: number;
    sicSum: number;
    dualGivenSum: number;
    multiSum: number;
}

export interface LandingsAnalytics
{
    dayLandings: number;
    nightLandings: number;
    approaches: number;
}

export interface SeriesData
{
    name: string;
    type?: string;
    data: number[];
}

export interface ChartData
{
    labels: string[];
    series: Record<string, any>; // Can be number[] or SeriesData[]
}

export interface AnalyticsCompendium
{
    summedTotals: TotalsAnalytics
    byMonth: TotalsAnalytics[]
    byCatClass: TotalsAnalytics[]
    byType: TotalsAnalytics[]
    byInstance: TotalsAnalytics[]
    landingsPast90Days: LandingsAnalytics
}
