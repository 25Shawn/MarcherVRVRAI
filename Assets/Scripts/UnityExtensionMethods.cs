using UnityEngine;

public static class UnityExtensionMethods
{
    /// <summary>
    /// Permet de v�rifier si un objet Unity est d�truit correctement.
    /// </summary>
    public static bool IsDestroyed(this Object obj)
    {
        return obj == null || obj.Equals(null);
    }
}
