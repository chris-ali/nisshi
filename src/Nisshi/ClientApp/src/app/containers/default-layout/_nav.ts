import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: 'Home',
    url: '/dashboard',
    iconComponent: { name: 'cil-home' }
  },
  {
    title: true,
    name: 'Logbook'
  },
  {
    name: 'Aircraft',
    url: '/aircraft/view',
    iconComponent: { name: 'cil-airplane-mode' }
  },
  {
    name: 'Logbook',
    url: '/logbook/view',
    iconComponent: { name: 'cil-book' }
  },
//   {
//     name: 'Trip',
//     url: '/trip',
//     iconComponent: { name: 'cil-globe-alt' }
//   },
//   {
//     name: 'Currency',
//     url: '/currency',
//     iconComponent: { name: 'cil-calendar' }
//   },
  {
    name: 'Maintenance',
    title: true
  },
  {
    name: 'Vehicles',
    url: '/vehicle/view',
    iconComponent: { name: 'cil-garage' }
  },
];
