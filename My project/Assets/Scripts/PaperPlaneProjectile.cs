using UnityEngine;

public class PaperPlaneProjectile : BaseProjectile
{
    protected override void Start()
    {
        damageAmount = 10; // Set the damage specific to this type of projectile
        speed = 500f;
        spinSpeed = 0f;
        base.Start();
        // CARACTERISTICAS DO PROJETIL
    }

    protected override void Update()
    {
        base.Update();
        // Adicionar comportamentos especificos se necessário
    }
}
