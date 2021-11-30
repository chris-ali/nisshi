import { Preferences } from "./preferences.types";

export interface User
{
    username: string;
    email: string;
    firstName: string;
    lastName: string;
    passwordQuestion?: string;
    lastLoginDate?: Date;
    lastBFR?: Date;
    lastMedical?: Date;
    isInstructor?: boolean;
    cfiExpiration?: Date;
    license?: string;
    certificateNumber?: string;
    preferences: Preferences;
}
