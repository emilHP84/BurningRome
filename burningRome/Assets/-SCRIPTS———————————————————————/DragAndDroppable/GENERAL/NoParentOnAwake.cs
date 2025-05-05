using UnityEngine;

public class NoParentOnAwake : MonoBehaviour
{
    [SerializeField] bool followOldParent = false;
    Transform myParent;

    void Awake()
    {
        myParent = transform.parent;
        transform.parent = null;
        if (followOldParent==false) Destroy(this);
    }

    void LateUpdate()
    {
        if (myParent==null) Destroy(this);
        else
        {
            transform.position = myParent.position;
            transform.rotation = myParent.rotation;
        }

    }


} // FIN DU SCRIPT
