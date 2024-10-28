using Application.DTOs;
using Application.Use_Cases.Commands;
using Domain.Entities;
using System;
using System.Collections.Generic;

using AutoMapper;

namespace Application.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ToDoTask, ToDoTaskDto>().ReverseMap();
            CreateMap<CreateToDoTaskCommand, ToDoTask>().ReverseMap();
            CreateMap<UpdateToDoTaskCommand, ToDoTask>().ReverseMap();
        }
    }
}