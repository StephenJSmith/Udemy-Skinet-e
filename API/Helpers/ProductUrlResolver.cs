using API.Dtos;
using Core.Entities;
using AutoMapper;

namespace API.Helpers
{
  public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
  {
    private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

    public ProductUrlResolver(Microsoft.Extensions.Configuration.IConfiguration config)
    {
      _config = config;
        
    }
    public string Resolve(Product source, 
        ProductToReturnDto destination, 
        string destMember, 
        ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.PictureUrl)) return null; 

        var fullPath = _config["ApiUrl"] + source.PictureUrl;

        return fullPath;
    }
  }
}