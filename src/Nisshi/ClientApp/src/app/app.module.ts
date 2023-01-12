import { NgModule } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
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
import { HashLocationStrategy, LocationStrategy, PathLocationStrategy } from '@angular/common';

import {
    PERFECT_SCROLLBAR_CONFIG,
    PerfectScrollbarConfigInterface,
    PerfectScrollbarModule,
  } from 'ngx-perfect-scrollbar';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
    suppressScrollX: true
  };

import {
    DefaultFooterComponent,
    DefaultHeaderComponent,
    DefaultLayoutComponent,
} from './containers';

import {
    AvatarModule,
    BadgeModule,
    BreadcrumbModule,
    ButtonGroupModule,
    ButtonModule,
    CardModule,
    DropdownModule,
    FooterModule,
    FormModule,
    GridModule,
    HeaderModule,
    ListGroupModule,
    NavModule,
    ProgressModule,
    SharedModule,
    SidebarModule,
    TabsModule,
    UtilitiesModule,
} from '@coreui/angular';

import { IconModule, IconSetService } from '@coreui/icons-angular';

const routerConfig: ExtraOptions = {
    preloadingStrategy       : PreloadAllModules,
    scrollPositionRestoration: 'enabled'
};

const APP_CONTAINERS = [
    DefaultFooterComponent,
    DefaultHeaderComponent,
    DefaultLayoutComponent,
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

        // CoreUI components
        AvatarModule,
        BreadcrumbModule,
        FooterModule,
        DropdownModule,
        GridModule,
        HeaderModule,
        SidebarModule,
        IconModule,
        PerfectScrollbarModule,
        NavModule,
        ButtonModule,
        FormModule,
        UtilitiesModule,
        ButtonGroupModule,
        //ReactiveFormsModule,
        SidebarModule,
        SharedModule,
        TabsModule,
        ListGroupModule,
        ProgressModule,
        BadgeModule,
        ListGroupModule,
        CardModule,

        // 3rd party modules that require global configuration via forRoot
        MarkdownModule.forRoot({})
    ],
    providers: [
        {
          provide: LocationStrategy,
          useClass: HashLocationStrategy,
        },
        {
          provide: PERFECT_SCROLLBAR_CONFIG,
          useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG,
        },
        IconSetService,
        Title
      ],
    bootstrap   : [
        AppComponent
    ]
})
export class AppModule
{
}
