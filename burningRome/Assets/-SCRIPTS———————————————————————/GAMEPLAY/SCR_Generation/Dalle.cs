using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Dalle : MonoBehaviour, IFlamable
{
    float burning;
    [SerializeField] LayerMask burnableLayers;
    Collider[] allocColliders = new Collider[10];
    [SerializeField] GameObject flames;
    [SerializeField] GameObject fx_StartBurn;
    int test;

    bool propagateBurn = true;
    public bool PropagationBurn
    {
        get { return propagateBurn; } 
        set { propagateBurn = value; }
    }

    void Start()
    {
        flames.SetActive(false);
    }

    public bool BurnFor(float duration, bool piercing)
    {
        
        Collider[] hits = Physics.OverlapBox(transform.position, Vector3.one * 0.45f, Quaternion.identity, burnableLayers);

        foreach (Collider col in hits)
        {
            Debug.Log("dalle" + transform.position.x.ToString("f0") + " " + transform.position.z.ToString("f0") + " a trouv�: " + col.name);
            if(col.GetComponent<BombManager>())
            {
                
            }

            if (col.GetComponent<Indestructible>() && !piercing ) 
            {
                Debug.Log("bombe pas propag� bloc indestructible");
                return false;
            }
            if (col.GetComponent<Obstacle>())
            {
                Debug.Log("bombe pas propag� bloc destructible");
                CheckBurn(duration);
                return false;
            }
        }

        CheckBurn(duration);
        Debug.Log("bombe propag�" + propagateBurn);
        return propagateBurn;
    }
    void CheckBurn(float duration)
    {
        if (burning <= 0) StartBurn(duration);
        else if (duration > burning) burning = duration;
    }


    void StartBurn(float duration)
    {
        burning = duration;
        //Debug.Log("La case " + transform.position.x + "," + transform.position.z + " commmence a bruler");
        if (flames) flames.SetActive(true);
        if (fx_StartBurn) Instantiate(fx_StartBurn,transform.position,transform.rotation);
        StartCoroutine(BurnRoutine());
    }

    public void StopBurn()
    {
        //Debug.Log("La case " + transform.position.x + "," + transform.position.z + " ne brule plus");
        burning = 0;
        if (flames) flames.SetActive(false);
    }

    IEnumerator BurnRoutine()
    {
        while (burning>0)
        {
            if (GAME.MANAGER.CurrentState != State.gameplay) yield return null;
            burning -= Time.deltaTime;
            Collider[] hits = Physics.OverlapBox(transform.position, Vector3.one * 0.45f, Quaternion.identity, burnableLayers);
            foreach(Collider col in hits) if (col.GetComponent<IExplodable>()!=null) col.GetComponent<IExplodable>().Explode();
            //Physics.OverlapBoxNonAlloc(transform.position,Vector3.one*0.45f,allocColliders,transform.rotation,burnableLayers);
            //for (int i=0;i<allocColliders.Length;i++) allocColliders[i]?.GetComponent<IExplodable>().Explode();
            yield return null;
        }
        StopBurn();
    }

    
} // FIN DU SCRIPT
