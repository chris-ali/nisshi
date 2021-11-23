import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { FuseNavigationModule } from '@fuse/components/navigation/navigation.module';
import { LogbookSidebarComponent } from 'app/modules/logbook/sidebar/logbook-sidebar.component';

@NgModule({
    declarations: [
        LogbookSidebarComponent
    ],
    imports     : [
        RouterModule.forChild([]),
        MatIconModule,
        MatProgressBarModule,
        FuseNavigationModule
    ],
    exports     : [
        LogbookSidebarComponent
    ]
})
export class LogbookSidebarModule
{
}
