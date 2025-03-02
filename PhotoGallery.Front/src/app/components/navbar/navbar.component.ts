import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UIAuthService } from 'src/app/services/ui-auth.service';
import { AuthorizationModel } from 'src/app/shared/models/authorization.model';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  currentAccount!: Observable<AuthorizationModel | null>;
  constructor(private uiaService: UIAuthService, private router: Router)
  {
    this.currentAccount = uiaService.currentAccount$;
  }

  logOut()
  {
    this.uiaService.clearAuthData();
    this.router.navigateByUrl("/Albums")
  }
}
