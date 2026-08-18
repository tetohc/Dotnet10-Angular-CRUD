import { GetSaleResponse } from './get-sale-response';

export interface GetSalesQueryResponse {
    totalCount: number;
    sales: GetSaleResponse[];
    page: number;
    pageSize: number;
}
