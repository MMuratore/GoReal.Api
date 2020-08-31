using GoReal.Models.Entities;
using System;
using System.Data;

namespace GoReal.Models.Services.Extensions
{
    public static class DataRecordExtensions
    {
        internal static User ToUser(this IDataRecord Dr)
        {
            int test = (Dr.FieldCount);
            return new User() {
                UserId = (int)Dr["UserId"],
                GoTag = (string)Dr["GoTag"],
                LastName = (string)Dr["LastName"],
                FirstName = (string)Dr["FirstName"],
                Email = (string)Dr["Email"],
                isActive = (bool)Dr["isActive"],
                isBan = (bool)Dr["isBan"],
                Roles = (Dr.FieldCount) == 9 ? (int)Dr["Role"] : 0
            };
        }
    }
}
