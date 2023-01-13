import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: 'Home',
    url: '/dashboard',
    // iconComponent: { name: 'cil-speedometer' }
    iconComponent: { name: 'cil-speedometer' }
  },
  {
    title: true,
    name: 'Logbook'
  },
  {
    name: 'Aircraft',
    url: '/aircraft/view',
    iconComponent: { name: 'cil-speedometer' }
  },
  {
    name: 'Logbook',
    url: '/logbook/view',
    iconComponent: { name: 'cil-speedometer' }
  },
//   {
//     name: 'Trip',
//     url: '/trip',
//     iconComponent: { name: 'cil-speedometer' }
//   },
//   {
//     name: 'Currency',
//     url: '/currency',
//     iconComponent: { name: 'cil-speedometer' }
//   },
  {
    name: 'Maintenance',
    title: true
  },
  {
    name: 'Vehicles',
    url: '/vehicle/view',
    iconComponent: { name: 'cil-speedometer' }
  },
];
