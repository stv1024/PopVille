using System.Collections.Generic;

namespace Assets.Sanxiao
{
    public static class ServerUserPrefs
    {
        public enum Key
        {
            IceTute,
        }

        public static readonly Dictionary<Key, bool> BoolDict = new Dictionary<Key, bool>();

        public static void SetBool(Key key, bool value)
        {
            
        }
        public static bool? GetBool(Key key)
        {
            return null;
        }
        public static void Delete(Key key)
        {
            
        }
        public static bool Contains(Key key)
        {
            return false;
        }
    }
}