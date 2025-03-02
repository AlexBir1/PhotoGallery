import { LikeModel } from "./like.model";

export interface PhotoModel {
    id: string;
    filename: string;
    uploadedDate: Date;
    albumId: string;
    likes: LikeModel[];
  }