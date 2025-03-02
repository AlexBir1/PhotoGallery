import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlbumService } from 'src/app/services/album.service';
import { APIResponse } from 'src/app/shared/api-response';
import { AlbumModel } from 'src/app/shared/models/album.model';
import { AuthorizationModel } from 'src/app/shared/models/authorization.model';
import { PageParams } from 'src/app/shared/page-params';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.css']
})
export class AlbumComponent implements OnInit{
  albums: AlbumModel[] = [];
  albumF!: FormGroup;
  errors: string[] = [];
  isLoading: boolean = false;
  pageParams: PageParams = new PageParams(0,1,5);
  personId!: string;
  photoPath: string = environment.photoPath;
  currentUser!: AuthorizationModel;
  createMode: boolean = false;

  constructor(
    private albumService: AlbumService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.personId = this.route.snapshot.paramMap.get('personId')!;
    this.currentUser = JSON.parse(localStorage.getItem(environment.authKey)!) as AuthorizationModel;

    if (this.personId) {
      this.loadAlbumsByPersonId();
    } else {
      // Fetch all albums
      this.loadAllAlbums();
    }

    this.albumF = new FormGroup({
      title: new FormControl('', Validators.required),
      createdByPersonId: new FormControl(this.personId, Validators.required),
    });
  }

  closeAlert(){
    this.errors = [];
  }


  openAlbum(albumId: string){
    this.router.navigateByUrl('/Albums/Photos/'+ albumId);
  }

  loadAllAlbums(): void {
    this.enableLoading();
    this.albumService.getAll(this.pageParams.itemsPerPage, this.pageParams.selectedPage).subscribe({
      next: (response) => {
        if (response.success) {
          this.albums = response.data;
          this.pageParams.itemsCount = response.itemsCount;
        }
        else
          this.errors = response.errors;
      },
      error: (err) => {
        this.errors = ['Connection to API has failed']
      },
    }).add(() => this.disableLoading());
  }


  loadAlbumsByPersonId(): void {
    this.enableLoading();
    this.albumService.getMyAlbums(this.pageParams.itemsPerPage, this.pageParams.selectedPage).subscribe({
      next: (response) => {
        if (response.success) {
          this.albums = response.data;
          this.pageParams.itemsCount = response.itemsCount;
        }
        else
          this.errors = response.errors;
      },
      error: (err) => {
        this.errors = ['Connection to API has failed']
      },
    }).add(() => this.disableLoading());
  }


  onPageChange(page: number): void {
    this.pageParams.selectedPage = page;
    if (this.personId) {
      this.loadAlbumsByPersonId();
    } else {
      this.loadAllAlbums();
    }
  }

  createAlbum(){
    this.enableLoading();
    this.albumService.createAlbum(this.albumF.value as AlbumModel).subscribe({
      next: (r) => {
        if(r.success){
          this.albums.push(r.data);
          this.pageParams.itemsCount += 1; 
        }
        else
          this.errors = r.errors;
      },
      error: (r) =>{
        this.errors = ['Connection to API has failed']
      }
    }).add(() => this.disableLoading());
  }

  deleteAlbum(albumId: string){
    this.enableLoading();
    this.albumService.deleteAlbum(albumId).subscribe({
      next: (r) => {
        if(r.success){
          let index = this.albums.findIndex(x=>x.id === albumId);
          this.albums.splice(index, 1);
          this.pageParams.itemsCount -= 1; 
        }
        else
          this.errors = r.errors;
      },
      error: (r) =>{
        this.errors = ['Connection to API has failed']
      }
    }).add(() => this.disableLoading());
  }

  enableLoading()
  {
    this.isLoading = true;
  }

  disableLoading()
  {
    this.isLoading = false;
  }
}
