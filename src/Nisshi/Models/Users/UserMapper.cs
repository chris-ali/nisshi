using AutoMapper;

namespace Nisshi.Models.Users
{
    public class UserMapper : AutoMapper.Profile
    {
        public UserMapper()
        {
            CreateMap<User, LoggedIn>(MemberList.None);
        }
    }
}