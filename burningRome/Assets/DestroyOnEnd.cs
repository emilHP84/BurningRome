using UnityEngine;

public class DestroyOnEnd : MonoBehaviour
{
    void OnEnable()
    {
        EVENTS.OnEndGame += DestroyOnBattleEnd;
        if (GAMEPLAY.access.CurrentState == GameplayState.battleOver) DestroyOnBattleEnd();
    }

    void OnDisable()
    {
        EVENTS.OnEndGame -= DestroyOnBattleEnd;
    }


    void DestroyOnBattleEnd()
    {
        Destroy(gameObject);
    }
} // FIN DU SCRIPT
