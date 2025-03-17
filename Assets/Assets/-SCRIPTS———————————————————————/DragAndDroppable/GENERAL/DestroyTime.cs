using DG.Tweening;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    [Range(0,30f)]public float timeBeforeDestruction;
    [Range(0,5f)]public float shrinkAnimation;

    void Start()
    {
        Invoke("Shrink", timeBeforeDestruction);
    }

    void Shrink()
    {
        transform.DOScale(0,shrinkAnimation).OnComplete(AutoDestroy);
    }

    void AutoDestroy()
    {
        Destroy(this.gameObject);
    }


} // FIN DU SCRIPT
