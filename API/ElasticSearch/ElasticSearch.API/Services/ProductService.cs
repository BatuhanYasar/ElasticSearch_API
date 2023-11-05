using Elastic.Clients.Elasticsearch;
using ElasticSearch.API.DTOs;
using ElasticSearch.API.Models;
using ElasticSearch.API.Repositories;
using System.Collections.Immutable;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace ElasticSearch.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        //loglamak için
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }



        // veri kaydetme
        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {
            var responseProduct = await _productRepository.SaveAsync(request.CreateProduct()); // Product geri dönüyor.

            if (responseProduct == null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string> { "kayıt esnasında bir hata meydana geldi." }, System.Net.HttpStatusCode.InternalServerError);
            }

            return ResponseDto<ProductDto>.Success(responseProduct.CreateDto(), System.Net.HttpStatusCode.Created);
        }




        // tüm verileri çekme
        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            var productListDto = new List<ProductDto>();

            // var productListDto= products.Select(x =>new ProductDto(x.Id,x.Name,x.Price,x.Stock,new ProductFeatureDto(x.Feature.Width,x.Feature.Height,x.Feature.Color))).ToList();


            foreach (var x in products)
            {
                if (x.Feature == null)
                {
                    productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, null));
                }
                else
                {
                    productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature.Width, x.Feature.Height, x.Feature.Color.ToString())));
                }
            }


            return ResponseDto<List<ProductDto>>.Success(productListDto, System.Net.HttpStatusCode.OK);
        }



        // id ye göre veri çekme
        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {
            var hasProduct = await _productRepository.GetByIdAsync(id);
            // has dememizin sebebi bu veriler var mı yok mu onu bilmiyoruz o sebeple if ile döndürdük.

            if (hasProduct == null) // Ürün bulunamadı
            {
                return ResponseDto<ProductDto>.Fail("ürün bulunamadı", System.Net.HttpStatusCode.NotFound);
            }

            //
            return ResponseDto<ProductDto>.Success(hasProduct.CreateDto(), HttpStatusCode.OK);
            // CreateDto() ise Feature kısmı null veya değil ona göre döndürüyor.
        }


        // verimizi güncelleme

        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updateProduct)
        {
            var isSuccess = await _productRepository.UpdateAsync(updateProduct);

            if(!isSuccess)
            {
                return ResponseDto<bool>.Fail(new List<string> { "update esnasında bir hata meydana geldi." }, System.Net.HttpStatusCode.InternalServerError);
            }

            return ResponseDto<bool>.Success(true, System.Net.HttpStatusCode.NoContent);
        }



        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var  deleteResponse = await _productRepository.DeleteAsync(id);

            if(!deleteResponse.IsValidResponse && deleteResponse.Result==Result.NotFound)
            {

                return ResponseDto<bool>.Fail(new List<string> { "Silmeye çalıştığını ürün bulunamamıştır.." }, System.Net.HttpStatusCode.NotFound);
            }

            if(!deleteResponse.IsValidResponse)
            {
                deleteResponse.TryGetOriginalException(out Exception? exception);

                // Çıkan hatayı döndürüyoruz
                _logger.LogError(exception, deleteResponse.ElasticsearchServerError.ToString());

                return ResponseDto<bool>.Fail(new List<string> { "delete esnasında bir hata meydana geldi." }, System.Net.HttpStatusCode.InternalServerError);
            }

            return ResponseDto<bool>.Success(true,HttpStatusCode.NoContent);
        }



    }
}
