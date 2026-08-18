namespace SalesApp.API.Dtos
{
    #region Request

    public record CreateSaleRequestDto(string CustomerName, int PaymentType, decimal Total, IEnumerable<CreateSaleDetailRequestDto> Details);

    public record CreateSaleDetailRequestDto(string ProductName, int Quantity, decimal UnitPrice);

    public record GetSaleQueryRequestDto(int PageNumber, int PageSize);

    #endregion Request

    #region Response

    public record GetSaleResponseDto(int Id, string CustomerName, string PaymentType, decimal Total, string SaleDate,
        IEnumerable<GetSaleDetailResponseDto>? Details = null);

    public record GetSaleDetailResponseDto(int SaleDetailId, int SaleId, string ProductName, int Quantity, decimal UnitPrice);

    public record GetSaleQueryResponseDto(int TotalCount, IEnumerable<GetSaleResponseDto> Sales, int Page, int PageSize);

    #endregion Response
}