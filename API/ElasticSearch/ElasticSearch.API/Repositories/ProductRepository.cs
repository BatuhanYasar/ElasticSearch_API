using ElasticSearch.API.DTOs;
using ElasticSearch.API.Models;
using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;

namespace ElasticSearch.API.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticsearchClient _client;

        private const string indexName = "products"; // sürekli products yazmak kafa karıştırıcı olabilir diyerek bir string olusturduk. 

        public ProductRepository(ElasticsearchClient client)
        {
            _client = client;
        }



        public async Task<Product> SaveAsync(Product newProduct)  
        //Bu metod, Product türünden bir newProduct parametresi alır ve Task<Product> türünden bir sonuç döndürür.
        {
            newProduct.Created = DateTime.Now;

            var response = await _client.IndexAsync(newProduct, x => x.Index(indexName));
            //  "-" _client üzerinden yeni ürünü Elasticsearch dizinine (index) kaydetmek için asenkron bir işlem başlatır. Bu işlem sonucu response değişkenine atanır.

            // .Id(Guid.NewGuid().ToString()) ile Id yi kendimiz atıyoruz

            // products adlı tabloya kaydediyorum.
            // IndexAsync --> aslında create yerine geçer.
            

            //fast fail
            if (!response.IsSuccess()) return null;
            // IsSuccess() metodumuz var artık.

            newProduct.Id = response.Id;

            return newProduct;

        }



        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            var result = await _client.SearchAsync<Product>(
                s => s.Index(indexName).Query(q=>q.MatchAll()));



            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            //ıD NULL DÖNÜYORDU. Hit içinde verimz tutulur ve Id boş gözükür ama Hit içinde Source var ve onun içinde Id KAYDI GÖZÜKÜYOR. Id yi oradan alıp ekrana yansıttık. Alttaki Documents de Hits>Sourceden beslendiği için Id yi alacak. 

            return result.Documents.ToImmutableList();
            // ToImmutableList() --> System.Collections.Immutable adlı bir .NET Core kitaplığı tarafından sağlanan bir sınıftır ve koleksiyonun içeriğini değiştirmeye izin vermez. Yani, bir kez oluşturulduğunda, bu liste üzerinde yapılan herhangi bir işlem yeni bir liste oluşturur ve orijinal listeyi değiştirmez.

            // Ne kadar immutableLst ile calısırsak o kadar hataya kapalı olur.
        }


        // id'ye verimizi çekiyoruz. 
        public async Task<Product?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync<Product>(id, x => x.Index(indexName));


            if (!response.IsSuccess()) 
                return null;

            response.Source.Id = response.Id;
            return response.Source;
        }

        //id girip güncelliyoruz.
        public async Task<bool> UpdateAsync(ProductUpdateDto updateProduct)
        {
            var response = await _client.UpdateAsync<Product, ProductUpdateDto>(indexName,updateProduct.Id,x=>x.Doc(updateProduct));

            return response.IsSuccess();
        }


        // Hata Yönetimi içn ele alınmıştır
        //Silme İşlemi
        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var response = await _client.DeleteAsync<Product>(id,x => x.Index(indexName));

            return response;
        }

         
    }
}
