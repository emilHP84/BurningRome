using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class DeadZoneManager : MonoBehaviour
{
    public float Delay;
    public float BlocApparitionDuration;
    // a placer autre par
    [Header(" matrice")]
    [SerializeField] private int height = 17;
    [SerializeField] private int width = 11;

    [SerializeField] private int[,] m_matrice;

    [Header("GameObject")]
    [SerializeField] private GameObject m_gameObject;

    [Header(" position")]
    [SerializeField] private int top = 0;
    [SerializeField] private int bottom = 0;
    [SerializeField] private int left = 0;
    [SerializeField] private int right = 0;


    bool isTimed;
    private float time;

    private void OnEnable()
    {

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

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= Delay && isTimed == false) 
        { 
            GenerateDeadZone();
            Delay = 0;
            isTimed = true;
        }
    }

    public void GenerateDeadZone()
    {
        StartCoroutine(GenerateSpirale());
    }

    public IEnumerator GenerateSpirale()
    {
        for (int i = 0; i < 3; i++)
        {
            // left --> right
            for (int j = 0; j < right; j++)
            {
                yield return new WaitForSeconds(BlocApparitionDuration);
                GenerateBloc(new Vector3(j, transform.position.y, top));
            }
            right--;
            left++;

            // top --> bottom
            for (int j = width; j > 0 + bottom ; j--)
            {
                yield return new WaitForSeconds(BlocApparitionDuration);
                GenerateBloc(new Vector3(right, transform.position.y, j));
            }
            top--;
            bottom++;

            // right --> left
            for (int j = right; j > 0 + left ; j--)
            {
                yield return new WaitForSeconds(BlocApparitionDuration);
                GenerateBloc(new Vector3(j , transform.position.y, bottom));
            }

            // bottom --> top
            for (int j = 0; j < top; j++)
            {
                yield return new WaitForSeconds(BlocApparitionDuration);
                GenerateBloc(new Vector3(left, transform.position.y, j));
            }            
        }
        GenerateBloc(new Vector3(left, transform.position.y, top));
    }

    public void GenerateBloc(Vector3 direction)
    {
        GameObject obj = Instantiate(m_gameObject, direction, Quaternion.identity, transform.parent);
        obj.transform.localScale = new Vector3(0, 0, 0);
        obj.transform.DOScale(new Vector3(1, 1, 1), 0.6f);

    }

    private void OnDisable()
    {

    }
}
