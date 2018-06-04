namespace Azure.WebApi
{
    using System.Web.Http;
    using App_Start;
    using Unity;
    using Unity.Injection;
    using Repository.Implementations;
    using Repository.Interfaces;
    using Service.Implementations;
    using Service.Interfaces;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            RegisterIOC(config);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void RegisterIOC(HttpConfiguration config)
        {
            var container = new UnityContainer();

            container.RegisterType<ISecretKeyProvider, SecretKeyProvider>
                (new InjectionConstructor(AzureKeys.KeyVaultClientId, AzureKeys.KeyVaultSecret, AzureKeys.KeyVaultBaseUrl));

            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<IImageFaceRepository, ImageFaceRepository>();
            container.RegisterType<IImageRecognitionRepository, ImageRecognitionRepository>();
            container.RegisterType<IDataBaseManager, DataBaseManager>(new InjectionConstructor(typeof(ISecretKeyProvider),
                AzureKeys.CosmoDBVaultName));
            container.RegisterSingleton<ICognitiveService, CognitiveService>(new InjectionConstructor(typeof(ISecretKeyProvider),
                AzureKeys.CognityServicesVaultName));

            container.RegisterSingleton<IStorageService, StorageService>(new InjectionConstructor(typeof(ISecretKeyProvider), AzureKeys.StorageVaultName));

            container.RegisterType<ILoggerService, LoggerService>(new InjectionConstructor(typeof(ISecretKeyProvider), AzureKeys.ApplicatonInsightKey));

            config.DependencyResolver = new UnityResolver(container);

            var mapper = AutoMapperConfig.InitializeAutoMapper().CreateMapper();
            container.RegisterInstance(mapper);

        }
    }
}
