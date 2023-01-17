import { Component, OnInit } from '@angular/core';
import en from '@angular/common/locales/en';
import de from '@angular/common/locales/de';
import ja from '@angular/common/locales/ja';
import { registerLocaleData } from '@angular/common';
import { IconSetService } from '@coreui/icons-angular';
import { Title } from '@angular/platform-browser';
import { cilHome, cilAirplaneMode, cilBook, cilGlobeAlt, cilCalendar, cilGarage,
    cilMenu, cilUser, cilSettings, cilLockLocked, cilShortText, cilViewStream } from '@coreui/icons';

@Component({
    selector   : 'body',
    template: '<router-outlet></router-outlet>',
})
export class AppComponent implements OnInit
{
    title = 'Nisshi';

    /**
     * Constructor
     */
    constructor(
        private titleService: Title,
        private iconSetService: IconSetService)
    {
        titleService.setTitle(this.title);
        iconSetService.icons = { cilHome, cilAirplaneMode, cilBook, cilGlobeAlt, cilCalendar, cilGarage,
            cilMenu, cilUser, cilSettings, cilLockLocked, cilShortText, cilViewStream };
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
