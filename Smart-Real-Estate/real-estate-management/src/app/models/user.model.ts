export enum UserType {
    LegalEntity = 'LegalEntity',
    Individual = 'Individual',
    Admin = 'Admin'
}

export interface User {
    id: string;
    username: string;
    password: string;
    email: string;
    verified: boolean;
    rating: number;
    type: UserType;
    propertyHistory?: string[];
}