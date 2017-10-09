
using Common.Enums;
using System.Collections.Generic;

namespace Common
{
    public static class Constants
    {
        public static Dictionary<ResultType, string> ErorsDict = new Dictionary<ResultType, string>()
        {
             {ResultType.Error,"There seems to be an error" },
             {ResultType.AlreadyExists,"It seems entity already exists" },
             {ResultType.DoesntExists,"It seems entity doesnt exists" },
             {ResultType.Fail,"It seems the operation failed" },
             {ResultType.Success,"Operation successful" },
             {ResultType.AlreadyDeleted,"It seems this entity is already deleted" },
             {ResultType.NoChanges,"There are no changes made" },

        };

        public const string InvalidUrl = "This is not a valid Url";
        public const string UserImgUrl = "UserImgUrl";
        public const string UserId = "UserId";
        public const string EmptyRequest="Comments can`t be empty";
    }
}
