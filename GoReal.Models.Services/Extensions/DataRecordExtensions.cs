using GoReal.Models.Entities;
using System;
using System.Data;

namespace GoReal.Models.Services.Extensions
{
    public static class DataRecordExtensions
    {
        internal static User ToUser(this IDataRecord Dr)
        {
            return new User() { 
                UserId = (int)Dr["UserId"],
                GoTag = (string)Dr["GoTag"],
                LastName = (string)Dr["LastName"],
                FirstName = (string)Dr["FirstName"],
                Email = (string)Dr["Email"],
            };
        }

        internal static Role ToRole(this IDataRecord Dr)
        {
            return (Role)Enum.Parse(typeof(Role), (string)Dr["RoleName"]);
        }
    }
}
