import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { AuthorizationModel } from "../shared/models/authorization.model";
import { environment } from "src/environments/environment";

@Injectable()
export class UIAuthService{
    private accountSource = new BehaviorSubject<AuthorizationModel | null>(null);
    private authKey = environment.authKey;
    
    currentAccount$ = this.accountSource.asObservable();

    setAuthData(authData: AuthorizationModel): void {
        this.accountSource.next(authData);
        localStorage.setItem(this.authKey, JSON.stringify(authData));
      }
      clearAuthData(): void {
        this.accountSource.next(null);
        localStorage.removeItem(this.authKey);
      }
}