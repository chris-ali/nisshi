import { TestBed } from '@angular/core/testing';

import { LogbookEntryService } from './logbookentry.service';

describe('LogbookEntryService', () => {
  let service: LogbookEntryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LogbookEntryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
