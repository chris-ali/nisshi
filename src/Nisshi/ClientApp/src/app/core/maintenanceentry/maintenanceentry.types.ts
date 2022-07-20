import { Vehicle } from "../vehicles/vehicle.types";

export interface MaintenanceEntry
{
    id: number;
    datePerformed?: Date;
    milesPerformed: number;
    type: number;
    workDescription: string;
    performedBy: string;
    duration?: number;
    repairPrice?: number;
    idVehicle: number;
    vehicle: Vehicle;
}
