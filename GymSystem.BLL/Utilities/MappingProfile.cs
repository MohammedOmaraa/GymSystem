using AutoMapper;
using GymSystem.BLL.ViewModels.SessionsViewModels;
using GymSystem.DAL.Entities;

namespace GymSystem.BLL.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapSession();
        }

        private void MapSession()
        {
            //CreateMap<Session, SessionViewModel>(); // from Session to SessionViewModel
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest=> dest.CategoryName, opt=> opt.MapFrom(src=> src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore())
                .ReverseMap(); // From Session to SessionViewModel and from SessionViewModel to Session

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Trainer, TrainerSelectViewModel>();

            CreateMap<Category, CategorySelectViewModel>();

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

        }
    }
}
