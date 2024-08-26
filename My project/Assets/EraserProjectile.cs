using UnityEngine;

public class EraserProjectile : BaseProjectile
{
    protected override void Start()
    {
        speed = 20f;
        base.Start();
        // CARACTERISTICAS DO PROJETIL
    }

    protected override void Update()
    {
        base.Update();
        // Adicionar comportamentos especificos
    }

    // Colisões aqui
}
