import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { FuseNavigationModule } from '@fuse/components/navigation/navigation.module';
import { LogbookSidebarComponent } from 'app/modules/logbook/sidebar/logbook-sidebar.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { SharedModule } from 'app/shared/shared.module';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';

@NgModule({
    declarations: [
        LogbookSidebarComponent
    ],
    imports     : [
        RouterModule.forChild([]),
        MatIconModule,
        MatProgressBarModule,
        MatButtonModule,
        MatCheckboxModule,
        MatDividerModule,
        MatFormFieldModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatOptionModule,
        MatSelectModule,
        MatMenuModule,
        MatInputModule,
        MatRadioModule,
        FuseNavigationModule,
        SharedModule
    ],
    exports     : [
        LogbookSidebarComponent
    ]
})
export class LogbookSidebarModule
{
}
