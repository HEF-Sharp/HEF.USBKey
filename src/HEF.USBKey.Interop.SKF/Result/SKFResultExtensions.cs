using System;

namespace HEF.USBKey.Interop.SKF
{
    public static class SKFResultExtensions
    {
        public static bool IsSuccess(this SKFResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            return result.Code == SKFResultCodes.SAR_OK;
        }

        public static SKFResult<TData> ToResult<TData>(this SKFResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            if (result is SKFResult<TData> resultWithData)
                return resultWithData;

            return new SKFResult<TData>
            {
                Code = result.Code
            };
        }

        public static SKFResult<TData> WithData<TData>(this SKFResult<TData> result, TData resultData)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            if (resultData == null)
                throw new ArgumentNullException(nameof(resultData));

            result.Data = resultData;

            return result;
        }
        
        public static SKFResult<TData> WithData<TData>(this SKFResult result, TData resultData)
        {
            return result.ToResult<TData>().WithData(resultData);
        }
    }
}
