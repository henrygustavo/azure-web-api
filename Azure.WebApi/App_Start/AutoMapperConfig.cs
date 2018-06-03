namespace Azure.WebApi.App_Start
{
    using Entity;
    using Models;
    using AutoMapper;
    using Microsoft.ProjectOxford.Face.Contract;
    using System.Linq;

    public static class AutoMapperConfig
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Item, ItemModel>().
                    ForMember(dest => dest.Colors,
                    m => m.MapFrom(src => string.Join(",", src.Colors)));

                cfg.CreateMap<ItemModel, Item>().
                    ForMember(dest => dest.Colors,
                    source => source.MapFrom(s => s.Colors.Split(',')));

                cfg.CreateMap<Face, FaceAttributeModel>().
                   ForMember(dest => dest.FaceId, m => m.MapFrom(src => src.FaceId)).
                   ForMember(dest => dest.Age, m => m.MapFrom(src => src.FaceAttributes.Age)).
                   ForMember(dest => dest.Gender, m => m.MapFrom(src => src.FaceAttributes.Gender)).
                   ForMember(dest => dest.Smile, m => m.MapFrom(src => src.FaceAttributes.Smile)).
                   ForMember(dest => dest.Glasses, m => m.MapFrom(src => src.FaceAttributes.Glasses));

                cfg.CreateMap<ImageRecognition, ImageRecognitionModel>()
                    .ForMember(dest => dest.ImageId, m => m.MapFrom(src => src.Id))
                    .ForMember(dest => dest.ImagePath, m => m.MapFrom(src => src.ImagePath))
                    .ForMember(dest => dest.Description, m => m.MapFrom(src => GetDescriptionImage(src.AnalysisResult)));

            });

            return config;
        }

        private static string GetDescriptionImage(Microsoft.ProjectOxford.Vision.Contract.AnalysisResult analysisResult)
        {
            string message = analysisResult?.Description?.Captions.FirstOrDefault()?.Text;

            return string.IsNullOrEmpty(message) ? $"Couldn't find a caption for this one"
                                                    : $"I think it\'s {message}";
        }
    }
}