using Common.Contracts;
using Common.Enums;

namespace Common
{
    public class Result : IResult
    {
        private string errorMsg;
        public Result()
        {
            this.ErrorMsg = string.Empty;
            this.ResulType = ResultType.Success;
        }

        public Result(ResultType type)
        {
            this.ErrorMsg = string.Empty; 
            this.ResulType = type;
        }
        public Result(string errorMsg, ResultType type)
        {
            this.ErrorMsg = errorMsg;
            this.ResulType = type;
        }

        public string ErrorMsg
        {
            get
            {
                if (string.IsNullOrWhiteSpace(errorMsg))
                {
                    return Constants.ErorsDict[this.ResulType];
                }
                return this.errorMsg;
            }

            set
            {
                this.errorMsg = value;

            }
        }
        public ResultType ResulType { get; set; }
    }
}
