import { Component, OnInit } from '@angular/core';
import { AuthorizationModel } from './shared/models/authorization.model';
import { environment } from 'src/environments/environment';
import { UIAuthService } from './services/ui-auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  constructor(private uiaService: UIAuthService, private router: Router){}
  ngOnInit(): void {
    const currentUser: AuthorizationModel = JSON.parse(localStorage.getItem(environment.authKey)!) as AuthorizationModel;
    
        // Check if the token is expired
        if (currentUser && new Date(currentUser.tokenExpirationDate).getTime() < Date.now()) {
          this.uiaService.clearAuthData(); 
          this.router.navigateByUrl('/Albums'); 
        }
        else{
          this.uiaService.setAuthData(currentUser);
        }
  }
  title = 'PhotoGallery';
}
