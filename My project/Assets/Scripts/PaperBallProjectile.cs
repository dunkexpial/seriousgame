using UnityEngine;

public class PaperBallProjectile : BaseProjectile
{
    protected override void Start()
    {
        damageAmount = 4; // Set the damage specific to this type of projectile
        speed = 300f;
        base.Start();
        spinSpeed = 2000f;
        // CARACTERISTICAS DO PROJETIL
    }

    protected override void Update()
    {
        base.Update();
        // Adicionar comportamentos especificos se necessário
    }
}
