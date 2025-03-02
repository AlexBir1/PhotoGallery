export interface AuthorizationModel {
    personId: string;
    token: string;
    tokenExpirationDate: Date;
    roleName: string;
    keepAuthorized: boolean;
  }