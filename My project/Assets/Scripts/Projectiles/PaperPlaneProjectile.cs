using UnityEngine;

public class PaperPlaneProjectile : BaseProjectile
{
    protected override void Start()
    {
        damageAmount = 20; // Set the damage specific to this type of projectile
        speed = 500f;
        spinSpeed = 0f;
        base.Start();
    }
    
    private void OnDestroy() {
        
    }

    protected override void Update()
    {
        base.Update();
        // Adicionar comportamentos especificos se necessário
    }
}
