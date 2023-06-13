using System;
using System.Collections.Generic;

namespace FindMe.Error
{
    public class Result<T>
    {
        public T Data { get; set; }

        public ErrorData Error { get; set; }

        public static Result<T> GetSuccess(T transferredData)
        {

            if (object.Equals(transferredData, default(T)) && typeof(T) != typeof(bool)) // default(bool) is false
            {
                throw new ArgumentNullException();
            }
            else
            {
                var transferredDataToReturn = new Result<T>()
                {
                    Data = transferredData,
                };

                return transferredDataToReturn;
            }
        }

        public static Result<T> GetError(ErrorCode errorCode, string errorMessage)
        {
            var newResult = new Result<T>
            {
                Error = new ErrorData
                {
                    Code = errorCode,
                    Message = errorMessage
                },
            };

            return newResult;
        }

        public static Result<T> GetError(ErrorCode errorCode, IEnumerable<string> errorMessages)
        {
            var newResult = new Result<T>
            {
                Error = new ErrorData
                {
                    Code = errorCode,
                    Message = string.Join("; ", errorMessages)
                },
            };

            return newResult;
        }
    }
}
