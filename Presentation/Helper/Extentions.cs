
using ApplicationCore.DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Presentation
{
    public static class Extentions
    {
        public static string GetControllerName(this ActionContext actionContext)
        {
            return (actionContext.ActionDescriptor as ControllerActionDescriptor).ControllerName;
        }

        public static ModelStateDictionary TranslateErrors(this ModelStateDictionary modelState, Language language)
        {
            List<ModelStateError> listOfErrors = new();
            foreach (var item in modelState)
            {
                var field = item.Key;
                var state = item.Value;
                foreach (var error in state.Errors)
                {
                    listOfErrors.Add(new ModelStateError { Field = field, Message = error.ErrorMessage + "-Translated..." });
                }
            }
            foreach (var error in listOfErrors)
            {
                modelState.Remove(error.Field);
                modelState.AddModelError(error.Field, error.Message);
            }
            return modelState;
        }
    }
}
