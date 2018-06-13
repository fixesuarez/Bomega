import { Component, OnInit } from '@angular/core';
import { TestServiceService } from './test-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(private testService: TestServiceService) {}
  title = 'app';

  ngOnInit() {
    this.testService.getNumbers()
      .subscribe(numbers => this.title = numbers[0] + numbers[1]);
  }

  login(provider: string) {
    this.testService.login(provider);
  }
}
