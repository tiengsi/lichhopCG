export enum PERMISSIONS {
    ALL = '0',
    LIST_SCHEDULE = '1',
    ADD_SCHEDULE = '2',
    EDIT_SCHEDULE = '3',
    DELETE_SCHEDULE = '4',
    APPROVE_SCHEDULE = '5',
    NEWS = '7',
    SETTINGS = '8',
}

export enum ROLES {
    SuperAdmin = 'SuperAdmin',
    Admin = 'Admin',
    NormalAdmin = 'Normal-Admin',
    User = 'User',
    Scheduler = 'Scheduler'
}

export const ROLES_PERMISSIONS: { [key: string]: string[] } = {
    [ROLES.SuperAdmin]: [PERMISSIONS.ALL],
    [ROLES.Admin]: [PERMISSIONS.ALL],
    [ROLES.NormalAdmin]: [
        PERMISSIONS.LIST_SCHEDULE, PERMISSIONS.ADD_SCHEDULE,
        PERMISSIONS.EDIT_SCHEDULE, PERMISSIONS.NEWS, PERMISSIONS.SETTINGS,
    ],
    [ROLES.Scheduler] : [
        PERMISSIONS.LIST_SCHEDULE, PERMISSIONS.ADD_SCHEDULE,
        PERMISSIONS.EDIT_SCHEDULE]
};



