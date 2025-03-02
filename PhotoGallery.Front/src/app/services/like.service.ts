import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { APIResponse } from "../shared/api-response";
import { LikeModel } from "../shared/models/like.model";

@Injectable()
export class LikeService {
    private apiUrl = `${environment.apiHttpUrl}Likes`; // Replace with your API base URL
  
    constructor(private http: HttpClient) {}
  
    cudLike(likeDTO: LikeModel): Observable<APIResponse<LikeModel>> {
      return this.http.put<APIResponse<LikeModel>>(this.apiUrl, likeDTO);
    }
  }