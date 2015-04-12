using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    public static class MorlnResources
    {
        public static T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public static Object Load(string path)
        {
            return Resources.Load(path);
        }

        public static Object Load(string path, Type systemTypeInstance)
        {
            return Resources.Load(path, systemTypeInstance);
        }
    }
}