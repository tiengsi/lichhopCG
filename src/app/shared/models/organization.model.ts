export class Organization {
    organizeId?: number; 
    name: string 
    organizeParentId?: number; 
    codeName :string
    otherName :string
    address: string 
    phone : string
    order : number;
    isActive: boolean; 
}

export class OrganizationTree{
    organizeId: number; 
    name : string;
    subOrganizeList: OrganizationTree[]; 
    organizeParentId?: number;
    codeName: string; 
    otherName: string; 
    address: string; 
    phone : string;
    order  : number
    isActive: boolean; 
}