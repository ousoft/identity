using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oyang.Identity.WebApi.Models
{

    public class ResultModel
    {
        public ResultModel()
        {
            IsSuccess = true;
            Message = "操作成功";
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ResultModel<T> : ResultModel
    {
        public ResultModel() : base()
        {

        }
        public T Data { get; set; }
    }
}
