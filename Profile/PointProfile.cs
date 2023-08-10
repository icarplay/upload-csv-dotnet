using UploadCsv.Data.Dto;
using UploadCsv.Models;
using AutoMapper;

namespace UploadCsv.Profiles;

public class PointProfile : Profile
{
    public PointProfile()
    {
        CreateMap<Point, ReadPointDto>();
    }
}