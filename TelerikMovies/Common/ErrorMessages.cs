
using Common.Enums;
using System.Collections.Generic;

namespace Common
{
    public static class ErrorMessages
    {
        public static Dictionary<ResultType, string> ErorsDict = new Dictionary<ResultType, string>()
        {
             {ResultType.Error,"There seems to be an error" },
             {ResultType.AlreadyExists,"It seems entity already exists" },
             {ResultType.DoesntExists,"It seems entity doesnt exists" },
             {ResultType.Fail,"It seems the operation failed" },
             {ResultType.Success,"Operation successful" },
             {ResultType.AlreadyDeleted,"It seems this entity is already deleted" },
             
        };
    }
}
