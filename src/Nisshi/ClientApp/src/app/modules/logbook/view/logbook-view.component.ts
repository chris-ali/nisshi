import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'logbook-view',
  templateUrl: './logbook-view.component.html',
  encapsulation: ViewEncapsulation.None
})
export class ViewComponent implements OnInit
{
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;

    constructor() { }

    ngOnInit(): void
    {
    }

}
