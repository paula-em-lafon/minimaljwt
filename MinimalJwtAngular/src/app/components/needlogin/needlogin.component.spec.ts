import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NeedloginComponent } from './needlogin.component';

describe('NeedloginComponent', () => {
  let component: NeedloginComponent;
  let fixture: ComponentFixture<NeedloginComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NeedloginComponent]
    });
    fixture = TestBed.createComponent(NeedloginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
