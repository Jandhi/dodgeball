using UnityEngine;

namespace Core.Scripts.Utils
{
    public static class VectorUtility
    {
        public static Vector2 Flatten(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }

        public static Vector3 WithZ(this Vector2 vec, float z)
        {
            return new Vector3(vec.x, vec.y, z);
        }
    }
}