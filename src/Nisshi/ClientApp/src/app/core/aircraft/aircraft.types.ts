import { Model } from "../model/model.types";

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
    registrationDue?: Date;
    notes?: string;
    idModel: number;
    model: Model;
}
