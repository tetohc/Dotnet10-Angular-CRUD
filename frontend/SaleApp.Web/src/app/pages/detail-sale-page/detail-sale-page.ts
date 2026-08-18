import { Component, inject, signal } from '@angular/core';
import { SaleService } from '../../services/sale-service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { GetSaleResponse } from '../../interfaces/get-sale-response';
import Swal from 'sweetalert2';
import { Navbar } from "../../components/navbar/navbar";

@Component({
  selector: 'app-detail-sale-page',
  imports: [RouterLink, Navbar],
  templateUrl: './detail-sale-page.html',
  styleUrl: './detail-sale-page.css',
})
export class DetailSalePage {
  private activateRoute = inject(ActivatedRoute);
  private saleService = inject(SaleService);
  protected saleModel = signal<GetSaleResponse>({
    id: 0,
    customerName: "",
    paymentType: "",
    total: 0,
    saleDate: "",
    details: [{
      saleDetailId: 0,
      saleId: 0,
      productName: "",
      quantity: 1,
      unitPrice: 0
    }]
  });

  constructor() {
    this.activateRoute.params.subscribe((param) => {
      this.saleService.getById(param["id"]).subscribe({
        next: response => {
          if (response.isSuccess) {
            this.saleModel.set(response.data);
          } else {
            Swal.fire({
              title: "¡Algo salió mal!",
              text: response.message,
              icon: "error",
              confirmButtonText: "Aceptar"
            });
          }
        },
        error: err => {
          Swal.fire({
            title: "¡Error en la petición!",
            text: err.error?.message || "No se pudo obtener los datos de la venta.",
            icon: "error",
            confirmButtonText: "Aceptar"
          });
        }
      });
    })
  }
}
