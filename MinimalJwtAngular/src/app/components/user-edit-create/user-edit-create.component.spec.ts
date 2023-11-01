import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserEditCreateComponent } from './user-edit-create.component';

describe('UserEditCreateComponent', () => {
  let component: UserEditCreateComponent;
  let fixture: ComponentFixture<UserEditCreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserEditCreateComponent]
    });
    fixture = TestBed.createComponent(UserEditCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
