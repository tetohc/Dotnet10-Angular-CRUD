using Microsoft.EntityFrameworkCore;
using SalesApp.API.Data;
using SalesApp.API.Data.Entities;
using SalesApp.API.Data.Enums;
using SalesApp.API.Dtos;
using System.Globalization;

namespace SalesApp.API.Business
{
    public class SalesBusiness(AppDbContext _dbContext)
    {
        public async Task<ApiResponse<int>> CreateAsync(CreateSaleRequestDto requestDto)
        {
            if (string.IsNullOrEmpty(requestDto.CustomerName))
                return ApiResponse<int>.Failure("El nombre del cliente es requerido.");

            if (!Enum.IsDefined(typeof(PaymentType), requestDto.PaymentType))
                return ApiResponse<int>.Failure("El tipo de pago no es válido.");

            var productNameEmpty = requestDto.Details.Any(x => string.IsNullOrEmpty(x.ProductName));
            if (productNameEmpty)
                return ApiResponse<int>.Failure("El nombre del producto es requerido.");

            var entity = new Sale
            {
                CustomerName = requestDto.CustomerName,
                PaymentType = (PaymentType)requestDto.PaymentType,
                Total = requestDto.Total,
                Details = requestDto.Details.Select(x => new SaleDetail
                {
                    ProductName = x.ProductName?.Trim()!,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                }).ToList()
            };

            _dbContext.Sale.Add(entity);
            await _dbContext.SaveChangesAsync();
            return ApiResponse<int>.Success(entity.Id);
        }

        public async Task<ApiResponse<GetSaleResponseDto>> GetByIdAsync(int saleId)
        {
            var sale = await _dbContext.Sale.Include(x => x.Details).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == saleId);

            if (sale is null)
                return ApiResponse<GetSaleResponseDto>.Failure("La venta no fue encontrada.");

            var result = new GetSaleResponseDto(
                Id: sale.Id,
                CustomerName: sale.CustomerName?.Trim()!,
                PaymentType: Enum.GetName(typeof(PaymentType), sale.PaymentType)?.Trim()!,
                Total: sale.Total,
                SaleDate: sale.SaleDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Details: sale.Details.Select(x => new GetSaleDetailResponseDto(
                    SaleDetailId: x.SaleDetailId,
                    SaleId: x.SaleId,
                    ProductName: x.ProductName?.Trim()!,
                    Quantity: x.Quantity,
                    UnitPrice: x.UnitPrice
                )).ToList()
            );
            return ApiResponse<GetSaleResponseDto>.Success(result);
        }

        public async Task<ApiResponse<GetSaleQueryResponseDto>> GetAllAsync(GetSaleQueryRequestDto requestDto)
        {
            var query = _dbContext.Sale.AsNoTracking();
            var totalCount = await query.CountAsync();

            // paginacion
            var sales = await query
                .OrderByDescending(x => x.SaleDate)
                .Skip((requestDto.PageNumber - 1) * requestDto.PageSize)
                .Take(requestDto.PageSize)
                .ToListAsync();

            var formatSales = sales.Select(x => new GetSaleResponseDto(
                Id: x.Id,
                CustomerName: x.CustomerName?.Trim()!,
                PaymentType: Enum.GetName(typeof(PaymentType), x.PaymentType)?.Trim()!,
                Total: x.Total,
                SaleDate: x.SaleDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            )).ToList();

            var response = new GetSaleQueryResponseDto(totalCount, formatSales, requestDto.PageNumber, requestDto.PageSize);
            return ApiResponse<GetSaleQueryResponseDto>.Success(response);
        }
    }
}