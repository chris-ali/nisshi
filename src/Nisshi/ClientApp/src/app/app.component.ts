import { Component, OnInit } from '@angular/core';
import en from '@angular/common/locales/en-US-POSIX';
import de from '@angular/common/locales/de';
import ja from '@angular/common/locales/ja';
import { registerLocaleData } from '@angular/common';

@Component({
    selector   : 'app-root',
    templateUrl: './app.component.html',
    styleUrls  : ['./app.component.scss']
})
export class AppComponent implements OnInit
{
    /**
     * Constructor
     */
    constructor()
    {
    }

    ngOnInit()
    {
        registerLocaleData(en, 'en-US');
        registerLocaleData(de, 'de-DE');
        registerLocaleData(ja, 'ja-JA');
    }
}
