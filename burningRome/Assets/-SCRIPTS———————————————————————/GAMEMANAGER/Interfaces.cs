using UnityEngine;

public interface iDamageable
{
    public void TakeDamage(float amount);
}

public interface iDamageableBy
{
    public void TakeDamageBy(Transform source, float amount);
}

public interface iPushable
{
    public void Push(Vector3 origin, Vector3 force);
}

public interface iPushableBy
{
    public void PushedBy(Transform source, Vector3 force);
}

// <-- Setup new interfaces here


public interface IFlamable
{
    public bool BurnFor(float duration, bool piercing,bool isHadesFire);
    public void StopBurn();
}


public interface IExplodable
{
    public void Explode();
}
