using AutoMapper;

namespace Nisshi.Models
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserLoggedIn>(MemberList.None);
        }
    }
}