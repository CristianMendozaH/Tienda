import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  // --- PROPIEDADES PARA EL FORMULARIO ---
  credentials = {
    correoElectronico: '',
    password: ''
  };
  error: string = '';

  // --- PROPIEDADES PARA LA INTERFAZ DE USUARIO (UI) ---
  showPassword = false;
  isForgotPasswordModalVisible = false;
  isSuccessModalVisible = false;

  constructor(private authService: AuthService, private router: Router) {}

  // --- LÓGICA DE AUTENTICACIÓN ---
  onSubmit() {
    this.error = ''; // Limpiar errores previos
    if (!this.credentials.correoElectronico || !this.credentials.password) {
      this.error = 'Por favor, ingrese sus credenciales.';
      return;
    }

    this.authService.login(this.credentials).subscribe({
      next: (res: any) => {
        if (res.token) {
          localStorage.setItem('token', res.token);
          localStorage.setItem('usuario', this.credentials.correoElectronico);

          // En lugar de redirigir, mostramos el modal de éxito
          this.isSuccessModalVisible = true;
        } else {
            this.error = 'Respuesta inesperada del servidor.';
        }
      },
      error: () => {
        this.error = 'Las credenciales son incorrectas.';
      }
    });
  }

  // --- MÉTODOS PARA CONTROLAR LA UI ---

  /** Redirige al dashboard y cierra el modal de éxito. */
  redirectToDashboard() {
    this.isSuccessModalVisible = false;
    this.router.navigate(['/dashboard']);
  }

  /** Cambia la visibilidad del campo de contraseña. */
  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  /** Abre el modal para recuperar contraseña. */
  openForgotPassword() {
    this.isForgotPasswordModalVisible = true;
  }

  /** Cierra el modal para recuperar contraseña. */
  closeForgotPassword() {
    this.isForgotPasswordModalVisible = false;
  }

  /** Lógica simulada para enviar correo de recuperación. */
  sendRecovery() {
    // Aquí puedes agregar la lógica para llamar a tu servicio de recuperación
    alert('Se han enviado las instrucciones de recuperación.');
    this.closeForgotPassword();
  }
}
