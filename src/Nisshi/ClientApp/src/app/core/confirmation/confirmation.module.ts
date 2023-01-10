import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { ConfirmationService } from './confirmation.service';
import { ConfirmationDialogComponent } from 'app/core/confirmation/dialog/dialog.component';
import { CommonModule } from '@angular/common';

@NgModule({
    declarations: [
        ConfirmationDialogComponent
    ],
    imports: [
        MatButtonModule,
        MatDialogModule,
        MatIconModule,
        CommonModule
    ],
    providers   : [
        ConfirmationService
    ]
})
export class ConfirmationModule
{
    /**
     * Constructor
     */
    constructor(private _confirmationService: ConfirmationService)
    {
    }
}
