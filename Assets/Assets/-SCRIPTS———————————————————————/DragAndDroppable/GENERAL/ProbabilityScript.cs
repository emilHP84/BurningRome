using UnityEngine;

public class ProbabilityScript : MonoBehaviour
{
    [Range(0,1f)]public float probabilityToSpawn;

    void Awake()
    {
        if (probabilityToSpawn<1f && probabilityToSpawn <= Random.value) Destroy(this.gameObject);
    }

} // FIN DU SCRIPT
