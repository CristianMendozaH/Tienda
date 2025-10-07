// login.component.ts (versión limpia)
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
  credentials = {
    correoElectronico: '',
    password: ''
  };
  error: string = '';
  showPassword = false;
  isForgotPasswordModalVisible = false;
  isSuccessModalVisible = false;

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.error = '';
    if (!this.credentials.correoElectronico || !this.credentials.password) {
      this.error = 'Por favor, ingrese sus credenciales.';
      return;
    }

    this.authService.login(this.credentials).subscribe({
      next: (res: any) => {
        if (res.token) {
          localStorage.setItem('token', res.token);
          localStorage.setItem('usuario', this.credentials.correoElectronico);
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

  redirectToDashboard() {
    this.isSuccessModalVisible = false;
    this.router.navigate(['/dashboard']);
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  openForgotPassword() {
    this.isForgotPasswordModalVisible = true;
  }

  closeForgotPassword() {
    this.isForgotPasswordModalVisible = false;
  }

  sendRecovery() {
    alert('Se han enviado las instrucciones de recuperación.');
    this.closeForgotPassword();
  }
}
