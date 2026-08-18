import { CreateSaleDetailRequest } from "./create-sale-detail-request";

export interface CreateSaleRequest {
    customerName: string;
    paymentType: number;
    total: number;
    details: CreateSaleDetailRequest[];
}
