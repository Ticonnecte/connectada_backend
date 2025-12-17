using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace CidConnectada.Website.Code.ModelBinders
{
    public class JsonDateModelBinder : DefaultModelBinder
    {
        public const string JsonDatePattern = @"/date\(([0-9]+)\)/";
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object result = null;
            if (bindingContext.ValueProvider.GetValue(bindingContext.ModelName) != null)
            {
                string attemptedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;

                if (!Regex.IsMatch(attemptedValue, JsonDatePattern, RegexOptions.IgnoreCase))
                    return base.BindModel(controllerContext, bindingContext);

                long miliseconds = long.Parse(Regex.Match(attemptedValue, JsonDatePattern, RegexOptions.IgnoreCase).Groups[1].Value);

                DateTime epoc = new DateTime(1970, 1, 1);
                result = epoc.AddMilliseconds(miliseconds);
            }

            return result;
        }
    }
}