import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { APIResponse } from "../shared/api-response";
import { AuthorizationModel } from "../shared/models/authorization.model";
import { SignInModel } from "../shared/models/sign-in.model";
import { SignUpModel } from "../shared/models/sign-up.model";
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AuthService {
    private apiUrl = `${environment.apiHttpUrl}Auth`; // Replace with your API base URL
  
    constructor(private http: HttpClient) {}

    signUp(signUpModel: SignUpModel): Observable<APIResponse<AuthorizationModel>> {
      return this.http.post<APIResponse<AuthorizationModel>>(`${this.apiUrl}/SignUp`, signUpModel);
    }
  
    signIn(signInModel: SignInModel): Observable<APIResponse<AuthorizationModel>> {
      return this.http.post<APIResponse<AuthorizationModel>>(`${this.apiUrl}/SignIn`, signInModel);
    }
  }