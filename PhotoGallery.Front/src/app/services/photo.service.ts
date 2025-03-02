import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { APIResponse } from "../shared/api-response";
import { PhotoModel } from "../shared/models/photo.model";

@Injectable()
  export class PhotoService {
    private apiUrl = `${environment.apiHttpUrl}Photos`; // Replace with your API base URL
  
    constructor(private http: HttpClient) {}
  
    // Get all photos by album ID
    getAllByAlbumId(albumId: string, itemsPerPage: number = 10, selectedPage: number = 1): Observable<APIResponse<PhotoModel[]>> {
      const params = new HttpParams()
        .set('albumId', albumId)
        .set('itemsPerPage', itemsPerPage)
        .set('selectedPage', selectedPage);
  
      return this.http.get<APIResponse<PhotoModel[]>>(`${this.apiUrl}/Album`, { params });
    }
  
    // Upload a new photo
    uploadPhoto(albumId: string, file: File): Observable<APIResponse<PhotoModel>> {
      const formData = new FormData();
      formData.append('file', file);
  
      return this.http.post<APIResponse<PhotoModel>>(`${this.apiUrl}/${albumId}`, formData);
    }
  
    // Delete a photo
    deletePhoto(photoId: string): Observable<APIResponse<PhotoModel>> {
      const params = new HttpParams().set('photoId', photoId);
      return this.http.delete<APIResponse<PhotoModel>>(this.apiUrl, { params });
    }
  }