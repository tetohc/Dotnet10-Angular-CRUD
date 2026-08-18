using Microsoft.AspNetCore.Mvc;
using SalesApp.API.Business;
using SalesApp.API.Dtos;

namespace SalesApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController(SalesBusiness _salesBusiness) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<int>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<int>))]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequestDto requestDto)
        {
            var response = await _salesBusiness.CreateAsync(requestDto);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<GetSaleResponseDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<GetSaleResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<GetSaleResponseDto>))]
        public async Task<IActionResult> GetSaleById(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<GetSaleResponseDto>.Failure("El ID de la venta debe ser válido."));

            var response = await _salesBusiness.GetByIdAsync(id);
            if (!response.IsSuccess)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<GetSaleResponseDto>>))]
        public async Task<IActionResult> GetSales([FromQuery] GetSaleQueryRequestDto queryDto)
        {
            var response = await _salesBusiness.GetAllAsync(queryDto);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
    }
}