using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MealsToGo.Models;
using MealsToGo.ViewModels;
using System.Web.Mvc;

namespace MealsToGo
{
    public class AutoMapperConfiguration
    {

        public static void Configure()
        {
            Mapper.CreateMap<LoginRegisterViewModel, LoginModel>();
            Mapper.CreateMap<LoginRegisterViewModel, RegisterModel>();
            Mapper.CreateMap<UserDetail, UserDetailViewModel>();
            Mapper.CreateMap<UserDetailViewModel, UserDetail>();
            Mapper.CreateMap<AddressViewModel, AddressList>();
            Mapper.CreateMap<AddressList, AddressViewModel>();
            Mapper.CreateMap<MealItem, MealItemViewModel>();
            Mapper.CreateMap<MealAd_Schedules, MealAdSchedule>();
            Mapper.CreateMap<MealAdSchedule, MealAd_Schedules>();
            // .ForMember(d => d.Status, m => m.MapFrom(p => p.Status == 1 ? true : false));
            Mapper.CreateMap<MealAd, MealAdViewModel>();

            Mapper.CreateMap<UserSetting, UserSettingsViewModel>().ForMember(d => d.ActivityType, m => m.MapFrom(p => p.LKUPActivityType.ActivityType))
                                                                  .ForMember(d => d.PrivacySetting, m => m.MapFrom(p => p.LKUPPrivacySetting.PrivacySettings))
                                                                  .ForMember(d => d.NotificationFrequency, m => m.MapFrom(p => p.NotificationFrequency.Description))
                                                                   .ForMember(d => d.ReceiveEmailNotification, m => m.AddFormatter<VipFormatter>())
                                                                     .ForMember(d => d.ReceiveMobileTextNotification, m => m.AddFormatter<VipFormatter>());
            Mapper.CreateMap<UserSettingsViewModel, UserSetting>();

            Mapper.CreateMap<MealItems_AllergenicFoods, Allergen>()
                  .ForMember(d => d.AllergenID, opt => opt.MapFrom(s => s.AllergenicFoodID));


            Mapper.CreateMap<MealItemViewModel, MealItem>().ForMember(d => d.MealTypeID, m => m.MapFrom(p => p.MealTypeDD.SelectedMealType))
                                                             .ForMember(d => d.CusineTypeID, m => m.MapFrom(p => p.CusineTypeDD.SelectedCuisine))
                                                             .ForMember(d => d.DietTypeID, m => m.MapFrom(p => p.DietTypeDD.SelectedDietType))
                //.ForMember(d => d.Status, m => m.MapFrom(p => p.Status ? 1 : 0))
                                                              .AfterMap((s, d) =>
                                                                          {
                                                                              foreach (var mealaller in d.MealItems_AllergenicFoods)

                                                                                  mealaller.MealItemID = s.MealItemId;


                                                                          });

            Mapper.CreateMap<MealAdViewModel, MealAd>().ForMember(d => d.MealItemID, m => m.MapFrom(p => p.MealItemsDD.SelectedMealItem))
                                                            .ForMember(d => d.AvailabilityTypeID, m => m.MapFrom(p => p.AvailabilityTypeDD.SelectedAvailabilityType))
                                                            //.ForMember(d => d.MealItem.MealItemName, m => m.MapFrom(p => p.MealItemName))
                                                            .AfterMap((s, d) =>
                                                            {
                                                                foreach (var mealamealadpayment in d.MealAds_PaymentOptions)

                                                                    mealamealadpayment.MealAdID = s.MealAdID;


                                                            })

                                                             .AfterMap((s, d) =>
                                                             {
                                                                 foreach (var mealaddel in d.MealAds_DeliveryMethods)

                                                                     mealaddel.MealAdID = s.MealAdID;


                                                             }).AfterMap((s, d) =>
                                                             {
                                                                 if (d.MealItem != null)
                                                                     s.MealItemName = d.MealItem.MealItemName;


                                                             });



            Mapper.CreateMap<Allergen, MealItems_AllergenicFoods>()
                  .ForMember(d => d.AllergenicFoodID, opt => opt.MapFrom(s => s.AllergenID));

        }



        public class VipFormatter : IValueFormatter
        {
            public string FormatValue(ResolutionContext context)
            {


                if (context.SourceValue.ToString() == "1")
                {
                    return "Yes";
                }


                return "No";

            }
        }

    }
}