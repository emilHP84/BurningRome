using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour ,IDetect
{
    [Header("GAME SYSTEM")]
    [SerializeField] private int playerID;
    public int PlayerID
    {
        get { return playerID; }
        private set { playerID = value; }
    }


    [Header("DEATH")]
    [SerializeField] private bool isAlive;
    [SerializeField] private bool hited;
    public bool IsAlive
    {
        get { return isAlive; }
        private set { isAlive = value; }
    }
    public bool Hited
    {
        get { return hited; }
        private set { Hited = value; }
    }
    [SerializeField] private float deathTime;



    private void OnEnable()
    {
        isAlive = true;
    }

    private void Awake()
    {
        
    }

    void Start()
    {

    }

    void Update()
    {

    }

    IEnumerator OnDeath(float deathTime)
    {
        transform.DOScale(new Vector3(0, 0, 0), deathTime);
        isAlive = false;
        hited = true;
        yield return new WaitForSeconds(deathTime);
        EVENTS.InvokeOnDeath(this, playerID);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        isAlive = false;
    }



    public void OnDetectionWith(IDetect detect)
    {
        Debug.Log("player touché...");
        StartCoroutine(OnDeath(deathTime));
    }
}
