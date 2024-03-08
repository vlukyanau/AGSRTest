using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;


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
        public OutputResult(object result)
            : base(result, OutputResult.GetSettings())
        {
        }
        public OutputResult(HttpStatusCode code, object result)
            : base(result, OutputResult.GetSettings())
        {
            this.StatusCode = (int)code;
        }
        #endregion
    }
}
