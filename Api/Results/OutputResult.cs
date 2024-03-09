using System.Text.Json;
using System.Text.Json.Serialization;

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
    }
}
