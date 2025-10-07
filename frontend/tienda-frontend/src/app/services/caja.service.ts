import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CajaService {
  private apiUrl = 'http://localhost:5102/api/Caja';

  constructor(private http: HttpClient) {}

  getCajas(): Observable<any> {
    return this.http.get(this.apiUrl);
  }

  abrirCaja(caja: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/abrir`, caja);
  }

  cerrarCaja(id: number, montoFinal: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/cerrar/${id}`, montoFinal);
  }
}
