using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDetect
{
    [Header("GAME SYSTEM")]
    [SerializeField] private float playerID;

    [Header("DEATH")]
    [SerializeField] private bool isAlive;
    public bool IsAlive
    {
        get { return isAlive; }
        private set { isAlive = value; }
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
        yield return new WaitForSeconds(deathTime);
        IsAlive = false;
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
