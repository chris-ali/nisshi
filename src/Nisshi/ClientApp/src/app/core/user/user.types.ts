
export interface User
{
    username: string;
    email: string;
    firstName: string;
    lastName: string;
    passwordQuestion?: string;
    passwordAnswer?: string;
    lastLoginDate?: Date;
    lastBFR?: Date;
    lastMedical?: Date;
    monthsToMedical?: number;
    isInstructor?: boolean;
    cfiExpiration?: Date;
    license?: string;
    certificateNumber?: string;
}
