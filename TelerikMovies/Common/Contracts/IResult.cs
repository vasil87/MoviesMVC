using Common.Enums;

namespace Common.Contracts
{
    public interface IResult
    {
        string ErrorMsg { get; set; }
        ResultType ResulType { get; set; }
    }
}