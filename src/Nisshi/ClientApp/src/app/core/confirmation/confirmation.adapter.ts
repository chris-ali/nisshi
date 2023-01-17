import { Injectable } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ConfirmationService } from './confirmation.service';
import { ConfirmationDialogComponent } from './dialog/dialog.component';

/**
 * Adapter to generate unified Fuse confirmation and alert dialog popups
 */
@Injectable({
    providedIn: 'root'
})
export class ConfirmationAdapter
{
    /**
     * Constructor
     */
    constructor(private confirmation: ConfirmationService)
    { }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Opens a confirmation dialog popup with a simple confirm/cancel button and a warn-colored exclamation icon
     *
     * @param title Title of the confirm popup
     * @param message Message of the confirm popup
     * @param confirmTitle Title of the confirm button
     * @param cancelTitle Title of the cancel button
     * @returns Reference to dialog popup
     */
    confirm(title: string, message: string, confirmTitle: string, cancelTitle: string): MatDialogRef<ConfirmationDialogComponent, any>
    {
        return this.confirmation.open({
            "title": title,
            "message": message,
            "icon": {
              "show": true,
              "name": "heroicons_outline:exclamation",
              "color": "warn"
            },
            "actions": {
              "confirm": {
                "show": true,
                "label": confirmTitle,
                "color": "warn"
              },
              "cancel": {
                "show": true,
                "label": cancelTitle
              }
            },
            "dismissible": true
        });
    }

    /**
     * Opens an alert dialog popup
     *
     * @param title Title of the confirm popup
     * @param message Message of the confirm popup
     * @param success (Optional, default false) If true, uses success-colored check icon, otherwise a warn-colored exclamation icon
     * @returns Refrence to dialog popup
     */
    alert(title: string, message: string, success?: boolean): MatDialogRef<ConfirmationDialogComponent, any>
    {
        return this.confirmation.open({
            "title": title,
            "message": message,
            "icon": {
              "show": true,
              "name": success ? "heroicons_outline:check-circle" : "heroicons_outline:exclamation",
              "color": success ? "success" : "warn"
            },
            "actions": {
              "confirm": {
                "show": false,
                "label": "Remove",
                "color": "warn"
              },
              "cancel": {
                "show": false,
                "label": "Cancel"
              }
            },
            "dismissible": true
        });
    }

    /**
     * Given a dictionary of error messages, formats the error
     * into an unordered list string
     *
     * @param error
     * @returns Formated error message
     */
    formatErrors(error: any): string
    {
        var message = '<ul>'

        for (const [key, value] of Object.entries(error?.errors))
            message += `<li>${key}: ${value}</li>`;

        message += '</ul>';

        return message;
    }
}
