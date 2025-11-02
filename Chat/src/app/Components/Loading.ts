import { Component, Input, OnInit } from '@angular/core';
import { AnimationOptions, LottieComponent } from 'ngx-lottie';

@Component({
  selector: 'Loading',
  template: '<ng-lottie [width]="width" [options]="lottie" ></ng-lottie>',
  imports: [LottieComponent],
  styles: '',
})
export class Loading implements OnInit{
  @Input() width="";
  @Input() IsBlack=false;
  lottie:AnimationOptions={};
  ngOnInit(): void {

    this.lottie = {
    path: (this.IsBlack)?'/CosmosBlack.json':"/Cosmos.json",
  };
  }
 
}
