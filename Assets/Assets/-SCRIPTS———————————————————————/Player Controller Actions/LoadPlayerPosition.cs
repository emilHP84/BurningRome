using UnityEngine;

public class LoadPlayerPosition : MonoBehaviour
{
    PlayerLook pLook => GetComponent<PlayerLook>();

    void Awake()
    {
        EVENTS.OnGameStart += LoadPosition;
    }

    void OnDestroy()
    {
        EVENTS.OnGameStart -= LoadPosition;
    }


    void LoadPosition()
    {
        SaveData data = SaveLoadSystem.Load(0);
        if (data!=null)
        {
            Vector3 loadedPos = new Vector3(data.playerPositionX, data.playerPositionY, data.playerPositionZ);
            if (PositionOnMapIsCorrect(loadedPos))
            {
                transform.position = loadedPos;
                Debug.Log("💾 Loaded Player position at "+loadedPos);
                if (pLook!=null)
                {
                    pLook.VerticalPivot.localEulerAngles = Vector3.right * data.playerLookRotationXVertical;
                    pLook.HorizontalPivot.localEulerAngles = Vector3.up * data.playerLookRotationYHorizontal;
                    Debug.Log("Look vAngle = "+pLook.VerticalPivot.localEulerAngles.x+"     Look hAngle = "+pLook.HorizontalPivot.localEulerAngles.y);
                }
            }
            else
            {
                Debug.Log("💾⚠️ Position to load is out of bounds! Aborting load");
                return;
            }
        }
        else Debug.Log("💾⚠️ Trying to load data file, but data is null");
    }

    bool PositionOnMapIsCorrect(Vector3 check) // avoid loading a position out of map
    {
        if (Mathf.Abs(check.x)>100f) return false;
        if (Mathf.Abs(check.z)>100f) return false;
        if (check.y<-1f) return false;
        return true;
    }

} // SCRIPT END
