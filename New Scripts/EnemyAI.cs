using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private BattleController _battleController;
    [SerializeField] private PlayerInstance _enemyInstance, _playerInstance;

    public static event System.EventHandler EnemyTurnEnded;
    public static event System.EventHandler EnemyTurnStarted;


    public void MakeTurn()
    {
        StartCoroutine(EnemyTurn());
    }
    IEnumerator EnemyTurn()
    {
        if (EnemyTurnStarted != null) EnemyTurnStarted.Invoke(this, System.EventArgs.Empty);
        yield return new WaitForSeconds(1.50f);
        PlayTurn();
        if (EnemyTurnEnded != null) EnemyTurnEnded.Invoke(this, System.EventArgs.Empty);
    }

    private void PlayTurn()
    {
        _enemyInstance.refreshStamina();            //todo: move to a different location
        Debug.Log("Hit!");
        _playerInstance.getDamage(10, "PHYSICAL");
    }
}
