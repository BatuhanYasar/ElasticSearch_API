using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using System.Runtime.CompilerServices;


namespace ElasticSearch.API.Extensions
{
    public static class Elasticsearch
    {
        public static void AddElastic(this IServiceCollection services,IConfiguration configuration) 
        {
            // ElasticSearch.clent için yaptığımı yenilikleri yansıttık ve eski kodları sildik 

            var userName = configuration.GetSection("Elastic")["UserName"]; // Kullanıcı Adı

            var password = configuration.GetSection("Elastic")["Password"]; // Şifre (appsettings kısmından aldıklarımız)


            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!)).Authentication(new BasicAuthentication(userName!,password!));
            // Bu kod parçası, Elasticsearch sunucusuyla iletişim kurmak ve belirli işlemleri gerçekleştirmek için bu istemci yapılandırmasını kullanmanızı sağlar. 

            // configuration nesnesinin içinden Elasticsearch sunucusuna erişim sağlamak için kullanılacak URL'yi içeriyor. configuration.GetSection("Elastic")["Url"]! kodu, "Elastic" adlı bir bölüm içindeki "Url" anahtarını alır ve bu anahtarın değerini kullanır.

            // Oluşturulan settings örneği, Elasticsearch sunucusuna kimlik doğrulama yapmak için temel kimlik doğrulama (basic authentication) kullanacak şekilde yapılandırılır. Kimlik doğrulama için kullanıcı adı (userName) ve parola (password) parametreleri kullanılır.



            var client = new ElasticsearchClient(settings);
            // settings, Elasticsearch sunucusuna nasıl bağlanılacağı ve kimlik doğrulama bilgileri gibi yapılandırmaları içerir ve bu yapılandırmaları kullanarak bir Elasticsearch istemci örneği oluşturulur.

            services.AddSingleton(client);
            //  Elasticsearch istemci örneğini uygulamanızın hizmet sağlayıcısına (Dependency Injection Container) ekler. services bir IServiceCollection örneğini temsil eder ve genellikle ASP.NET Core uygulamalarının başlangıcında kullanılır. Bu, Elasticsearch istemcisini uygulamanızın hizmetlerinin bir parçası olarak kaydeder, böylece bu istemciyi uygulama içinde kullanabilirsiniz.
        }
    }
}
