using UnityEngine;

public class HideOnStart : MonoBehaviour
{

    void Start()
    {
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) mr.enabled = false;
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) sr.enabled = false;
    }

}
