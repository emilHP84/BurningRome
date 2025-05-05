using UnityEngine;
using System;

public class HealthScript : MonoBehaviour, iDamageable
{
    [Range(1f,100f)][SerializeField] float hpMax = 1;
    float hp;
    public float HP { get {return hp;} set {hp=value; if(hp<1) Death();} }
    public float HPMax{get{return hpMax;}}
    [SerializeField] bool spawnOnStart = true;
    [SerializeField] bool destroyOnDeath = true;

    public event Action OnDeath;
    public void InvokeDeath() {Debug.Log(gameObject.name+" Death 💀"); OnDeath?.Invoke();}   
    public event Action OnSpawn;
    public void InvokeSpawn() {Debug.Log(gameObject.name+" Spawn 🚼"); OnSpawn?.Invoke();}   
    public event Action<float> OnDamage;
    public void InvokeDamage(float amount) {Debug.Log(gameObject.name+" Damaged 🪓"); OnDamage?.Invoke(amount);}   


    bool dead = true;
    public bool Dead{get{return dead;}}
    bool invulnerable = false;
    public bool Invulnerable{get{return invulnerable;} set{invulnerable = value;}}
    [Header("OPTIONAL")]
    [SerializeField] BarHandler healthBar;
    [Header("EFFECTS")]
    [SerializeField] GameObject[] onSpawn;
    [SerializeField] GameObject[] onHit;
    [SerializeField] GameObject[] onDeath;


    void Start()
    {
        if (spawnOnStart) SpawnFullHealth();
    }


    public void Spawn()
    {
        dead = false;
        for (int i=0;i<onSpawn.Length;i++) if (onSpawn[i]) Instantiate (onSpawn[i], transform);
        InvokeSpawn();
    }

    public void SpawnFullHealth()
    {
        FullHealth();
        Spawn();
    }


    public void FullHealth()
    {
        HP = hpMax;
    }

    public void ChangeMaxHP(int quantity, bool adjustCurrentHP)
    {
        if (quantity<=0)
        {
            hpMax = hp = 0;
            Death();
        }
        else
        {
            if (adjustCurrentHP) {hp = Mathf.CeilToInt(hp*quantity/(float)hpMax);}
            hpMax = quantity;
        }
    }

    public void Death()
    {
        if (Dead) return;
        dead = true;
        for (int i=0;i<onDeath.Length;i++) if (onDeath[i]) Instantiate (onDeath[i], transform);
        InvokeDeath();
        if (destroyOnDeath) Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        if (Dead || Invulnerable || GAME.MANAGER.CurrentState!=State.gameplay) return;
        HP = Mathf.Clamp(HP-amount,0,hpMax);
        if (Dead==false)
        {
            for (int i=0;i<onHit.Length;i++) if (onHit[i]) Instantiate (onHit[i], transform);
            InvokeDamage(amount);
            if (healthBar) healthBar.FillAmount = HP/HPMax;
        }
    }

} // FIN DU SCRIPT