using UnityEngine;

public class EraserProjectile : BaseProjectile
{
    protected override void Start()
    {
        damageAmount = 5; // Set the damage specific to this type of projectile
        speed = 200f;
        spinSpeed = 1000f;
        base.Start();
        // CARACTERISTICAS DO PROJETIL
    }

    protected override void Update()
    {
        base.Update();
        // Adicionar comportamentos especificos se necessário
    }
}
