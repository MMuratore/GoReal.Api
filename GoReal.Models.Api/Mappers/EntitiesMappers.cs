using System;
using System.Collections.Generic;
using System.Text;
using D = GoReal.Models.Entities;

namespace GoReal.Models.Api.Mappers
{
    public static class EntitiesMappers
    {
        internal static User ToClient(this D.User entity)
        {
            return new User() { 
                UserId = entity.UserId,
                GoTag = entity.GoTag,
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                Password = entity.Password,
                Email = entity.Email,
                isAdmin = entity.isAdmin,
                isActive = entity.isActive
            };
        }
        internal static D.User ToDal(this User entity)
        {
            return new D.User()
            {
                UserId = entity.UserId,
                GoTag = entity.GoTag,
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                Password = entity.Password,
                Email = entity.Email,
                isAdmin = entity.isAdmin,
                isActive = entity.isActive
            };
        }
    }
}
