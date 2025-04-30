using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class DeadZoneManager : MonoBehaviour
{
    public float Delay;
    public float BlocApparitionDuration;
    // a placer autre par
    [Header("matrice")]
    [SerializeField] private int height = 17;
    [SerializeField] private int width = 11;
    [SerializeField] private int YHeight = 3;

    [SerializeField] private int[,] m_matrice;

    [Header("GameObject")]
    [SerializeField] private GameObject m_gameObject;

    [Header("position")]
    [SerializeField] private int top = 0;
    [SerializeField] private int bottom = 0;
    [SerializeField] private int left = 0;
    [SerializeField] private int right = 0;


    bool isTimed = true;
    private float time;

    private void OnEnable()
    {
        EVENTS.OnSuddenDeathEventHandler += GenerateDeadZone;
    }

    public void Awake()
    {
        width = width - 1;

        top = width;
        right = height;

        bottom -= 1;
        left -= 1;
    }

    public void Start()
    {
        //GenerateDeadZone();
    }

    //private void Update()
    //{
    //    time += Time.deltaTime;
    //    if (time >= Delay && isTimed == false) 
    //    { 
    //        GenerateDeadZone(this, new System.EventArgs());
    //        Delay = 0;
    //        isTimed = true;
    //    }
    //}

    public void GenerateDeadZone(object invoker, EventArgs e)
    {
        if(isTimed)
        {
            Debug.Log("ergyytybdydbryybrddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddu");
            StartCoroutine(GenerateSpirale());
            isTimed = false;
        }
    }

    public IEnumerator GenerateSpirale()
    {
        for (int i = 0; i < 4; i++)
        {
            // left --> right
            for (int j = 0 + i ; j < right; j++)
            {
                yield return new WaitForSeconds(BlocApparitionDuration);
                GenerateBloc(new Vector3(j, YHeight, top));
            }
            right--;
            left++;

            // top --> bottom
            for (int j = width - i - 1; j > 0 + bottom; j--)
            {
                yield return new WaitForSeconds(BlocApparitionDuration);
                GenerateBloc(new Vector3(right, YHeight, j));
            }
            top--;
            bottom++;

            // right --> left
            for (int j = right - 1; j > 0 + left ; j--)
            {
                yield return new WaitForSeconds(BlocApparitionDuration);
                GenerateBloc(new Vector3(j , YHeight, bottom));
            }

            // bottom --> top
            for (int j = 0 + i; j < top + 1; j++)
            {
                yield return new WaitForSeconds(BlocApparitionDuration);
                GenerateBloc(new Vector3(left, YHeight, j));
            }            
        }
        GenerateBloc(new Vector3(left, YHeight, top));
    }

    public void GenerateBloc(Vector3 direction)
    {
        Vector3 originScale = new Vector3 (1,1,1);
        GameObject obj = Instantiate(m_gameObject, direction, Quaternion.identity, this.gameObject.transform);
        obj.transform.localScale = new Vector3(0, 0, 0);
        obj.transform.DOScale(originScale, 0.6f);

    }

    private void OnDisable()
    {
        EVENTS.OnSuddenDeathEventHandler -= GenerateDeadZone;
    }
}
