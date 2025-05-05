using UnityEngine;

public class IfNoChildThenDestroy : MonoBehaviour
{
    void Update()
    {
        if (transform.childCount<1) Destroy(gameObject);
    }
}
