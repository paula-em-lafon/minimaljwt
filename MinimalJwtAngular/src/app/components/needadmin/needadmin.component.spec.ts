import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NeedadminComponent } from './needadmin.component';

describe('NeedadminComponent', () => {
  let component: NeedadminComponent;
  let fixture: ComponentFixture<NeedadminComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NeedadminComponent]
    });
    fixture = TestBed.createComponent(NeedadminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
