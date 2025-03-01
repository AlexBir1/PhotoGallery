using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.BaseResponse.Responses
{
    public class DbResponse<T> : IResponse<T> where T : class
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public DbResponse(bool success, T entity, IEnumerable<string> errors = null)
        {
            Success = success;
            Data = entity;
            Errors = errors;
        }

        public static DbResponse<T> SuccessResponse(T data)
        {
            return new DbResponse<T>(true, data);
        }

        public static DbResponse<T> FailureResponse(string error)
        {
            return new DbResponse<T>(false, null, new List<string> { error });
        }

        public static DbResponse<T> FailureResponse(IEnumerable<string> errors)
        {
            return new DbResponse<T>(false, null, errors);
        }
    }
}
