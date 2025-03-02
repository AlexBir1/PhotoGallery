import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, throwError, catchError } from "rxjs";
import { UIAuthService } from "../services/ui-auth.service";
import { environment } from "src/environments/environment";
import { AuthorizationModel } from "../shared/models/authorization.model";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private uiaService: UIAuthService, private router: Router) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    // Get the token from the AuthService
    const currentUser: AuthorizationModel = JSON.parse(localStorage.getItem(environment.authKey)!) as AuthorizationModel;

    // Check if the token is expired
    if (currentUser && new Date(currentUser.tokenExpirationDate).getTime() < Date.now()) {
      this.uiaService.clearAuthData(); 
      this.router.navigateByUrl('/Albums'); 
      return throwError('Token expired'); 
    }

    if (currentUser) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser.token}`,
        },
      });
    }
    return next.handle(request);
  }
}