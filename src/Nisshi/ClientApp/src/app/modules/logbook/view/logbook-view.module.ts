import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { SharedModule } from 'app/shared/shared.module';
import { ViewComponent as LogbookViewComponent } from 'app/modules/logbook/view/logbook-view.component';
import { LogbookSidebarModule } from 'app/modules/logbook/sidebar/logbook-sidebar.module';

export const routes: Route[] = [
    {
        path     : '',
        component: LogbookViewComponent
    }
];

@NgModule({
    declarations: [
        LogbookViewComponent
    ],
    imports     : [
        RouterModule.forChild(routes),
        MatButtonModule,
        MatButtonToggleModule,
        MatCheckboxModule,
        MatDividerModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
        MatSidenavModule,
        MatProgressBarModule,
        MatTooltipModule,
        SharedModule,
        LogbookSidebarModule
    ]
})
export class LogbookViewModule
{
}
