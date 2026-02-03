using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Repositories.Interfaces
{
    public interface IRoleRepo
    {
        /// <summary>
        /// Posts a new role to the table
        /// </summary>
        /// <param name="newRole">The new role that needs to be posted</param>
        /// <returns>Role that was added, null if it already exists, and throws exception if error occurs under creation</returns>
        public Task<Role?> PostRole(Role newRole);

        /// <summary>
        /// Gets a Role with given Id
        /// </summary>
        /// <param name="id">The id of the wanted Role</param>
        /// <returns>Role with given id, if not found returns null</returns>
        public Task<Role?> GetRole(string id);

        /// <summary>
        /// Gets all roles in the table
        /// </summary>
        /// <returns>List of Roles in the table or empty list if none is found</returns>
        public Task<List<Role>> GetAllRoles(Expression<Func<Role, bool>>? filter = null);

        /// <summary>
        /// Updates given Role
        /// </summary>
        /// <param name="Role">The new version of the Role</param>
        /// <returns>The Role that was updated, returns null if not succesfull</returns>
        public Task<Role?> UpdateRole(Role Role);

        /// <summary>
        /// Delets Role from table
        /// </summary>
        /// <param name="RoleId">Id of the Role needed to be deleted</param>
        /// <returns>Boolean, true if succesful and false if not</returns>
        public Task<bool> DeleteRole(string RoleId);
    }
}
