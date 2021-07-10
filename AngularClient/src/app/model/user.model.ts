export interface User
{    
    username: string;
    token: string;
    fullName:string;
    //photoUrl: string;
    roles: string[];
    appUserId:number;
}