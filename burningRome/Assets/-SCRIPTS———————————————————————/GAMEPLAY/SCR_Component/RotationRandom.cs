using UnityEngine;

public class RotationRandom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float finalrotation = 0;
        float chance = Random.value;
        if (chance > 0.75f) finalrotation = 90f;
        else if (chance > 0.5f) finalrotation = 180f;
        else if (chance > 0.25f) finalrotation = 270f;
        transform.localEulerAngles = Vector3.up * finalrotation;
    }
}
