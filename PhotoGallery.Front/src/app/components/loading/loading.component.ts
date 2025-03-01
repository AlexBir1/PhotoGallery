import { Component } from '@angular/core';

@Component({
  selector: 'app-loading',
  template: `
            <div class="loader_wrapper">
              <div class="loader_box">
                <span class="loader"></span>
              </div>
            </div>
  `,
  styleUrls: ['./loading.component.css']
})
export class LoadingComponent {

}
