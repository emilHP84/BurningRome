using UnityEngine;

public class PlayerAnim : MonoBehaviour
{

    public void PlayIdle()
    {
        Play(idle);
    }

    public void PlayMoveDown()
    {
        Play(movedown);
    }

    public void PlayMoveUp()
    {
        Play(moveup);
    }

    public void PlayMoveRight()
    {
        Play(moveright);
    }

    public void PlayMoveLeft()
    {
        Play(moveleft);
    }

    public void PlaySpawn()
    {
        Play(spawn);
    }

    public void HideAllAnim()
    {
        idle.SetActive(false);
        movedown.SetActive(false);
        moveup.SetActive(false);
        moveleft.SetActive(false);
        moveright.SetActive(false);
        spawn.SetActive(false);
    }



    [SerializeField]GameObject idle,movedown,moveup,moveleft,moveright,spawn;

    Vector3 lastPos;
    

    void Play(GameObject anim)
    {
        //if (anim.activeSelf) return;
        HideAllAnim();
        anim.SetActive(true);
    }



} // FIN DU SCRIPT
