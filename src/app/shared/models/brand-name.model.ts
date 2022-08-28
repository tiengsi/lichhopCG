export class BrandNameModel {
    brandNameId?: number;
    contractType: number;
    apiUser: string;
    apiPass: string;
    userName?: string;
    branchName: string;
    apiLink?: string;
    isActive: boolean;
    organizeId: number;
}

export class ViettelBrandNameModel extends BrandNameModel{
    cpCode?: string;
}

export class VNPTBrandNameModel extends BrandNameModel{
    phoneNumber?: string;
}