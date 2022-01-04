import { Route } from '@angular/router';
import { SettingsComponent } from './settings.component';

export const settingsRoutes: Route[] = [
    {
        path     : ':view',
        component: SettingsComponent
    }
];
