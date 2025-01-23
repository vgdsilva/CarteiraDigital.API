using CarteiraDigital.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteiraDigital.Application.Builders
{
    public class ApiResultBuilder
    {
        private readonly ApiResult _apiResult;

        public ApiResultBuilder()
        {
            _apiResult = new ApiResult();
        }

        public ApiResultBuilder SetMessage(string message)
        {
            _apiResult.Message = message;
            return this;
        }

        public ApiResultBuilder SetErrors(params object[] error)
        {
            if (error != null)
            {
                _apiResult.Errors = [error];
            }

            return this;
        }

        public ApiResultBuilder SetData(object data)
        {
            if (data != null)
            {
                _apiResult.Data = data;
            }

            return this;
        }

        public ApiResultBuilder SetSuccess(bool isSuccess)
        {
            _apiResult.Success = isSuccess;
            return this;
        }

        public ApiResult Build()
        {
            return _apiResult;
        }
    }
}
