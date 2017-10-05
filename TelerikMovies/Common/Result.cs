using Common.Contracts;
using Common.Enums;

namespace Common
{
    public class Result : IResult
    {
        public Result()
        {
            this.ErrorMsg = string.Empty;
            this.ResulType = ResultType.Success;
        }
        public Result(string errorMsg, ResultType type)
        {
            this.ErrorMsg = errorMsg;
            this.ResulType = type;
        }

        public string ErrorMsg { get; set; }
        public ResultType ResulType { get; set; }
    }
}
