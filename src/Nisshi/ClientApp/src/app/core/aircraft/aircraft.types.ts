export interface Aircraft
{
    id: number;
    tailNumber: string;
    instanceType: number;
    lastAnnual?: Date;
    lastPitotStatic?: Date;
    lastVOR?: Date;
    lastAltimeter?: Date;
    lastTransponder?: Date;
    lastELT?: Date;
    last100Hobbs?: number;
    lastOilHobbs?: number;
    lastEngineHobbs?: number;
    registrationDue?: number;
    notes?: string;
    idModel: number;
}
