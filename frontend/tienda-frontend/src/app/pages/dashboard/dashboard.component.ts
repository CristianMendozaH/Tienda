import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  usuario: string = 'Administrador';
  activePage: string = 'dashboard';
  openSubmenus: { [key: string]: boolean } = {};

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.usuario = localStorage.getItem('usuario') || 'Administrador';
  }

  showPage(page: string): void {
    this.activePage = page;
  }

  toggleSubmenu(menuId: string): void {
    this.openSubmenus[menuId] = !this.openSubmenus[menuId];
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
    this.router.navigate(['/login']);
  }
}
