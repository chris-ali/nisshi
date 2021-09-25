namespace Nisshi.Models 
{
    public class HeroUser
    {
        public int UserIdFk { get; set; }

        public User User { get; set; }

        public int HeroIdFk { get; set; }

        public Hero Hero { get; set; }
    }
}