import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlbumService } from 'src/app/services/album.service';
import { LikeService } from 'src/app/services/like.service';
import { PhotoService } from 'src/app/services/photo.service';
import { APIResponse } from 'src/app/shared/api-response';
import { AlbumModel } from 'src/app/shared/models/album.model';
import { AuthorizationModel } from 'src/app/shared/models/authorization.model';
import { LikeModel } from 'src/app/shared/models/like.model';
import { PhotoModel } from 'src/app/shared/models/photo.model';
import { PageParams } from 'src/app/shared/page-params';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo',
  templateUrl: './photo.component.html',
  styleUrls: ['./photo.component.css']
})
export class PhotoComponent {
  photos: PhotoModel[] = [];
  currentAlbumId: string = '';
  currentAlbum!: AlbumModel;
  currentUser!: AuthorizationModel;
  errors: string[] = [];
  isLoading: boolean = false;
  pageParams: PageParams = new PageParams(0,1,5);
  photoPath: string = environment.photoPath;
  selectedPhoto: PhotoModel | null = null;

  constructor(
    private photoService: PhotoService,
    private likeService: LikeService,
    private route: ActivatedRoute,
    private albumService: AlbumService,
  ) {}

  ngOnInit(): void {
    this.currentAlbumId = this.route.snapshot.paramMap.get('albumId')!;
    this.currentUser = JSON.parse(localStorage.getItem(environment.authKey)!) as AuthorizationModel;

    if(this.currentAlbumId){
      this.loadAllPhotos();
      this.loadCurrentAlbum(this.currentAlbumId);
    }
  }

  photoClick(photoId: string){
    this.selectedPhoto = this.photos.find(x=>x.id === photoId)!;
  }

  closePhoto(){
    this.selectedPhoto = null;
  }

  enableLoading()
  {
    this.isLoading = true;
  }

  disableLoading()
  {
    this.isLoading = false;
  }

  closeAlert(){
    this.errors = [];
  }

  // Fetch all photos
  loadAllPhotos(): void {
    this.enableLoading();
    this.photoService.getAllByAlbumId(this.currentAlbumId, this.pageParams.itemsPerPage, this.pageParams.selectedPage).subscribe({
      next: (response: APIResponse<PhotoModel[]>) => {
        if (response.success) {
          this.photos = response.data;
          this.pageParams.itemsCount = response.itemsCount;
          this.pageParams.selectedPage = response.selectedPage;
        }
        else
          this.errors = response.errors;
      },
      error: () => {
        this.errors = ['Connection to API has failed']
      },
    }).add(() => this.disableLoading());
  }

  loadCurrentAlbum(id: string){
    this.enableLoading();
    this.albumService.getById(id).subscribe({
      next: (response: APIResponse<AlbumModel>) => {
        if (response.success) {
          this.currentAlbum = response.data;
        }
        else
          this.errors = response.errors;
      },
      error: () => {
        this.errors = ['Connection to API has failed']
      },
    }).add(() => this.disableLoading());
  }

  // Handle page change event
  onPageChange(page: number): void {
    this.pageParams.selectedPage = page;
    this.loadAllPhotos();
  }

  // Handle Like button click
  onLike(photoId: string): void {
    this.enableLoading();
    var like = this.photos.find(x=>x.id == photoId)?.likes.find(x=>x.personId == this.currentUser.personId);
    this.likeService.cudLike(like ? {id:like.id, isLike: true, personId: this.currentUser.personId, photoId: photoId} : {id:'', isLike: true, personId: this.currentUser.personId, photoId: photoId}).subscribe({
      next: (response) => {
        if (response.success) {
          if(like && like.id === response.data.id && like.isLike === response.data.isLike){
            this.photos.find(x=>x.id == photoId)?.likes.splice(this.photos.find(x=>x.id == photoId)!.likes.findIndex(x=>x.id === response.data.id),1);
          }
          else if(like && like.id === response.data.id && like.isLike !== response.data.isLike){
            this.photos.find(x=>x.id == photoId)?.likes.splice(this.photos.find(x=>x.id == photoId)!.likes.findIndex(x=>x.id === response.data.id),1);
            this.photos.find(x=>x.id == photoId)?.likes.push(response.data);
          }
          else{
            this.photos.find(x=>x.id == photoId)?.likes.push(response.data);
          }
        }
        else
          this.errors = response.errors;
      },
      error: () => {
        this.errors = ['Connection to API has failed']
      },
    }).add(() => this.disableLoading());
  }

  // Handle Dislike button click
  onDislike(photoId: string): void {
    this.enableLoading();
    var like = this.photos.find(x=>x.id == photoId)?.likes.find(x=>x.personId == this.currentUser.personId);
    this.likeService.cudLike(like ? {id:like.id, isLike: false, personId: this.currentUser.personId, photoId: photoId} : {id:'', isLike: false, personId: this.currentUser.personId, photoId: photoId}).subscribe({
      next: (response) => {
        if (response.success) {
          if(like && like.id === response.data.id && like.isLike === response.data.isLike){
            this.photos.find(x=>x.id == photoId)?.likes.splice(this.photos.find(x=>x.id == photoId)!.likes.findIndex(x=>x.id === response.data.id),1);
          }
          else if(like && like.id === response.data.id && like.isLike !== response.data.isLike){
            this.photos.find(x=>x.id == photoId)?.likes.splice(this.photos.find(x=>x.id == photoId)!.likes.findIndex(x=>x.id === response.data.id),1);
            this.photos.find(x=>x.id == photoId)?.likes.push(response.data);
          }
          else{
            this.photos.find(x=>x.id == photoId)?.likes.push(response.data);
          }
        }
        else
          this.errors = response.errors;
      },
      error: () => {
        this.errors = ['Connection to API has failed']
      },
    }).add(() => this.disableLoading());
  }
  
  onUpload(event: any){
    this.enableLoading();
    let file = event?.target?.files[0] as File; 
    this.photoService.uploadPhoto(this.currentAlbumId, file).subscribe({
      next: (response) => {
        if (response.success) {
          this.photos.push(response.data);
          this.pageParams.itemsCount -= 1;
        }
        else
          this.errors = response.errors;
      },
      error: () => {
        this.errors = ['Connection to API has failed']
      },
    }).add(() => this.disableLoading());
  }

  // Handle Delete button click
  onDelete(photoId: string): void {
    this.enableLoading();
    this.photoService.deletePhoto(photoId).subscribe({
      next: (response) => {
        if (response.success) {
          var index = this.photos.findIndex(x=>x.id === photoId);
          this.photos.splice(index,1);
          this.pageParams.itemsCount -= 1;
        }
        else
          this.errors = response.errors;
      },
      error: () => {
        this.errors = ['Connection to API has failed']
      },
    }).add(() => this.disableLoading());
  }

  likeCount(arr: LikeModel[]){
    return arr.filter(x=>x.isLike === true).length;
  }

  dislikeCount(arr: LikeModel[]){
    return arr.filter(x=>x.isLike === false).length;
  }
}
