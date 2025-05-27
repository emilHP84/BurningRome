using UnityEngine;
using DG.Tweening;

public class CameraFocus : MonoBehaviour
{
    Vector3 startPos;

    void Awake()
    {
        startPos = transform.position;
    }

    private void OnEnable()
    {
        EVENTS.OnCallCamera += MoveCamera;
        EVENTS.OnRematch += ResetPosition;
    }

    private void OnDisable()
    {
        EVENTS.OnCallCamera -= MoveCamera;
        EVENTS.OnRematch -= ResetPosition;
    }

    void MoveCamera(GameObject i)
    {
        transform.DOKill();
        transform.DOMove(new Vector3(i.transform.position.x, i.transform.position.y + 4.5f, i.transform.position.z - 3), 3).From(startPos);
    }

    void ResetPosition()
    {
        transform.DOKill();
        transform.position = startPos;
    }

} // FIN DU SCRIPT
