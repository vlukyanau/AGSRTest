using System;
using System.Collections.Generic;
using System.Net;

using Logic.Models;


namespace Logic
{
    public interface IResult
    {
        object Data { get; }
        HttpStatusCode Code { get; }
    }

    public sealed class Result : IResult
    {
        #region Operators
        public static implicit operator Result(HttpStatusCode code)
        {
            return Result.New(code);
        }
        public static implicit operator HttpStatusCode(Result result)
        {
            return result.Code;
        }
        public static implicit operator int(Result result)
        {
            return (int)result.Code;
        }
        #endregion

        #region Classes
        private class MessageInfo
        {
            #region Static
            public static MessageInfo New(string message)
            {
                MessageInfo info = new MessageInfo();
                info.Message = message;

                return info;
            }
            #endregion

            #region Properties
            public string Message { get; set; }
            #endregion
        }
        #endregion

        #region Static
        public static Result New(Exception exception)
        {
            Result result = new Result();
            result.Data = MessageInfo.New(exception.Message);
            result.Code = HttpStatusCode.BadRequest;

            return result;
        }
        public static Result New(HttpStatusCode code)
        {
            Result result = new Result();
            result.Data = default;
            result.Code = code;

            return result;
        }
        public static Result New(IReadOnlyList<IEntry> data, HttpStatusCode code = HttpStatusCode.OK)
        {
            Result result = new Result();
            result.Data = data;
            result.Code = code;

            return result;
        }
        public static Result New(IEntry data, HttpStatusCode code = HttpStatusCode.OK)
        {
            Result result = new Result();
            result.Data = data;
            result.Code = code;

            return result;
        }

        public static readonly Result Ok = HttpStatusCode.OK;
        public static readonly Result Created = HttpStatusCode.Created;
        public static readonly Result NoContent = HttpStatusCode.NoContent;
        public static readonly Result NotFound = HttpStatusCode.NotFound;
        #endregion

        #region Constructors
        private Result()
        {
        }
        #endregion

        #region IResult
        public object Data { get; private set; }
        public HttpStatusCode Code { get; private set; }
        #endregion
    }
}
