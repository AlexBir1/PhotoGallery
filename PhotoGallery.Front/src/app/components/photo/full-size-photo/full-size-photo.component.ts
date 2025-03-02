import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PhotoModel } from 'src/app/shared/models/photo.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-full-size-photo',
  templateUrl: './full-size-photo.component.html',
  styleUrls: ['./full-size-photo.component.css']
})
export class FullSizePhotoComponent {
  @Input() photo!: PhotoModel | null;
  @Output() closeEvent: EventEmitter<any> = new EventEmitter<any>();
  photoPath: string = environment.photoPath;

  close(){
    this.closeEvent.emit();
  }
}
