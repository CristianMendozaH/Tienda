import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-caja',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './caja.component.html',
  styleUrls: ['./caja.component.scss']
})
export class CajaComponent {
  titulo = 'Módulo de Caja';

  ventasHoy = 0;
  totalEfectivo = 0;
  totalTarjeta = 0;

  registrarVenta(metodo: string, monto: number) {
    if (metodo === 'efectivo') {
      this.totalEfectivo += monto;
    } else if (metodo === 'tarjeta') {
      this.totalTarjeta += monto;
    }
    this.ventasHoy++;
  }
}
