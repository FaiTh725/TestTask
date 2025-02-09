using CSharpFunctionalExtensions;

namespace Authentication.Domain.Entities
{
    public class Role
    {
        public string RoleName { get; } = string.Empty;
        
        public List<User> Users { get; set; } = new List<User>();

        // For EF
        public Role() { }
        private Role(string roleName)
        {
            RoleName = roleName;
        }

        public static Result<Role> Initialize(string roleName)
        {
            if(roleName is null)
            {
                return Result.Failure<Role>("Role Name Is Null");
            }

            return Result.Success(new Role(roleName));
        }
    }
}
