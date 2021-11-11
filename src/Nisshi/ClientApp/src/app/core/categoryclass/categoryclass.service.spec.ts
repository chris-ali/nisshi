import { TestBed } from '@angular/core/testing';

import { CategoryClassService } from './categoryclass.service';

describe('CategoryclassService', () => {
  let service: CategoryClassService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CategoryClassService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
