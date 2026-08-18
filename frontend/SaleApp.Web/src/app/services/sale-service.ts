import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { CreateSaleRequest } from '../interfaces/create-sale-request';
import { Observable } from 'rxjs';
import { ApiResponse } from '../interfaces/api-response';
import { GetSalesQueryRequest } from '../interfaces/get-sales-query-request';
import { GetSalesQueryResponse } from '../interfaces/get-sales-query-response';
import { GetSaleResponse } from '../interfaces/get-sale-response';

@Service()
export class SaleService {
    private http = inject(HttpClient);
    private endPoint = `${environment.apiUrl}/Sales`;

    create(request: CreateSaleRequest): Observable<ApiResponse<number>> {
        return this.http.post<ApiResponse<number>>(this.endPoint, request);
    }

    get(request: GetSalesQueryRequest): Observable<ApiResponse<GetSalesQueryResponse>> {
        return this.http.get<ApiResponse<GetSalesQueryResponse>>(`${this.endPoint}?PageNumber=${request.pageNumber}&PageSize=${request.pageSize}`);
    }

    getById(id: number): Observable<ApiResponse<GetSaleResponse>> {
        return this.http.get<ApiResponse<GetSaleResponse>>(`${this.endPoint}/${id}`);
    }
}
