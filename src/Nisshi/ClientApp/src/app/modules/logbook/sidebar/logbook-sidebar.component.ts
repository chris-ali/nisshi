import { Component, ViewEncapsulation } from '@angular/core';
import { FuseNavigationItem } from '@fuse/components/navigation/navigation.types';

@Component({
    selector     : 'logbook-sidebar',
    template     : `
        <div class="py-10">
            <!-- Add any extra content that might be supplied with the component -->
            <div class="extra-content">
                <ng-content></ng-content>
            </div>

            <!-- Fixed demo sidebar -->
            <div class="mx-6 text-3xl font-bold tracking-tighter">Demo Sidebar</div>
            <fuse-vertical-navigation
                [appearance]="'default'"
                [navigation]="menuData"
                [inner]="true"
                [mode]="'side'"
                [name]="'logbook-sidebar-navigation'"
                [opened]="true"></fuse-vertical-navigation>
        </div>
    `,
    styles       : [
        `
            logbook-sidebar fuse-vertical-navigation .fuse-vertical-navigation-wrapper {
                box-shadow: none !important;
            }
        `
    ],
    encapsulation: ViewEncapsulation.None
})
export class LogbookSidebarComponent
{
    menuData: FuseNavigationItem[];

    /**
     * Constructor
     */
    constructor()
    {
        this.menuData = [
            {
                title   : 'Actions',
                subtitle: 'Task, project & team',
                type    : 'group',
                children: [
                    {
                        title: 'Create task',
                        type : 'basic',
                        icon : 'heroicons_outline:plus-circle'
                    },
                    {
                        title: 'Create team',
                        type : 'basic',
                        icon : 'heroicons_outline:user-group'
                    },
                    {
                        title: 'Create project',
                        type : 'basic',
                        icon : 'heroicons_outline:briefcase'
                    },
                    {
                        title: 'Create user',
                        type : 'basic',
                        icon : 'heroicons_outline:user-add'
                    },
                    {
                        title   : 'Assign user or team',
                        subtitle: 'Assign to a task or a project',
                        type    : 'basic',
                        icon    : 'heroicons_outline:badge-check'
                    }
                ]
            },
            {
                title   : 'Tasks',
                type    : 'group',
                children: [
                    {
                        title: 'All tasks',
                        type : 'basic',
                        icon : 'heroicons_outline:clipboard-list',
                        badge: {
                            title  : '49',
                            classes: 'px-2 bg-primary text-on-primary rounded-full'
                        }
                    },
                    {
                        title: 'Ongoing tasks',
                        type : 'basic',
                        icon : 'heroicons_outline:clipboard-copy'
                    },
                    {
                        title: 'Completed tasks',
                        type : 'basic',
                        icon : 'heroicons_outline:clipboard-check'
                    },
                    {
                        title: 'Abandoned tasks',
                        type : 'basic',
                        icon : 'heroicons_outline:clipboard'
                    },
                    {
                        title: 'Assigned to me',
                        type : 'basic',
                        icon : 'heroicons_outline:user'
                    },
                    {
                        title: 'Assigned to my team',
                        type : 'basic',
                        icon : 'heroicons_outline:users'
                    }
                ]
            },
            {
                type: 'divider'
            }
        ];
    }
}
