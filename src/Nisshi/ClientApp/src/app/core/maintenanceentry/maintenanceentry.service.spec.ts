import { TestBed } from '@angular/core/testing';

import { MaintenanceEntryService } from './maintenanceentry.service';

describe('MaintenanceEntryService', () => {
  let service: MaintenanceEntryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MaintenanceEntryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
