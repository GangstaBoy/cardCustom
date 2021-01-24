using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance; //singleton
    public TextMeshProUGUI PlayerHP, PlayerMana, PlayerGold, EnemyHP, EnemyMana, EnemyGold;
    public GameObject ResultGo;
    public TextMeshProUGUI ResultText;
    public TextMeshProUGUI TurnTime;
    public Button EndTurnButton;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        EndTurnButton.interactable = true;
        ResultGo.SetActive(false);
        UpdateResources();

    }

    public void UpdateResources()
    {
        PlayerHP.text = GameManagerScr.Instance.CurrentGame.Player.HP.ToString();
        PlayerMana.text = GameManagerScr.Instance.CurrentGame.Player.Mana.ToString();
        PlayerGold.text = GameManagerScr.Instance.CurrentGame.Player.Gold.ToString();
        EnemyHP.text = GameManagerScr.Instance.CurrentGame.Enemy.HP.ToString();
        EnemyMana.text = GameManagerScr.Instance.CurrentGame.Enemy.Mana.ToString();
        EnemyGold.text = GameManagerScr.Instance.CurrentGame.Enemy.Gold.ToString();

    }

     public void ShowResult()
     {
         ResultGo.SetActive(true);
         if(GameManagerScr.Instance.CurrentGame.Player.HP <= 0) ResultText.text = "You lost!";
         else ResultText.text = "You won!";
     }

     public void UpdateTurnTime (int time)
     {
         TurnTime.text = time.ToString();
     }
     
     public void DisableTurnButton()
     {
         EndTurnButton.interactable = GameManagerScr.Instance.IsPlayerTurn; //quite a strange implementation
     }


}
