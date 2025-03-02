import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { LoadingComponent } from './components/loading/loading.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { Route, RouterModule } from '@angular/router';
import { AuthComponent } from './components/auth/auth.component';
import { AlertComponent } from './components/alert/alert.component';
import { AuthService } from './services/auth.service';
import { UIAuthService } from './services/ui-auth.service';
import { LikeService } from './services/like.service';
import { PhotoService } from './services/photo.service';
import { ReactiveFormsModule } from '@angular/forms';
import { AlbumService } from './services/album.service';
import { AlbumComponent } from './components/album/album.component';
import { PhotoComponent } from './components/photo/photo.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { FullSizePhotoComponent } from './components/photo/full-size-photo/full-size-photo.component';

const routes: Route[] = [
  { path: 'Albums/Photos/:albumId', component: PhotoComponent },
  { path: 'Albums/My/:personId', component: AlbumComponent },
  { path: 'Albums', component: AlbumComponent },
  { path: 'Auth', component: AuthComponent },
  { path: '', component: AlbumComponent },
]

@NgModule({
  declarations: [
    AppComponent,
    LoadingComponent,
    NavbarComponent,
    AuthComponent,
    AlertComponent,
    AlbumComponent,
    PhotoComponent,
    FullSizePhotoComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgxPaginationModule,
    RouterModule.forRoot(routes)
  ],
  providers: [AuthService, UIAuthService, LikeService, PhotoService, AlbumService,{provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
