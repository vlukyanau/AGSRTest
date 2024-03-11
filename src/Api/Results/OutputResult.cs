using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Logic;


namespace Api.Results
{
    public class OutputResult : JsonResult
    {
        #region Static
        private static JsonSerializerOptions GetSettings()
        {
            JsonSerializerOptions settings = new JsonSerializerOptions();
            settings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            settings.Converters.Add(new JsonStringEnumConverter());

            return settings;
        }
        #endregion

        #region Constructors
        public OutputResult(IResult result)
            : base(result.Data, OutputResult.GetSettings())
        {
            this.StatusCode = (int)result.Code;
        }
        #endregion

        #region Overriding
        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (this.StatusCode == 204)
                context.HttpContext.Response.Body = Stream.Null;

            return base.ExecuteResultAsync(context);
        }

        public override void ExecuteResult(ActionContext context)
        {
            if (this.StatusCode == 204)
                context.HttpContext.Response.Body = Stream.Null;

            context.HttpContext.Response.StatusCode = (int)this.StatusCode;

            base.ExecuteResult(context);
        }
        #endregion
    }
}
