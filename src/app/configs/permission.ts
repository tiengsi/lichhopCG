export class PermissionList{    
    WEBSITE_SCHEDULE = 'WEBSITE_SCHEDULE';
    WEBSITE_SCHEDULE_DOCUMENTS_QRCODE= 'WEBSITE_SCHEDULE_DOCUMENTS_QRCODE';
    WEBSITE_SCHEDULE_TAKENOTE= 'WEBSITE_SCHEDULE_TAKENOTE';
    WEBSITE_SCHEDULE_RESULT_DOCUMENT= 'WEBSITE_SCHEDULE_RESULT_DOCUMENT';
    WEBSITE_SCHEDULE_RESULT_REPORT= 'WEBSITE_SCHEDULE_RESULT_REPORT';
    WEBSITE_PERSONAL_SCHEDULE= 'WEBSITE_PERSONAL_SCHEDULE';
    WEBSITE_PERSONAL_SCHEDULE_CREATE='WEBSITE_PERSONAL_SCHEDULE_CREATE';
    WEBSITE_DEPARTMENT_SCHEDULE='WEBSITE_DEPARTMENT_SCHEDULE';
    ADMIN_ORGANIZATION='ADMIN_ORGANIZATION';
    ADMIN_ORGANIZATION_CREATE='ADMIN_ORGANIZATION_CREATE';
    ADMIN_ORGANIZATION_EDIT='ADMIN_ORGANIZATION_EDIT';
    ADMIN_ORGANIZATION_DELETE='ADMIN_ORGANIZATION_DELETE'
    ADMIN_DEPARTMENT='ADMIN_DEPARTMENT'
    ADMIN_DEPARTMENT_CREATE='ADMIN_DEPARTMENT_CREATE'
    ADMIN_DEPARTMENT_EDIT='ADMIN_DEPARTMENT_EDIT'
    ADMIN_DEPARTMENT_DELETE='ADMIN_DEPARTMENT_DELETE'
    ADMIN_USER='ADMIN_USER'
    ADMIN_USER_ADMIN_CREATE='ADMIN_USER_ADMIN_CREATE'
    ADMIN_USER_OFFICER_CREATE='ADMIN_USER_OFFICER_CREATE'
    ADMIN_USER_EDIT='ADMIN_USER_EDIT'
    ADMIN_USER_DELETE='ADMIN_USER_DELETE'
    ADMIN_GROUP='ADMIN_GROUP'
    ADMIN_GROUP_CREATE='ADMIN_GROUP_CREATE'
    ADMIN_GROUP_EDIT='ADMIN_GROUP_EDIT'
    ADMIN_GROUP_DELETE='ADMIN_GROUP_DELETE'
    ADMIN_SCHEDULE='ADMIN_SCHEDULE'
    ADMIN_SCHEDULE_CREATE='ADMIN_SCHEDULE_CREATE'
    ADMIN_SCHEDULE_EDIT='ADMIN_SCHEDULE_EDIT'
    ADMIN_SCHEDULE_DELAY='ADMIN_SCHEDULE_DELAY'
    ADMIN_SCHEDULE_SENDMAIL='ADMIN_SCHEDULE_SENDMAIL'
    ADMIN_SCHEDULE_COPY='ADMIN_SCHEDULE_COPY'
    ADMIN_SCHEDULE_APPROVE='ADMIN_SCHEDULE_APPROVE'
    ADMIN_SCHEDULE_CHANGEDATE='ADMIN_SCHEDULE_CHANGEDATE'
    ADMIN_SCHEDULE_QR='ADMIN_SCHEDULE_QR'
    ADMIN_SCHEDULE_DELETE='ADMIN_SCHEDULE_DELETE'
    ADMIN_SCHEDULE_PUBLISH='ADMIN_SCHEDULE_PUBLISH'
    ADMIN_SCHEDULE_TEMPLATE='ADMIN_SCHEDULE_TEMPLATE'
    ADMIN_SCHEDULE_TEMPLATE_ADD='ADMIN_SCHEDULE_TEMPLATE_ADD'
    ADMIN_SCHEDULE_TEMPLATE_EDIT='ADMIN_SCHEDULE_TEMPLATE_EDIT'
    ADMIN_SCHEDULE_TEMPLATE_DELETE='ADMIN_SCHEDULE_TEMPLATE_DELETE'
    ADMIN_SETTINGS='ADMIN_SETTINGS'
    ADMIN_SETTINGS_GENERAL='ADMIN_SETTINGS_GENERAL'
    ADMIN_SETTINGS_BANNER_FAVICON='ADMIN_SETTINGS_BANNER_FAVICON'
    ADMIN_TITLE_TEMPLATE='ADMIN_TITLE_TEMPLATE'
    ADMIN_TITLE_TEMPLATE_ADD='ADMIN_TITLE_TEMPLATE_ADD'
    ADMIN_TITLE_TEMPLATE_EDIT='ADMIN_TITLE_TEMPLATE_EDIT'
    ADMIN_TITLE_TEMPLATE_DELETE='ADMIN_TITLE_TEMPLATE_DELETE'
    ADMIN_LOCATION='ADMIN_LOCATION'
    ADMIN_LOCATION_ADD='ADMIN_LOCATION_ADD'
    ADMIN_LOCATION_EDIT='ADMIN_LOCATION_EDIT'
    ADMIN_LOCATION_DELETE='ADMIN_LOCATION_DELETE'
    ADMIN_SEND_SMS='ADMIN_SEND_SMS'
    ADMIN_EMAIL_LOGS='ADMIN_EMAIL_LOGS'
    ADMIN_STATISTIC='ADMIN_STATISTIC'
    ADMIN_EMAIL_TEMPLATE= 'ADMIN_EMAIL_TEMPLATE'
    ADMIN_BRANDNAME= 'ADMIN_BRANDNAME'

    // permissions = {
    //     PERMISSION_ORGANIZATION: {
    //         key: 'PERMISSION_ORGANIZATION',
    //         display: ''
    //     },        
    //     PERMISSION_ORGANIZATION_ADD: {
    //         key: 'PERMISSION_ORGANIZATION_ADD',
    //         display: ''
    //     },
    //     PERMISSION_ORGANIZATION_EDIT:{
    //         key: 'PERMISSION_ORGANIZATION_EDIT',
    //         display: ''
    //     },
    //     PERMISSION_ORGANIZATION_DELETE:{
    //         key: 'PERMISSION_ORGANIZATION_DELETE',
    //         display: ''
    //     },
    //     PERMISSION_ROLE:{
    //         key: 'PERMISSION_ROLE',
    //         display: ''
    //     },
    //     PERMISSION_ROLE_ADD:{
    //         key: 'PERMISSION_ROLE_ADD',
    //         display: ''
    //     },
    //     PERMISSION_ROLE_EDIT:{
    //         key: 'PERMISSION_ROLE_EDIT',
    //         display: ''
    //     },
    //     PERMISSION_ROLE_DELETE:{
    //         key: 'PERMISSION_ROLE_DELETE',
    //         display: ''
    //     },
    //     PERMISSION_ROLE_PERMISSION_ASSIGN:{
    //         key: 'PERMISSION_ROLE_PERMISSION_ASSIGN',
    //         display: ''
    //     },
    //     PERMISSION_USER:{
    //         key: 'PERMISSION_USER',
    //         display: ''
    //     },
    //     PERMISSION_USER_ADD_OFFICER:{
    //         key: 'PERMISSION_USER_ADD_OFFICER',
    //         display: ''
    //     },
    //     PERMISSION_USER_EDIT_OFFICER:{
    //         key: 'PERMISSION_USER_EDIT_OFFICER',
    //         display: ''
    //     },
    //     PERMISSION_USER_DELETE_OFFICER:{
    //         key: 'PERMISSION_USER_DELETE_OFFICER',
    //         display: ''
    //     },
    //     PERMISSION_USER_ADD_ADMIN:{
    //         key: 'PERMISSION_USER_ADD_ADMIN',
    //         display: ''
    //     },
    //     PERMISSION_USER_EDIT_ADMIN:{
    //         key: 'PERMISSION_USER_EDIT_ADMIN',
    //         display: ''
    //     },
    //     PERMISSION_USER_DELETE_ADMIN:{
    //         key: 'PERMISSION_USER_DELETE_ADMIN',
    //         display: ''
    //     },
    //     PERMISSION_DEPARTMENT:{
    //         key: 'PERMISSION_DEPARTMENT',
    //         display: ''
    //     },
    //     PERMISSION_DEPARTMENT_ADD:{
    //         key: 'PERMISSION_DEPARTMENT_ADD',
    //         display: ''
    //     },
    //     PERMISSION_DEPARTMENT_EDIT:{
    //         key: 'PERMISSION_DEPARTMENT_EDIT',
    //         display: ''
    //     },
    //     PERMISSION_DEPARTMENT_DELETE:{
    //         key: 'PERMISSION_DEPARTMENT_DELETE',
    //         display: ''
    //     },
    //     PERMISSION_GROUP_MEETING:{
    //         key: 'PERMISSION_GROUP_MEETING',
    //         display: ''
    //     },
    //     PERMISSION_GROUP_MEETING_ADD:{
    //         key: 'PERMISSION_GROUP_MEETING_ADD',
    //         display: ''
    //     },
    //     PERMISSION_GROUP_MEETING_EDIT:{
    //         key: 'PERMISSION_GROUP_MEETING_EDIT',
    //         display: ''
    //     },
    //     PERMISSION_GROUP_MEETING_DELETE:{
    //         key: 'PERMISSION_GROUP_MEETING_DELETE',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE:{
    //         key: 'PERMISSION_SCHEDULE',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_ADD:{
    //         key: 'PERMISSION_SCHEDULE_ADD',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_SEND_INVITATION:{
    //         key: 'PERMISSION_SCHEDULE_SEND_INVITATION',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_COPY:{
    //         key: 'PERMISSION_SCHEDULE_COPY',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_EDIT:{
    //         key: 'PERMISSION_SCHEDULE_EDIT',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_APPROVE:{
    //         key: 'PERMISSION_SCHEDULE_APPROVE',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_PUBLISH:{
    //         key: 'PERMISSION_SCHEDULE_PUBLISH',
    //         display: '',
    //     },
    //     PERMISSION_SCHEDULE_DELAY:{
    //         key: 'PERMISSION_SCHEDULE_DELAY',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_RESCHEDULE:{
    //         key: 'PERMISSION_SCHEDULE_RESCHEDULE',
    //         display:''
    //     },
    //     PERMISSION_SCHEDULE_QRCODE:{
    //         key: 'PERMISSION_SCHEDULE_QRCODE',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_DELETE:{
    //         key: 'PERMISSION_SCHEDULE_DELETE',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_RESULT_REPORTS:{
    //         key: 'PERMISSION_SCHEDULE_RESULT_REPORTS',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_RESULT_DOCUMENTS:{
    //         key: 'PERMISSION_SCHEDULE_RESULT_DOCUMENTS',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_ROLL_CALL:{
    //         key: 'PERMISSION_SCHEDULE_ROLL_CALL', 
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_TEMPLATE:{
    //         key: 'PERMISSION_SCHEDULE_TEMPLATE',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_TEMPLATE_ADD:{
    //         key: 'PERMISSION_SCHEDULE_TEMPLATE_ADD',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_TEMPLATE_EDIT:{
    //         key: 'PERMISSION_SCHEDULE_TEMPLATE_EDIT',
    //         display: ''
    //     },
    //     PERMISSION_SCHEDULE_TEMPLATE_DELETE:{
    //         key: 'PERMISSION_SCHEDULE_TEMPLATE_DELETE',
    //         display: ''
    //     },
    //     PERMISSION_INVITATION_TEMPLATE:{
    //         key: 'PERMISSION_INVITATION_TEMPLATE',
    //         display: ''
    //     },
    //     PERMISSION_INVITATION_TEMPLATE_ADD:{
    //         key: 'PERMISSION_INVITATION_TEMPLATE_ADD',
    //         display: ''
    //     },
    //     PERMISSION_INVITATION_TEMPLATE_EDIT:{
    //         key: 'PERMISSION_INVITATION_TEMPLATE_EDIT',
    //         display: ''
    //     },
    //     PERMISSION_INVITATION_TEMPLATE_DELETE:{
    //         key: 'PERMISSION_INVITATION_TEMPLATE_DELETE',
    //         display: ''
    //     },
    //     PERMISSION_BRAND_NAME:{
    //         key: 'PERMISSION_BRAND_NAME',
    //         display: ''
    //     },
    //     PERMISSION_BRAND_NAME_ADD:{
    //         key: 'PERMISSION_BRAND_NAME_ADD',
    //         display: ''
    //     },
    //     PERMISSION_BRAND_NAME_EDIT:{
    //         key: 'PERMISSION_BRAND_NAME_EDIT',
    //         display: ''
    //     },
    //     PERMISSION_BRAND_NAME_DELETE:{
    //         key: 'PERMISSION_BRAND_NAME_DELETE',
    //         display: ''
    //     },
    //     PERMISSION_SETTINGS_GENERAL:{
    //         key: 'PERMISSION_SETTINGS_GENERAL',
    //         display: ''
    //     },
    //     PERMISSION_SETTINGS_BANNER_FAVICON:{
    //         key: 'PERMISSION_SETTINGS_BANNER_FAVICON',
    //         display: ''
    //     },
    //     PERMISSION_TITLE_TEMPLATE:{
    //         key: 'PERMISSION_TITLE_TEMPLATE',
    //         display: ''
    //     },
    //     PERMISSION_TITLE_TEMPLATE_ADD:{
    //         key: 'PERMISSION_TITLE_TEMPLATE_ADD',
    //         display: ''
    //     },
    //     PERMISSION_TITLE_TEMPLATE_EDIT:{
    //         key: 'PERMISSION_TITLE_TEMPLATE_EDIT',
    //         display: ''
    //     },
    //     PERMISSION_TITLE_TEMPLATE_DELETE:{
    //         key: 'PERMISSION_TITLE_TEMPLATE_DELETE',
    //         display: ''
    //     },
    //     PERMISSION_LOCATION:{
    //         key: 'PERMISSION_LOCATION',
    //         display: ''
    //     },
    //     PERMISSION_LOCATION_ADD:{
    //         key: 'PERMISSION_LOCATION_ADD',
    //         display: ''
    //     },
    //     PERMISSION_LOCATION_EDIT:{
    //         key: 'PERMISSION_LOCATION_EDIT',
    //         display: ''
    //     },
    //     PERMISSION_LOCATION_DELETE:{
    //         key: 'PERMISSION_LOCATION_DELETE',
    //         display: ''
    //     },
    //     PERMISSION_POST:{
    //         key: 'PERMISSION_POST',
    //         display: ''
    //     },
    //     PERMISSION_POST_ADD:{
    //         key: 'PERMISSION_POST_ADD',
    //         display: ''
    //     },
    //     PERMISSION_POST_EDIT:{
    //         key: 'PERMISSION_POST_EDIT',
    //         display: ''
    //     },
    //     PERMISSION_POST_DELETE:{
    //         key: 'PERMISSION_POST_DELETE',
    //         display: ''
    //     },
    //     PERMISSION_SEND_SMS:{
    //         key: 'PERMISSION_SEND_SMS',
    //         display: ''
    //     },
    //     PERMISSION_EMAIL_LOGS:{
    //         key: 'PERMISSION_EMAIL_LOGS',
    //         display: ''
    //     },
    //     PERMISSION_STATISTIC:{
    //         key: 'PERMISSION_STATISTIC',
    //         display: ''
    //     },
    // }
}