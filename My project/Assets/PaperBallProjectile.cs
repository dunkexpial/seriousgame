using UnityEngine;

public class PaperBallProjectile : BaseProjectile
{
    protected override void Start()
    {
        speed = 50f;
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
