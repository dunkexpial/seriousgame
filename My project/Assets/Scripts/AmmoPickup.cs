using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Tooltip("Which projectile type this pickup gives ammo to (index).")]
    public int projectileTypeIndex;

    [Tooltip("Minimum ammo to give.")]
    public int minAmount = 5;

    [Tooltip("Maximum ammo to give.")]
    public int maxAmount = 15;
}
