import { GetSaleDetailResponse } from './get-sale-detail-response';

export interface GetSaleResponse {
    id: number;
    customerName: string;
    paymentType: string;
    total: number;
    saleDate: string;
    details?: GetSaleDetailResponse[];
}
