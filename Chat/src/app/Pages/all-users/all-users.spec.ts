import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllUsers } from './all-users';

describe('AllUsers', () => {
  let component: AllUsers;
  let fixture: ComponentFixture<AllUsers>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllUsers]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllUsers);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
