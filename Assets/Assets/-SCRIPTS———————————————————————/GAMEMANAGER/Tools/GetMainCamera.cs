using UnityEngine;

public class GetMainCamera : MonoBehaviour
{
    static int getMainCamInstances = 0;

    void Awake()
    {
        Destroy(GetComponentInChildren<Camera>().gameObject);
    }

    void OnEnable()
    {
        getMainCamInstances++;
        if (getMainCamInstances>1)
        {
            this.enabled = false;
            Debug.Log("🎦⚠️ Can't enable two GetMainCamera components in scene!");
        }
    }

    void OnDisable()
    {
        getMainCamInstances--;
    }


    void LateUpdate()
    {
        GAME.MANAGER.gameCam.position = transform.position;
        GAME.MANAGER.gameCam.rotation = transform.rotation;
    }
} // SCRIPT END
