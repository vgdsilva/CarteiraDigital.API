using AutoMapper;
using CarteiraDigital.Domain.DTOs;
using CarteiraDigital.Domain.Entities;

namespace CarteiraDigital.Application.Services;

public static class MapperService
{
    private static IMapper iMapper = null;

    public static IMapper CreateMapper()
    {
        if (iMapper != null)
            return iMapper;

        MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TransactionDTO, Transaction>();
        });

        iMapper = mapperConfiguration.CreateMapper();

        return iMapper;
    }
}
