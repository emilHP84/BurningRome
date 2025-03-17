using UnityEngine;

public class ChooseOneChildRandom : MonoBehaviour
{
    void Awake()
    {
        transform.GetChild(Random.Range(0,transform.childCount)).parent = transform.parent;
        Destroy(this.gameObject);
    }
}
