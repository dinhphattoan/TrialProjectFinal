using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactApp.Server.Contracts.DTOs
{
#pragma warning disable CS8618
#pragma warning disable CS8625
    public class ResultDto<T>
    {
 
        public List<string> Message { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }
        public static ResultDto<T> CreateSuccess(string message = default, T data = default)
        {
            return new ResultDto<T>
            {
                Success = true,
                Message = string.IsNullOrEmpty(message) ? [] : [message],
                Data = data
            };
        }
        public static ResultDto<T> CreateFail(string message = default, T data = default)
        {
            return new ResultDto<T>
            {
                Success = false,
                Message = string.IsNullOrEmpty(message) ? [] : [message],
                Data = data
            };
        }
    }
}
