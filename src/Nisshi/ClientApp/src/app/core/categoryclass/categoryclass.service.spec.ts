import { TestBed } from '@angular/core/testing';

import { CategoryclassService } from './categoryclass.service';

describe('CategoryclassService', () => {
  let service: CategoryclassService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CategoryclassService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
