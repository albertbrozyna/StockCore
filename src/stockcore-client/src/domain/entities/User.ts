import type { RefreshToken  } from "./RefreshToken";

export interface User {
  id: string;
  email: string;
  userName: string;
  phoneNumber?: string;
  emailConfirmed: boolean;

  firstName: string;
  lastName: string;
  isActive: boolean;
  isDeleted: boolean;
  createdAt: Date;
  lastLogin: Date;
  avatarUrl?: string | null;
  refreshTokens: RefreshToken[];
  tenantId: string; 
  isAcceptedByCompany: boolean;
}


export class UserEntity implements User {
  id: string;
  email: string;
  userName: string;
  phoneNumber?: string;
  emailConfirmed: boolean;
  firstName: string;
  lastName: string;
  isActive: boolean;
  isDeleted: boolean;
  createdAt: Date;
  lastLogin: Date;
  avatarUrl?: string | null;
  refreshTokens: RefreshToken[] = [];
  tenantId: string;
  isAcceptedByCompany: boolean;

  constructor(data: Partial<User>) {
    this.id = data.id || '';
    this.email = data.email || '';
    this.userName = data.email || '';
    this.firstName = data.firstName || '';
    this.lastName = data.lastName || '';
    this.isActive = data.isActive ?? true;
    this.isDeleted = data.isDeleted ?? false;
    this.createdAt = data.createdAt || new Date();
    this.lastLogin = data.lastLogin || new Date();
    this.tenantId = data.tenantId || '';
    this.isAcceptedByCompany = data.isAcceptedByCompany ?? false;
    this.emailConfirmed = data.emailConfirmed ?? false;
    this.avatarUrl = data.avatarUrl;
  }


}