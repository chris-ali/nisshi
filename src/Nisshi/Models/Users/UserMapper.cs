using AutoMapper;

namespace Nisshi.Models.Users
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, LoggedIn>(MemberList.None);
        }
    }
}