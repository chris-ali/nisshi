import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ExtraOptions, PreloadAllModules, RouterModule } from '@angular/router';
import { MarkdownModule } from 'ngx-markdown';
import { CoreModule } from 'app/core/core.module';
import { appConfig } from 'app/core/config/app.config';
import { AppComponent } from 'app/app.component';
import { appRoutes } from 'app/app.routing';
import { ConfirmationModule } from './core/confirmation/confirmation.module';
import { TailwindConfigModule } from './core/tailwind/tailwind.module';
import { ConfigModule } from './core/config/config.module';
import { DefaultLayoutComponent } from './containers/default-layout';

import {
    AppAsideModule,
    AppBreadcrumbModule,
    AppHeaderModule,
    AppFooterModule,
    AppSidebarModule,
  } from '../../projects/coreui/angular/src/public-api';

const routerConfig: ExtraOptions = {
    preloadingStrategy       : PreloadAllModules,
    scrollPositionRestoration: 'enabled'
};

const APP_CONTAINERS = [
    DefaultLayoutComponent
];

@NgModule({
    declarations: [
        AppComponent,
        ...APP_CONTAINERS,
    ],
    imports     : [
        BrowserModule,
        BrowserAnimationsModule,
        RouterModule.forRoot(appRoutes, routerConfig),

        ConfirmationModule,
        TailwindConfigModule,
        ConfigModule.forRoot(appConfig),

        // Core module of your application
        CoreModule,

        // 3rd party modules that require global configuration via forRoot
        MarkdownModule.forRoot({})
    ],
    bootstrap   : [
        AppComponent
    ]
})
export class AppModule
{
}
