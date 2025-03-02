import { PhotoModel } from "./photo.model";

export interface AlbumModel {
    id: string;
    title: string;
    createdDate: Date;
    createdByPersonId: string;
    photos: PhotoModel[];
  }