using GoReal.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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
                isAdmin = (bool)Dr["isAdmin"],
                isActive = (bool)Dr["isActive"],
            };
        }
    }
}
