import { Component, effect, inject, signal } from '@angular/core';
import { NgbPagination } from '@ng-bootstrap/ng-bootstrap/pagination';
import { GetSaleResponse } from '../../interfaces/get-sale-response';
import { SaleService } from '../../services/sale-service';
import { GetSalesQueryRequest } from '../../interfaces/get-sales-query-request';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from "@angular/router";
import { Navbar } from "../../components/navbar/navbar";

@Component({
  selector: 'app-list-sales-page',
  imports: [NgbPagination, CurrencyPipe, RouterLink, Navbar],
  templateUrl: './list-sales-page.html',
  styleUrl: './list-sales-page.css',
})
export class ListSalesPage {

  protected currentPage = signal(1);
  protected pageSize = signal(5);
  protected totalRecords = signal(0);
  protected sales = signal<GetSaleResponse[]>([]);

  private saleService = inject(SaleService);

  constructor() {
    effect(() => {
      const query: GetSalesQueryRequest = {
        pageNumber: this.currentPage(),
        pageSize: this.pageSize()
      }

      this.saleService.get(query).subscribe({
        next: response => {
          console.log(response)
          if (response.isSuccess) {
            const { totalCount, sales: items } = response.data;
            this.sales.set(items);
            this.totalRecords.set(totalCount);
          }
        },
        error: (e) => { console.log(e.error) }
      });
    });
  }
}
