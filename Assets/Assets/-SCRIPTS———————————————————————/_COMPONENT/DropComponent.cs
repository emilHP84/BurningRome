using UnityEngine;

public class DropComponent : MonoBehaviour
{
    public void DroppingObject(GameObject gameObject,Vector3 position,Quaternion rotation, Transform parent)
    {
        Instantiate(gameObject,position,rotation,parent);
    }
}
