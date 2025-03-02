using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhotoGalleryAPI.BaseResponse.Responses
{
    public class APIResponse<T> : IResponse<T> where T : class
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public int ItemsPerPage { get; set; }
        public int SelectedPage { get; set; }
        public int ItemsCount { get; set; }

        public APIResponse(bool success, T data, IEnumerable<string> errors = null, int itemsPerPage = 1, int selectedPage = 1, int itemsCount = 1)
        {
            Success = success;
            Data = data;
            Errors = errors ?? new List<string>();

            ItemsPerPage = itemsPerPage;
            SelectedPage = selectedPage;
            ItemsCount = itemsCount;
        }

        public static APIResponse<T> SuccessResponse(T data)
        {
            return new APIResponse<T>(true, data);
        }

        public static APIResponse<T> SuccessPagedResponse(T data, int itemsPerPage = 1, int selectedPage = 1, int itemsCount = 1)
        {
            return new APIResponse<T>(true, data, null, itemsPerPage, selectedPage, itemsCount);
        }

        public static APIResponse<T> FailureResponse(string error)
        {
            return new APIResponse<T>(false, null, new List<string> { error });
        }

        public static APIResponse<T> FailureResponse(IEnumerable<string> errors)
        {
            return new APIResponse<T>(false, null, errors);
        }
    }
}
