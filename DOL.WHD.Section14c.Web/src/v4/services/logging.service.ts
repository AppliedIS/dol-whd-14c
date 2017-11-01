// Modules
import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { WindowRef } from './window.service';
import { customError } from '../../models/customError';


@Injectable()
export class LoggingService {


  constructor(private http: Http, private windowRef: WindowRef) {}

  addLog(error: any): Promise<void> {
    console.log(error);
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let options = new RequestOptions({headers: headers})
    const url = this.windowRef.nativeWindow.__env.api_url + "/api/ErrorLogs/AddLog";
    this.http.post(url, JSON.stringify(error), options).subscribe()
    return
  }

}
