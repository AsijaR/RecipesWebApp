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
using RecipesServer.DTOs.Member;

namespace RecipesServer.Helpers
{
	public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>()
              .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                   src.UserPhoto.Url));
            CreateMap<AppUser, UserInfoDTO>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(u => u.FirstName + " " + u.LastName))
                .ForMember(d => d.AppUserId, opt => opt.MapFrom(u => u.Id))
                .ForMember(d => d.Username, opt => opt.MapFrom(u => u.UserName)).ReverseMap();
               

            CreateMap<MemberUpdateProfileDTO, AppUser>();
            CreateMap<MemberUpdateShippingPriceDTO, AppUser>();
            // .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<UserPhoto, UserPhotoDTO>().ReverseMap();

            CreateMap<RecipePhotos, RecipePhotoDTO>();

            CreateMap<Category, CategoryUpdateDTO>()
                .ForMember(des => des.Name, src => src.MapFrom(c => c.Name)).ReverseMap();
            CreateMap<Category, AllCategoriesDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>();
           
            //CreateMap<Recipe, RecipeBasicInfoDTO>();

            CreateMap<RecipeBookmarks, RecipeBasicInfoDTO>()
                .ForMember(dest => dest.RecipeId, src => src.MapFrom(b => b.RecipeId))
                .ForMember(dest=>dest.HeaderUrl,src=>src.MapFrom(r=>r.Recipe.RecipePhotos.FirstOrDefault(p => p.IsMain == true).Url))
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
                .ForMember(dest=>dest.UserPhotoUrl,src=>src.MapFrom(u=>u.Comment.User.UserPhoto.Url))
                .ForMember(dest => dest.DateCommentIsPosted, src => src.MapFrom(u => u.Comment.DateCommentIsPosted))
                .ForMember(dest => dest.Message, src => src.MapFrom(u => u.Comment.Message));
            CreateMap<AddCommentDTO, Comment>();

            CreateMap<RecipeOrders, GetOrdersDTO>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(r => r.Order.FullName))
                .ForMember(dest => dest.Address, src => src.MapFrom(r => r.Order.Address))
                .ForMember(dest => dest.ApprovalStatus, src => src.MapFrom(r => r.ApprovalStatus))
                .ForMember(dest => dest.City, src => src.MapFrom(r => r.Order.City))
                .ForMember(dest => dest.DateMealShouldBeShipped, src => src.MapFrom(r => r.Order.DateMealShouldBeShipped))
                .ForMember(dest => dest.NoteToChef, src => src.MapFrom(r => r.Order.NoteToChef))
                .ForMember(dest => dest.OrderId, src => src.MapFrom(r => r.OrderId))
                .ForMember(dest => dest.Price, src => src.MapFrom(r => r.Recipe.Price))
                .ForMember(dest => dest.RecipeId, src => src.MapFrom(r => r.Recipe.RecipeId))
                .ForMember(dest => dest.RecipeTitle, src => src.MapFrom(r => r.Recipe.Title))
                .ForMember(dest => dest.ServingNumber, src => src.MapFrom(r => r.Order.ServingNumber))
                .ForMember(dest => dest.ShippingPrice, src => src.MapFrom(r => r.Chef.ShippingPrice))
                .ForMember(dest => dest.State, src => src.MapFrom(r => r.Order.State))
                .ForMember(dest => dest.Total, src => src.MapFrom(r => r.Order.Total))
                .ForMember(dest => dest.Zip, src => src.MapFrom(r => r.Order.Zip))
                .ReverseMap();
           

            CreateMap<OrderStatusDTO, RecipeOrders>()
                .ForMember(dest => dest.ApprovalStatus, src => src.MapFrom(r => r.Status)).ReverseMap();

            CreateMap<RecipeOrders, MakeOrderDTO>()
                .ForMember(dest => dest.Address, src => src.MapFrom(r => r.Order.Address))
                .ForMember(dest => dest.City, src => src.MapFrom(r => r.Order.City))
                .ForMember(dest => dest.DateMealShouldBeShipped, src => src.MapFrom(r => r.Order.DateMealShouldBeShipped))
                .ForMember(dest => dest.FullName, src => src.MapFrom(r => r.Order.FullName))
                .ForMember(dest => dest.NoteToChef, src => src.MapFrom(r => r.Order.NoteToChef))
                .ForMember(dest => dest.RecipeId, src => src.MapFrom(r => r.RecipeId))
                .ForMember(dest => dest.ServingNumber, src => src.MapFrom(r => r.Order.ServingNumber))
                .ForMember(dest => dest.State, src => src.MapFrom(r => r.Order.State))
                .ForMember(dest => dest.Zip, src => src.MapFrom(r => r.Order.Zip))
                .ForMember(dest => dest.Total, src => src.MapFrom(r => r.Order.Total))
                .ReverseMap();

            CreateMap<RegisterDTO, AppUser>();

            CreateMap<Recipe, RecipeBasicInfoDTO>()
                .ForMember(dest=>dest.HeaderUrl,src=>src.MapFrom(r=>r.RecipePhotos.FirstOrDefault(p=>p.IsMain==true).Url))
                .ReverseMap();
            CreateMap<Recipe, RecipeDeleteDTO>().ReverseMap();
            CreateMap<Recipe, RecipeUpdateDTO>().ReverseMap();
            CreateMap<Recipe, NewRecipeDTO>().ReverseMap();
            CreateMap<RecipeIngDTO, Recipe>();
                //.ForMember(dest => dest.Recipe, src => src.MapFrom(r => r.Recipe))
                //.ForMember(dest => dest.Ingredient, src => src.MapFrom(r => r.Ingredients));

            CreateMap<Recipe, RecipeDTO>()
                 .ForMember(dest=>dest.ChefName,opt=>opt.MapFrom(u=>u.User.FirstName+" "+u.User.LastName))
                 .ForMember(dest=>dest.ChefPhoto,opt=>opt.MapFrom(u=>u.User.UserPhoto.Url))
                 .ForMember(dest=>dest.ShippingPrice,opt=>opt.MapFrom(u=>u.User.ShippingPrice));
             
        }
    }
}
