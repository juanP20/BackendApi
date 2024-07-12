using AutoMapper;
using BackendAPI.Dtos;
using BackendAPI.Models;
using System.Globalization;

namespace BackendAPI.Utilidades
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            #region Departamento
            CreateMap<Departamento, DepartamentoDto>();
            #endregion

            #region Empleado
            CreateMap<Empleado, EmpleadoDto>()
                .ForMember(dest => dest.NombreDepartamento, opt => opt.MapFrom(src => src.IdDepartamentoNavigation.Nombre))
                .ForMember(dest => dest.FechaContrato, opt => opt.MapFrom(src => src.FechaContrato.Value.ToString("dd/MM/yyyy")));

            CreateMap<EmpleadoDto, Empleado>()
                .ForMember(dest => dest.IdDepartamentoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.FechaContrato, opt => opt.MapFrom(src => DateTime.ParseExact(src.FechaContrato,"dd/MM/yyyy",CultureInfo.InvariantCulture)));
            #endregion
        }
    }
}
