import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

const OPTIONS = { headers: new HttpHeaders({'Content-Type': 'application/json'}) };

/**
 * Base API service that handles REST verbs and has pipes for additional
 * universal endpoint result handling
 */
@Injectable({
    providedIn: 'root'
})
export class ApiService {

    /**
     * Constructor
     */
    constructor(private _http: HttpClient) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    get(path: string, params: HttpParams = new HttpParams()): Observable<any>
    {
        return this._http.get(`${environment.api_url}${path}`, OPTIONS)
            .pipe(catchError(this.formatErrors));
    }

    put(path: string, body: Object = {}): Observable<any>
    {
        return this._http.put(`${environment.api_url}${path}`, JSON.stringify(body), OPTIONS)
            .pipe(catchError(this.formatErrors));
    }

    post(path: string, body: Object = {}): Observable<any>
    {
        return this._http.post(`${environment.api_url}${path}`, JSON.stringify(body), OPTIONS)
            .pipe(catchError(this.formatErrors));
    }

    delete(path: string): Observable<any>
    {
        return this._http.delete(`${environment.api_url}${path}`, OPTIONS)
            .pipe(catchError(this.formatErrors));
    }

    private formatErrors(error: any)
    {
        return throwError(error.error);
    }
}
