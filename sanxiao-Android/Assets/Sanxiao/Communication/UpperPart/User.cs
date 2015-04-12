using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public static class UserExtension
    {
        public static User GetDuplicate(this User user)
        {
            var dup = new User(user.UserId, user.Nickname, user.Level, user.CharacterCode, user.RoundInitHealth,
                               user.EnergyCapacity, user.RoundCount);
            return dup;
        }
    }
}