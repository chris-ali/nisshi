import { Component, OnInit } from '@angular/core';
import en from '@angular/common/locales/en';
import de from '@angular/common/locales/de';
import ja from '@angular/common/locales/ja';
import { registerLocaleData } from '@angular/common';
import { IconSetService } from '@coreui/icons-angular';
import { iconSubset } from './icons/icon-subset';
import { Title } from '@angular/platform-browser';

@Component({
    selector   : 'body',
    template: '<router-outlet></router-outlet>',
})
export class AppComponent implements OnInit
{
    title = 'CoreUI Free Angular Admin Template';

    /**
     * Constructor
     */
    constructor(
        private titleService: Title,
        private iconSetService: IconSetService)
    {
        titleService.setTitle(this.title);
        // iconSet singleton
        iconSetService.icons = { ...iconSubset };
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    ngOnInit()
    {
        registerLocaleData(en, 'en-US');
        registerLocaleData(de, 'de-DE');
        registerLocaleData(ja, 'ja-JA');
    }
}
