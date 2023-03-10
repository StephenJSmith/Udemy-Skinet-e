using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class ProductsController : BaseApiController
  {
    private readonly IGenericRepository<Product> _productsRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandsRepo;
    private readonly IGenericRepository<ProductType> _productTypesRepo;
    private readonly IMapper _mapper;

    public ProductsController(
      IGenericRepository<Product> productsRepo,
      IGenericRepository<ProductBrand> productBrandsRepo,
      IGenericRepository<ProductType> productTypesRepo,
      IMapper mapper
      )
    {
      _productsRepo = productsRepo;
      _productBrandsRepo = productBrandsRepo;
      _productTypesRepo = productTypesRepo;
      _mapper = mapper;
    }

    [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
    {
      var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
      var products = await _productsRepo.ListAsync(spec);
      var countSpec = new ProductWithFiltersForCountSpecification(productParams);
      var totalItems = await _productsRepo.CountAsync(countSpec);
      var data = _mapper.Map<IReadOnlyList<Product>, 
        IReadOnlyList<ProductToReturnDto>>(products);
      var pagination = new Pagination<ProductToReturnDto> (
        productParams.PageIndex, productParams.PageSize, totalItems, data
      );

      return Ok(pagination);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
      var spec = new ProductsWithTypesAndBrandsSpecification(id);
      var product = await _productsRepo.GetEntityWithSpec(spec);
      if (product == null) return NotFound(new ApiResponse(404));
      
      var dto = _mapper.Map<Product, ProductToReturnDto>(product);

      return Ok(dto);
    }

    [HttpGet]
    [Route("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
      var brands = await _productBrandsRepo.ListAllAsync();

      return Ok(brands);
    }

    [HttpGet]
    [Route("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
      var types = await _productTypesRepo.ListAllAsync();

      return Ok(types);
    }
  }
}