using UnityEngine;

public class EnergyBallProjectile : BaseProjectile
{
    protected override void Start()
    {
        damageAmount = 8; // Set the damage specific to this type of projectile
        speed = 400f;
        spinSpeed = 1000f;
        base.Start();
        
    }

    private void OnDestroy()
    {
        Transform visualChild = transform.Find("Visual");
        if (visualChild != null)
        {
            visualChild.SetParent(null); // Detach the child from the projectile

            // Reset the child's scale to maintain its original appearance
            visualChild.localScale = Vector3.one;

            // Destroy the child after 1 second
            Destroy(visualChild.gameObject, 3f);
        }
    }

    protected override void Update()
    {
        base.Update();
        // Add specific behaviors if necessary
    }
}
