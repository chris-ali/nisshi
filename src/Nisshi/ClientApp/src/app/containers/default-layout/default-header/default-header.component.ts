import { Component, Input, OnDestroy, OnInit } from '@angular/core';

import { ClassToggleService, HeaderComponent } from '@coreui/angular';
import { UserService } from 'app/core/user/user.service';
import { User } from 'app/core/user/user.types';
import { Subject, takeUntil } from 'rxjs';

@Component({
    selector: 'app-default-header',
    templateUrl: './default-header.component.html',
})
export class DefaultHeaderComponent extends HeaderComponent implements OnInit, OnDestroy
{
    @Input() sidebarId: string = "sidebar";
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    user: User;

    constructor(private classToggler: ClassToggleService,
                private userService: UserService)
    {
        super();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------


    /**
     * On init
     */
    ngOnInit(): void
    {
        this.userService.user$
        .pipe(takeUntil(this._unsubscribeAll))
        .subscribe((user: User) => {
            this.user = user;
        });
    }

    ngOnDestroy(): void
    {
        this._unsubscribeAll.next(this.user);
        this._unsubscribeAll.complete();
    }
}
