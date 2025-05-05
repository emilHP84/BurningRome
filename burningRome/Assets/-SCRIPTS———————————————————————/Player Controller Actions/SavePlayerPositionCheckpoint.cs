using UnityEngine;

public class SavePlayerPositionCheckpoint : MonoBehaviour
{
    [SerializeField] bool destroyAfterTriggered = true;
    [SerializeField] GameObject checkpointReachedFX;
    [SerializeField] LayerMask playerLayer;

    void OnEnable()
    {
        EVENTS.OnGameplay += DestroyIfPlayerInside;
    }

    void OnDisable()
    {
        EVENTS.OnGameplay -= DestroyIfPlayerInside;
    }

    void DestroyIfPlayerInside()
    {
        Collider[] found = Physics.OverlapSphere(transform.position,GetComponent<SphereCollider>().radius,playerLayer);
        for (int i=0; i<found.Length;i++)
        {
            if (found[i].GetComponentInParent<PlayerMove>())
            {
                gameObject.SetActive(false); 
                return; 
            }   
        } 
    }


    void OnTriggerEnter(Collider col)
    {
        if (GAME.MANAGER.CurrentState==State.gameplay && col.GetComponentInParent<PlayerMove>()) SavePlayerPosition();
    }

    void SavePlayerPosition()
    {
        SaveData data = new SaveData();
        data.playerPositionX = transform.position.x; // saving the player position in SaveData
        data.playerPositionY = transform.position.y; 
        data.playerPositionZ = transform.position.z; 
        data.playerLookRotationXVertical = transform.eulerAngles.x; // saving the rotation of the checkpoint as View Rotation for player in SaveData
        data.playerLookRotationYHorizontal = transform.eulerAngles.y;
        SaveLoadSystem.Save(data, 0); // writing the save data on disk to index 0
        PlayFX();
        if (destroyAfterTriggered) Destroy(gameObject);
    }

    void PlayFX()
    {
        if (checkpointReachedFX) Instantiate(checkpointReachedFX,transform.position,transform.rotation);
    }
} // SCRIPT END
