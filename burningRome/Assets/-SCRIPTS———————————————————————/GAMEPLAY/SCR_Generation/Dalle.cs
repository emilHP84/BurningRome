using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Dalle : MonoBehaviour, IFlamable
{
    float burning;
    [SerializeField] LayerMask burnableLayers;
    Collider[] allocColliders = new Collider[10];
    [SerializeField] GameObject flames;
    [SerializeField] GameObject hadesFlames;
    int test;
    bool isHadesFire;


    bool propagateBurn = true;
    public bool PropagationBurn
    {
        get { return propagateBurn; }
        set { propagateBurn = value; }
    }

    void Start()
    {
        flames.SetActive(false);
        hadesFlames.SetActive(false);
    }

    public bool BurnFor(float duration, bool piercing, bool wantHadesFire)
    {
        if (!isHadesFire || (isHadesFire && duration > burning))
            isHadesFire = wantHadesFire;


        Collider[] hits = Physics.OverlapBox(transform.position, Vector3.one * 0.45f, Quaternion.identity, burnableLayers);

        foreach (Collider col in hits)
        {
            //Debug.Log("dalle" + transform.position.x.ToString("f0") + " " + transform.position.z.ToString("f0") + " a trouv�: " + col.name);

            if (col.GetComponentInParent<BombManager>())
            {
                CheckBurn(duration);
                col.GetComponentInParent<IExplodable>().Explode();
            }

            if (col.GetComponent<Indestructible>() && !piercing)
            {
                //Debug.Log("bombe pas propag� bloc indestructible");
                return false;
            }

            if (col.GetComponent<Obstacle>())
            {
                //Debug.Log("bombe pas propag� bloc destructible");
                CheckBurn(duration);
                return false;
            }
        }

        CheckBurn(duration);
        //Debug.Log("bombe propag�" + propagateBurn);
        return propagateBurn;
    }
    void CheckBurn(float duration)
    {

        if (burning <= 0) StartBurn(duration);
        else if (duration > burning)
        {
            SwitchFlammeVFX();
            burning = duration;
        }

    }

    void SwitchFlammeVFX()
    {
        hadesFlames?.SetActive(isHadesFire);
        flames?.SetActive(!isHadesFire);
    }

    void StartBurn(float duration)
    {
        if (duration <= 0)
        {
            Debug.Log("lol");
            return;
        }
        burning = duration;

        if (isHadesFire)
        {
            hadesFlames.SetActive(true);
            hadesFlames.transform.DOScale(Vector3.one, 0.2f).From(0).SetEase(Ease.OutBack);
        }
        else if (flames)
        {
            flames.SetActive(true);
            flames.transform.DOScale(Vector3.one, 0.2f).From(0).SetEase(Ease.OutBack);
        }
        StartCoroutine(BurnRoutine());
    }

    public void StopBurn()
    {
        //Debug.Log("La case " + transform.position.x + "," + transform.position.z + " ne brule plus");
        burning = 0;
        isHadesFire = false;
        if (flames.activeSelf)
        {
            flames.transform.DOKill();
            flames.transform.DOScale(Vector3.zero, 2f).From(1f).SetEase(Ease.OutExpo).OnComplete(DisableHadesFlames);
        }
        if (hadesFlames.activeSelf)
        {
            hadesFlames.transform.DOKill();
            hadesFlames.transform.DOScale(Vector3.zero, 2f).From(1f).SetEase(Ease.OutExpo).OnComplete(DisableFlames);
        }
    }

    void DisableFlames()
    {
        flames.SetActive(false); 
    }

    void DisableHadesFlames()
    {
        hadesFlames.SetActive(false);
    }

    IEnumerator BurnRoutine()
    {
        while (burning > 0)
        {
            if (GAME.MANAGER.CurrentState != State.gameplay) yield return null;
            burning -= Time.deltaTime;
            Collider[] hits = Physics.OverlapBox(transform.position, Vector3.one * 0.45f, Quaternion.identity, burnableLayers);
            foreach (Collider col in hits) if (col.GetComponent<IExplodable>() != null) col.GetComponent<IExplodable>().Explode();
            //Physics.OverlapBoxNonAlloc(transform.position,Vector3.one*0.45f,allocColliders,transform.rotation,burnableLayers);
            //for (int i=0;i<allocColliders.Length;i++) allocColliders[i]?.GetComponent<IExplodable>().Explode();
            yield return null;
        }
        StopBurn();
    }


} // FIN DU SCRIPT
