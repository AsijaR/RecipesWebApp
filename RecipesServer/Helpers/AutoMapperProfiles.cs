using AutoMapper;
using RecipesServer.DTOs;
using RecipesServer.DTOs.Order;
using RecipesServer.DTOs.Category;
using RecipesServer.DTOs.Recipe;
using RecipesServer.DTOs.Comment;
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
           
            //CreateMap<Recipe, RecipeBasicInfoDTO>();

            CreateMap<RecipeBookmarks, RecipeBasicInfoDTO>()
                .ForMember(dest => dest.RecipeId, src => src.MapFrom(b => b.RecipeId))
                .ForMember(dest => dest.Title, src => src.MapFrom(r => r.Recipe.Title))
                .ForMember(dest => dest.Complexity, src => src.MapFrom(r => r.Recipe.Complexity))
                .ForMember(dest => dest.TimeNeededToPrepare, src => src.MapFrom(r => r.Recipe.TimeNeededToPrepare))
                .ForMember(dest => dest.ServingNumber, src => src.MapFrom(r => r.Recipe.ServingNumber));
            CreateMap<Bookmark, BookmarkDTO>();

            CreateMap<RecipeIngredients, IngredientDTO>()
                .ForMember(dest=>dest.Amount,src=>src.MapFrom(r=>r.Amount))
                .ForMember(dest=>dest.Name,src=>src.MapFrom(r=>r.Ingredient.Name)).ReverseMap();

            CreateMap<RecipeComments, CommentDTO>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(u => u.Comment.User.FirstName + " " + u.Comment.User.LastName))
                .ForMember(dest => dest.DateCommentIsPosted, src => src.MapFrom(u => u.Comment.DateCommentIsPosted))
                .ForMember(dest => dest.Message, src => src.MapFrom(u => u.Comment.Message));
            CreateMap<AddCommentDTO, Comment>();

           // CreateMap<RecipeOrders, OrderDTO>();
            //    .ForMember(dest=>dest.Address,src=>src.MapFrom(r=>r.Address))
            //    .ForMember(dest=>dest.City,src=>src.MapFrom(r=>r.City))
            //    .ForMember(dest=>dest.Address,src=>src.MapFrom(r=>r.Address))
            //    .ForMember(dest=>dest.Address,src=>src.MapFrom(r=>r.Address))
            //    .ForMember(dest=>dest.Address,src=>src.MapFrom(r=>r.Address))
            //.ForMember(dest=>dest.Title,src=>src.MapFrom(r=>r.Order.Recipes.))

            CreateMap<OrderStatusDTO, RecipeOrders>()
                .ForMember(dest => dest.ApprovalStatus, src => src.MapFrom(r => r.Status));
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<RegisterDTO, AppUser>();

            CreateMap<Recipe, RecipeBasicInfoDTO>().ReverseMap();
            CreateMap<Recipe, RecipeDeleteDTO>().ReverseMap();
            CreateMap<Recipe, RecipeUpdateDTO>().ReverseMap();
            CreateMap<Recipe, NewRecipeDTO>().ReverseMap();
            CreateMap<RecipeIngDTO, RecipeIngredients>()
                .ForMember(dest => dest.Recipe, src => src.MapFrom(r => r.Recipe))
                .ForMember(dest => dest.Ingredient, src => src.MapFrom(r => r.Ingredients));

            CreateMap<Recipe, RecipeDTO>()
                 .ForMember(dest=>dest.ChefName,opt=>opt.MapFrom(u=>u.User.FirstName+" "+u.User.LastName));
             
        }
    }
}
