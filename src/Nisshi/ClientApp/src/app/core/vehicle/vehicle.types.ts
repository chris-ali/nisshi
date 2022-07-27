export interface Vehicle
{
    id: number;
    vin: string;
    make: string;
    model: string;
    trim: string;
    year: number;
    lastRegistration?: Date;
    registrationDue?: Date;
    lastInspection?: Date;
    inspectionDue?: Date;
    miles: number;
    notes?: string;
}
