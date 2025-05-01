using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [SerializeField]GameObject idle,movedown,moveup,moveleft,moveright,death,spawn,victory;

    Vector3 lastPos;


    void Update()
    {
        Vector3 movement = transform.localPosition - lastPos;
        if (movement.sqrMagnitude==0)
        {
            if (GAMEPLAY.access.CurrentState==GameplayState.joining) Play(spawn);
            else Play(idle);
        }
        else
        {
            if (movement.y!=0)
            {
                if (movement.y>0) Play(moveup); else Play(movedown);
            }
            if (movement.x!=0)
            {
                if (movement.x>0) Play(moveright); else Play(moveleft);
            }
        }
    }


    void LateUpdate()
    {
        lastPos = transform.localPosition;
    }

    void Play(GameObject anim)
    {
        if (anim.activeSelf) return;
        HideAllAnim();
        anim.SetActive(true);
    }

    void HideAllAnim()
    {
        idle.SetActive(false);
        movedown.SetActive(false);
        moveup.SetActive(false);
        moveleft.SetActive(false);
        moveright.SetActive(false);
        death.SetActive(false);
        spawn.SetActive(false);
        victory.SetActive(false);
    }

} // FIN DU SCRIPT
