import { TestBed } from '@angular/core/testing';

import { HomeAuthguardService } from './homeauthguard.service';

describe('HomeauthguardService', () => {
  let service: HomeAuthguardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HomeAuthguardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
