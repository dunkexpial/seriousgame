using UnityEngine;

public class PaperBallProjectile : BaseProjectile
{
    protected override void Start()
    {
        damageAmount = 2; // Set the damage specific to this type of projectile
        speed = 5f;
        base.Start();
        // CARACTERISTICAS DO PROJETIL
    }

    protected override void Update()
    {
        base.Update();
        // Adicionar comportamentos especificos se necessário
    }
}
