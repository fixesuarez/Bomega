import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { AppConfigService } from './app-config.service';

@Injectable()
export class TestServiceService {

  server: string = "http://localhost:5000/";

  constructor(private httpClient: HttpClient, private appConfig: AppConfigService) { }

  getNumbers(): Observable<string[]> {
    const url: string = this.server + "api/values";
    return this.httpClient.get<string[]>(url);
  }

  login(provider: string) {
    var url: string = "";
    switch(provider) {
      case 'Spotify':
        url = this.server + "Account/ExternalLogin?provider=Spotify";
        break;
      case 'Facebook':
        url = this.server + "Account/ExternalLogin?provider=Facebook";
        break;
      }
      
      window.open(url, "Connexion à omega", "menubar=no, status=no, scrollbars=no, menubar=no, width=700, height=700");
  }

}
