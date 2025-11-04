import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Chatlist } from './chatlist';

describe('Chatlist', () => {
  let component: Chatlist;
  let fixture: ComponentFixture<Chatlist>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Chatlist]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Chatlist);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
