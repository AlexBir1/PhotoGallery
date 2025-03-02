import { Component, isSignal, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { UIAuthService } from 'src/app/services/ui-auth.service';
import { SignInModel } from 'src/app/shared/models/sign-in.model';
import { SignUpModel } from 'src/app/shared/models/sign-up.model';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit{
  isLoading: boolean = false;
  isSignIn: boolean = false;
  errors: string[] = [];
  authForm!: FormGroup;

  title = 'PhotoGallery';

  constructor(private authService: AuthService, private uiaService: UIAuthService, private router: Router){}

  ngOnInit(): void {
    this.authForm = new FormGroup({
        username: new FormControl('', Validators.required),
        email: new FormControl('', Validators.required),
        password: new FormControl('', Validators.required),
        passwordConfirm: new FormControl('', Validators.required),
        keepAuthorized: new FormControl(false, Validators.required),
      });
  }

  enableLoading()
  {
    this.isLoading = true;
  }

  disableLoading()
  {
    this.isLoading = false;
  }

  toggleSignIn()
  {
    this.isSignIn = !this.isSignIn;

    if(this.isSignIn)
      this.authForm = new FormGroup({
        userIdentifier: new FormControl('', Validators.required),
        password: new FormControl('', Validators.required),
        keepAuthorized: new FormControl(false, Validators.required),
      });
    else
      this.authForm = new FormGroup({
        username: new FormControl('', Validators.required),
        email: new FormControl('', Validators.required),
        password: new FormControl('', Validators.required),
        passwordConfirm: new FormControl('', Validators.required),
        keepAuthorized: new FormControl(false, Validators.required),
      });
  }

  signPerson()
  {
    this.enableLoading();

    if(this.isSignIn)
      this.authService.signIn(this.authForm.value as SignInModel).subscribe({
        next: (r) => {
          if(r.success){
            this.uiaService.setAuthData(r.data);
            this.router.navigateByUrl("/Albums")
          }
          else{
            this.errors = r.errors;
          }
        },
        error: (e) => {
          this.errors = ['Connection to API has failed']
        }
      }).add(() => this.disableLoading());

    else
      this.authService.signUp(this.authForm.value as SignUpModel).subscribe({
        next: (r) => {
          if(r.success){
            this.uiaService.setAuthData(r.data);
            this.router.navigateByUrl("/Albums")
          }
          else{
            this.errors = r.errors;
          }
        },
        error: (e) => {
          this.errors = ['Connection to API has failed']
        }
      }).add(() => this.disableLoading());
  }

  closeAlert(){
    this.errors = [];
  }
}
