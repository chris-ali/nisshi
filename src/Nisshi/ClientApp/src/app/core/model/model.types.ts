export interface Model
{
    id: number;
    typeName: string;
    family: string;
    modelName: string;
    isComplex: boolean;
    isMultiEngine: boolean;
    isHighPerformance: boolean;
    isTailwheel: boolean;
    hasConstantPropeller: boolean;
    isTurbine: boolean;
    isCertifiedSinglePilot: boolean;
    hasFlaps: boolean;
    isSimOnly: boolean;
    IsMotorGlider: boolean;
    isHelicopter: boolean;
    idCategoryClass: number;
    idManufacturer: number;
}