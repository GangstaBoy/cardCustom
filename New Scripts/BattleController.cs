
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class BattleController : MonoBehaviour
{
    public static event System.EventHandler PlayerTurnEnded;
    public EnemyAI enemyAI;
    public EnemyUI enemyUI;
    public PlayerUI playerUI;
    [SerializeField] private EnemyInstance _enemyInstance;
    [SerializeField] private PlayerInstance _playerInstance;
    private EnemySO[] _enemies;
    private GameCardSO[] _cards;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform _handTransform;


    void OnEnable()
    {
        Drop.CardDropEvent += PlayCard;
        EnemyAI.EnemyTurnEnded += StartTurn;
    }
    void OnDisable()
    {
        Drop.CardDropEvent -= PlayCard;
        EnemyAI.EnemyTurnEnded -= StartTurn;
    }

    private EnemySO PickRandomEnemy()
    {
        return _enemies.Length == 0 ? null : _enemies[Random.Range(0, _enemies.Length)];
    }

    private GameCardSO PickRandomCard()
    {
        return _cards.Length == 0 ? null : _cards[Random.Range(0, _cards.Length)];
    }

    private void InstantiateCards(int cardsCount)
    {
        for (int i = 0; i < cardsCount; i++)
        {
            GameCardSO cardSO = PickRandomCard();
            if (cardSO == null) return;
            var card = Object.Instantiate(cardPrefab, _handTransform);
            card.GetComponent<CardUI>().Display(cardSO);
            card.GetComponent<CardInstance>().Instantiate(cardSO, this);
        }
    }

    public void PlayCard(object sender, Drop.DropArgs e)
    {
        Debug.Log("Playing Card");
        if (e.dropObject.GetComponent<CardInstance>() == null) return;
        var card = e.dropObject.GetComponent<CardInstance>();
        if (card.Manacost > _playerInstance.Mana || card.Staminacost > _playerInstance.Stamina)
        {
            Debug.Log("Not enough resources!");
            return;
        }
        ProcessCard(card);
        Destroy(e.dropObject.gameObject);
    }

    private void Awake()
    {
        _enemies = Resources.LoadAll<EnemySO>("Characters/Enemies");
        _cards = Resources.LoadAll<GameCardSO>("GameCardObjects");

        Test();
    }

    private void ProcessCard(CardInstance card)
    {
        _playerInstance.decreaseResources(card.Manacost, card.Staminacost);
        for (int i = 0; i < card.СardEffectSOs.Length; i++)
        {
            ProcessCardEffect(card.СardEffectSOs[i]);
        }
    }

    private void ProcessCardEffect(CardEffectSO cardEffect)
    {
        switch (cardEffect.CardEffectType)
        {
            case CardEffectType.DEAL_DAMAGE:
                {
                    int damage = int.Parse(System.Array.Find<EffectValues>(cardEffect.EffectValues, p => p.key == "VALUE").value);
                    string type = System.Array.Find<EffectValues>(cardEffect.EffectValues, p => p.key == "TYPE").value;
                    _enemyInstance.getDamage(damage, type);
                    break;
                }

            case CardEffectType.RESTORE_HEALTH_INSTANT:
                {
                    int value = int.Parse(System.Array.Find<EffectValues>(cardEffect.EffectValues, p => p.key == "VALUE").value);
                    _playerInstance.increaseHealth(value);
                    break;
                }

            case CardEffectType.MANA_BURN:
                {
                    int burnValue = int.Parse(System.Array.Find<EffectValues>(cardEffect.EffectValues, p => p.key == "VALUE").value);
                    _enemyInstance.decreaseResources(burnValue);
                    break;
                }

            default: break;
        }
    }

    public void EndTurn()
    {
        if (PlayerTurnEnded != null) PlayerTurnEnded.Invoke(this, System.EventArgs.Empty);
        enemyAI.MakeTurn();
    }

    private void StartTurn(object sender, System.EventArgs eventArgs)
    {
        InstantiateCards(_playerInstance.CardsDrawPerTurn);
        _playerInstance.refreshStamina();
    }

    private void Test()
    {
        var enemy = PickRandomEnemy();
        _enemyInstance.Instantiate(enemy, this);
        enemyUI.Display(enemy, _enemyInstance);
        _playerInstance.Instantiate(30, 10, 10, this);
        playerUI.playerInstance = _playerInstance;
        InstantiateCards(_playerInstance.CardsDrawPerTurn);
    }
}