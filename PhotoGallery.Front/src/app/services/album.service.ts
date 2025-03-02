import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { APIResponse } from "../shared/api-response";
import { AlbumModel } from "../shared/models/album.model";

@Injectable()
  export class AlbumService {
    private apiUrl = `${environment.apiHttpUrl}Albums`; // Replace with your API base URL
  
    constructor(private http: HttpClient) {}
  
    // Get all albums
    getAll(itemsPerPage: number = 10, selectedPage: number = 1): Observable<APIResponse<AlbumModel[]>> {
      const params = new HttpParams()
        .set('itemsPerPage', itemsPerPage)
        .set('selectedPage', selectedPage);
  
      return this.http.get<APIResponse<AlbumModel[]>>(this.apiUrl, { params });
    }
  
    // Get albums created by the current user
    getMyAlbums(itemsPerPage: number = 10, selectedPage: number = 1): Observable<APIResponse<AlbumModel[]>> {
      const params = new HttpParams()
        .set('itemsPerPage', itemsPerPage)
        .set('selectedPage', selectedPage);
  
      return this.http.get<APIResponse<AlbumModel[]>>(`${this.apiUrl}/My`, { params });
    }
  
    // Create a new album
    createAlbum(albumDTO: AlbumModel): Observable<APIResponse<AlbumModel>> {
      return this.http.post<APIResponse<AlbumModel>>(this.apiUrl, albumDTO);
    }
  
    // Delete an album
    deleteAlbum(albumId: string): Observable<APIResponse<AlbumModel>> {
      const params = new HttpParams().set('albumId', albumId);
      return this.http.delete<APIResponse<AlbumModel>>(this.apiUrl, { params });
    }

    getById(albumId: string): Observable<APIResponse<AlbumModel>>{
      return this.http.get<APIResponse<AlbumModel>>(`${this.apiUrl}/Album/${albumId}`);
    }
  }