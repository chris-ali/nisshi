import {Component} from '@angular/core';

import { navItems } from '../../core/navigation/navigation.data';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html'
})
export class DefaultLayoutComponent {
  minimized = false;
  public navItems = [...navItems];

  toggleMinimize(e: boolean) {
    this.minimized = e;
  }
}
