using AutoMapper;
using RecipesServer.DTOs;
using RecipesServer.DTOs.Category;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Helpers
{
	public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>();
             //  .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
            //       src.Photos.FirstOrDefault(x => x.IsMain).Url))
              // .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<RecipePhotos, RecipePhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUser>();

            CreateMap<Category, CategoryUpdateDTO>()
                .ForMember(des => des.Name, src => src.MapFrom(c => c.Name)).ReverseMap();
            CreateMap<Category, AllCategoriesDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>();

            CreateMap<RecipeIngredients, IngredientDTO>()
                .ForMember(dest=>dest.Amount,src=>src.MapFrom(r=>r.Amount))
                .ForMember(dest=>dest.Name,src=>src.MapFrom(r=>r.Ingredient.Name));
            CreateMap<RecipeComments, CommentDTO>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(u => u.Comment.User.FirstName + " " + u.Comment.User.LastName))
                .ForMember(dest => dest.DateCommentIsPosted, src => src.MapFrom(u => u.Comment.DateCommentIsPosted))
                .ForMember(dest => dest.Message, src => src.MapFrom(u => u.Comment.Message));
            CreateMap<RegisterDTO, AppUser>();
             var c = CreateMap<Recipe, RecipeDTO>()
                 .ForMember(dest=>dest.ChefName,opt=>opt.MapFrom(u=>u.User.FirstName+" "+u.User.LastName));
             
        }
    }
}
