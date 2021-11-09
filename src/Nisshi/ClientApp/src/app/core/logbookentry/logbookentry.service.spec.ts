import { TestBed } from '@angular/core/testing';

import { LogbookentryService } from './logbookentry.service';

describe('LogbookentryService', () => {
  let service: LogbookentryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LogbookentryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
