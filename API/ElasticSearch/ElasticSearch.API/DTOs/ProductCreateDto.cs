using ElasticSearch.API.Models;

namespace ElasticSearch.API.DTOs
{
    // nesne örneği ürettiğimizde propertylerini değiştiremiyoruz.
    public record ProductCreateDto(string Name, decimal Price, int Stock, ProductFeatureDto Feature)
    {

        public Product CreateProduct()
        {
            return new Product
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                Feature = new ProductFeature()
                {
                    Width = Feature.Width,
                    Height = Feature.Height,
                    Color = (EColor)int.Parse(Feature.Color), // önce integer e çevirdik sonra color a
                }
            };
        }



    }

    // Olabildiğince Dto olan sınıfları koymaya çalışalım.
}
