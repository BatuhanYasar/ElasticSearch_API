using ElasticSearch.API.Models;

namespace ElasticSearch.API.DTOs
{
    public record ProductFeatureDto(int Width, int Height, string Color)
    {
    }
    // Bu sadece yazım farklılığı(syntax). Yoksa yine çalışırken class oluyor ve propertyler içine yazılıyor.
}
