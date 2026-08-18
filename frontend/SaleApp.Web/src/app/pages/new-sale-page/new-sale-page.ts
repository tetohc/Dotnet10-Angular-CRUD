import { Component, computed, inject, signal } from '@angular/core';
import { SaleService } from '../../services/sale-service';
import { form, required, validate, FormField } from '@angular/forms/signals';
import { CreateSaleRequest } from '../../interfaces/create-sale-request';
import Swal from 'sweetalert2';
import { RouterLink } from "@angular/router";
import { Navbar } from "../../components/navbar/navbar";

@Component({
  selector: 'app-new-sale-page',
  imports: [FormField, RouterLink, Navbar],
  templateUrl: './new-sale-page.html',
  styleUrl: './new-sale-page.css',
})
export class NewSalePage {
  private saleService = inject(SaleService);

  private initialSale = {
    customerName: "",
    paymentType: "0",
    total: 0,
    details: [{
      productName: "",
      quantity: 1,
      unitPrice: 0
    }]
  }

  private saleModel = signal(this.initialSale);

  protected saleForm = form(this.saleModel, (schemaPath) => {
    required(schemaPath.customerName, { message: 'El nombre del cliente es requerido.' });
    validate(schemaPath.paymentType, ({ value }) => {
      if (value().match('0'))
        return { kind: "equals", message: "El tipo de pago es requerido." }

      return null;
    });
    required(schemaPath.total, { message: 'Password is required' });
  });

  protected addProduct(): void {
    this.saleForm.details().value.update(current => ([
      ...current,
      {
        productName: "",
        quantity: 1,
        unitPrice: 0
      }
    ]));
  }

  protected removeProduct(index: number): void {
    this.saleForm.details().value.update(current => current.filter((detail, i) => i !== index));
  }

  protected readonly total = computed(() => {
    return this.saleForm.details().value().reduce((sum, detail) => {
      return sum + (detail.quantity * detail.unitPrice);
    }, 0);
  });

  isLoading = signal<boolean>(false);
  protected save(): void {
    this.isLoading.set(true);

    const { customerName, details, paymentType } = this.saleForm().value();
    const request: CreateSaleRequest = {
      customerName: customerName,
      paymentType: Number(paymentType),
      total: this.total(),
      details: details
    }

    this.saleService.create(request).subscribe({
      next: response => {
        this.isLoading.set(false);

        if (response.isSuccess) {
          this.saleModel.set(this.initialSale)
          this.saleForm().reset()

          Swal.fire({
            title: "¡Éxito!",
            text: `El número de venta ${response.data} ha sido registrado correctamente.`,
            icon: "success",
            confirmButtonText: "Aceptar"
          });
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
        this.isLoading.set(false);
        
        Swal.fire({
          title: "¡Error en la petición!",
          text: err.error?.message || "No se pudo procesar la venta.",
          icon: "error",
          confirmButtonText: "Aceptar"
        });
      }
    });
  }

}

