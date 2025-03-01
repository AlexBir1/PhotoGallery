export interface AuthorizationModel {
    personId: string;
    token: string;
    tokenExpirationDate: Date;
    keepAuthorized: boolean;
  }