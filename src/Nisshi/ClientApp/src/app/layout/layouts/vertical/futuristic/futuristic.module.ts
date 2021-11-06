import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { FuseFullscreenModule } from '@fuse/components/fullscreen';
import { FuseNavigationModule } from '@fuse/components/navigation';
import { LanguagesModule } from 'app/layout/common/languages/languages.module';
import { NotificationsModule } from 'app/layout/common/notifications/notifications.module';
import { UserModule } from 'app/layout/common/user/user.module';
import { SharedModule } from 'app/shared/shared.module';
import { FuturisticLayoutComponent } from 'app/layout/layouts/vertical/futuristic/futuristic.component';

@NgModule({
    declarations: [
        FuturisticLayoutComponent
    ],
    imports     : [
        HttpClientModule,
        RouterModule,
        MatButtonModule,
        MatDividerModule,
        MatIconModule,
        MatMenuModule,
        FuseFullscreenModule,
        FuseNavigationModule,
        LanguagesModule,
        NotificationsModule,
        UserModule,
        SharedModule
    ],
    exports     : [
        FuturisticLayoutComponent
    ]
})
export class FuturisticLayoutModule
{
}
