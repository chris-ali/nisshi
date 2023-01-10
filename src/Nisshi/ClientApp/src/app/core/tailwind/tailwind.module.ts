import { NgModule } from '@angular/core';
import { TailwindService } from 'app/core/tailwind/tailwind.service';

@NgModule({
    providers: [
        TailwindService
    ]
})
export class TailwindConfigModule
{
    /**
     * Constructor
     */
    constructor(private _tailwindConfigService: TailwindService)
    {
    }
}
