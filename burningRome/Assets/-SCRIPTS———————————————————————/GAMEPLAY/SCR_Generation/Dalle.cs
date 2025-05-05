using System.Collections;
using UnityEngine;

public class Dalle : MonoBehaviour, IFlamable
{
    float burning;
    [SerializeField] LayerMask burnableLayers;
    Collider[] allocColliders = new Collider[10];
    [SerializeField] GameObject flames;
    [SerializeField] GameObject fx_StartBurn;
    bool propagateBurn = true;

    public bool BurnFor(float duration)
    {
        if (burning<=0) StartBurn();
        if (duration>burning) burning = duration;
        return propagateBurn;
    }


    void StartBurn()
    {
        burning = 0;
        if (flames) flames.SetActive(true);
        if (fx_StartBurn) Instantiate(fx_StartBurn,transform.position,transform.rotation);
        StartCoroutine(BurnRoutine());
    }

    public void StopBurn()
    {
        burning = 0;
        if (flames) flames.SetActive(false);
    }

    IEnumerator BurnRoutine()
    {
        while (burning>0)
        {
            Physics.OverlapBoxNonAlloc(transform.position,Vector3.one*0.45f,allocColliders,transform.rotation,burnableLayers);
            for (int i=0;i<allocColliders.Length;i++) allocColliders[i]?.GetComponent<iDamageable>().TakeDamage(1);
            yield return null;
        }
        StopBurn();
    }

    
} // FIN DU SCRIPT

public interface IFlamable
{
    public bool BurnFor(float duration);
    public void StopBurn();
}
