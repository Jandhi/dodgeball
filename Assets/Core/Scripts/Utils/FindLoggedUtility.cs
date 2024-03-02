using UnityEngine;

namespace Core.Scripts.Utils
{
    public static class FindLoggedUtility
    {
        public static Transform FindLogged(this Transform transform, string name)
        {
            var value = transform.Find(name);
            if(value is null)
                Debug.LogError($"{transform.name} could not find child {name}");
            return value;
        }
    }
}