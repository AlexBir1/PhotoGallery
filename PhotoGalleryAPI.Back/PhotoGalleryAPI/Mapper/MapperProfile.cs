using AutoMapper;
using PhotoGalleryAPI.DAL.Entities;
using PhotoGalleryAPI.Shared.DTOs;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Album mappings
        CreateMap<Album, AlbumDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.CreatedByPersonId, opt => opt.MapFrom(src => src.CreatedByPersonId.ToString()))
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos));

        CreateMap<AlbumDTO, Album>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Id) ? Guid.NewGuid() : Guid.Parse(src.Id)))
            .ForMember(dest => dest.CreatedByPersonId, opt => opt.MapFrom(src => Guid.Parse(src.CreatedByPersonId)))
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos));

        // Photo mappings
        CreateMap<Photo, PhotoDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.AlbumId, opt => opt.MapFrom(src => src.AlbumId.ToString()));

        CreateMap<PhotoDTO, Photo>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Id) ? Guid.NewGuid() : Guid.Parse(src.Id)))
            .ForMember(dest => dest.AlbumId, opt => opt.MapFrom(src => Guid.Parse(src.AlbumId)));

        // Person mappings
        CreateMap<Person, PersonDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username)) // Map Username
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId.ToString()));

        CreateMap<PersonDTO, Person>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Id) ? Guid.NewGuid() : Guid.Parse(src.Id)))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username)) // Map Username
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => Guid.Parse(src.RoleId)));

        // Role mappings
        CreateMap<Role, RoleDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

        CreateMap<RoleDTO, Role>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Id) ? Guid.NewGuid() : Guid.Parse(src.Id)));

        CreateMap<Like, LikeDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.PhotoId.ToString()))
            .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId.ToString()));

        CreateMap<LikeDTO, Like>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Id) ? Guid.NewGuid() : Guid.Parse(src.Id)))
            .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => Guid.Parse(src.PhotoId)))
            .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => Guid.Parse(src.PersonId)));
    }
}
