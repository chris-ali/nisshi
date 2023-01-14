import { Route } from '@angular/router';
import { AuthGuard } from 'app/core/auth/guards/auth.guard';
import { NoAuthGuard } from 'app/core/auth/guards/noAuth.guard';
import { InitialDataResolver } from 'app/app.resolvers';
import { DefaultLayoutComponent } from './containers';

// @formatter:off
// tslint:disable:max-line-length
export const appRoutes: Route[] = [

    // Redirect empty path to '/dashboard'
    {path: '', pathMatch : 'full', redirectTo: 'dashboard'},

    // Redirect signed in user to the '/dashboard'
    //
    // After the user signs in, the sign in page will redirect the user to the 'signed-in-redirect'
    // path. Below is another redirection for that path to redirect the user to the desired
    // location. This is a small convenience to keep all main routes together here on this file.
    {path: 'signed-in-redirect', pathMatch : 'full', redirectTo: 'dashboard'},

    // Auth routes for guests
    {
        path: '',
        canActivate: [NoAuthGuard],
        canActivateChild: [NoAuthGuard],
        data: {
            layout: 'empty'
        },
        children: [
            // {path: 'confirmation-required', loadChildren: () => import('app/modules/auth/confirmation-required/confirmation-required.module').then(m => m.AuthConfirmationRequiredModule)},
            {path: 'forgot-password', loadChildren: () => import('app/modules/auth/forgot-password/forgot-password.module').then(m => m.AuthForgotPasswordModule)},
            // {path: 'reset-password', loadChildren: () => import('app/modules/auth/reset-password/reset-password.module').then(m => m.AuthResetPasswordModule)},
            {path: 'sign-in', loadChildren: () => import('app/modules/auth/sign-in/sign-in.module').then(m => m.AuthSignInModule)},
            {path: 'sign-up', loadChildren: () => import('app/modules/auth/sign-up/sign-up.module').then(m => m.AuthSignUpModule)}
        ]
    },

    // Auth routes for authenticated users
    {
        path: '',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        data: {
            layout: 'empty'
        },
        children: [
            {path: 'sign-out', loadChildren: () => import('app/modules/auth/sign-out/sign-out.module').then(m => m.AuthSignOutModule)},
        ]
    },

    // Landing routes
    {
        path: '',
        data: {
            layout: 'empty'
        },
        children   : [
            {path: 'home', loadChildren: () => import('app/modules/landing/home/home.module').then(m => m.LandingHomeModule)},
        ]
    },

    // Authenticated routes
    {
        path       : '',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        component: DefaultLayoutComponent,
        resolve    : {
            initialData: InitialDataResolver,
        },
        children   : [
            {path: 'dashboard', loadChildren: () => import('app/modules/dashboard/dashboard.module').then(m => m.DashboardModule)},
            {path: 'aircraft/view', loadChildren: () => import('app/modules/aircraft/view/aircraft-view.module').then(m => m.AircraftViewModule)},
            {path: 'aircraft', loadChildren: () => import('app/modules/aircraft/form/aircraft-form.module').then(m => m.AircraftFormModule)},
            {path: 'logbook/view', loadChildren: () => import('app/modules/logbook/view/logbook-view.module').then(m => m.LogbookViewModule)},
            {path: 'logbook', loadChildren: () => import('app/modules/logbook/form/logbook-form.module').then(m => m.LogbookFormModule)},
            {path: 'vehicle/view', loadChildren: () => import('app/modules/vehicle/view/vehicle-view.module').then(m => m.VehicleViewModule)},
            {path: 'vehicle', loadChildren: () => import('app/modules/vehicle/form/vehicle-form.module').then(m => m.VehicleFormModule)},
            {path: 'maintenance/view', loadChildren: () => import('app/modules/maintenance/view/maintenance-view.module').then(m => m.MaintenanceViewModule)},
            {path: 'maintenance', loadChildren: () => import('app/modules/maintenance/form/maintenance-form.module').then(m => m.MaintenanceFormModule)},
            {path: 'settings', loadChildren: () => import('app/modules/settings/settings.module').then(m => m.SettingsModule)},
        ]
    }
];

