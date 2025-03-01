using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.BaseResponse
{
    public interface IResponse<T> where T : class
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
