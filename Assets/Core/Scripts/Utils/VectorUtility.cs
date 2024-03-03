using UnityEngine;

namespace Core.Scripts.Utils
{
    public static class VectorUtility
    {
        public static Vector2 Flatten(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }
    }
}