using UnityEngine;

public class DropComponent : MonoBehaviour
{
    public GameObject DroppingObject(GameObject gameObject,Vector3 position,Quaternion rotation, Transform parent)
    {
        GameObject droppedObject = Instantiate(gameObject, position, rotation, parent);
        return droppedObject;
    }
}
