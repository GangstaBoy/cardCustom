
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class BattleController : MonoBehaviour
{
    public Deck playerDeck;
    public Hand playerHand;
    public static event System.EventHandler PlayerTurnEnded;
    public EnemyAI enemyAI;
    public PlayerUIController playerUI, enemyUI;
    [SerializeField] private PlayerInstance _enemyInstance, _playerInstance;
    private EnemySO[] _enemies;
    private GameCardSO[] _cards;
    [SerializeField] private GameObject cardPrefab, statePrefab, playerBuffZone, enemyBuffZone;
    [SerializeField] private Transform _handTransform;


    void OnEnable()
    {
        Moveable.CardDiscarded += DiscardCard;
        Drop.CardDropEvent += PlayCard;
        PlayerInstance.PlayerDied += ProcessDeath;
        EnemyAI.EnemyTurnEnded += StartTurn;
    }
    void OnDisable()
    {
        Moveable.CardDiscarded -= DiscardCard;
        Drop.CardDropEvent -= PlayCard;
        PlayerInstance.PlayerDied -= ProcessDeath;
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

    public void ProcessDeath(object sender, PlayerInstance.PlayerDiedArgs eventArgs)
    {
        Debug.Log("Died! " + eventArgs.diedInstance.gameObject + " " + eventArgs.diedInstance.name);
        Test();
    }

    public void DiscardCard(object sender, Moveable.CardDiscardedArgs eventArgs)
    {
        playerHand.RemoveCard(eventArgs.card.GetComponent<CardInstance>());
    }

    private void InstantiateCards(int cardsCount)
    {
        for (int i = 0; i < cardsCount; i++)
        {
            Debug.Log("Can draw? " + playerHand.canDraw);
            if (playerHand.canDraw)
            {
                GameCardSO cardSO = playerDeck.DrawCard();
                if (cardSO == null)
                {
                    Debug.Log("Cards over!");       //todo: start reshuffle
                    playerDeck.RandomizeDeck();
                    cardSO = playerDeck.DrawCard();
                }
                var card = Object.Instantiate(cardPrefab, _handTransform);
                card.GetComponent<CardUI>().Display(cardSO);
                var cardInstance = card.GetComponent<CardInstance>();
                cardInstance.Instantiate(cardSO, this);
                playerHand.AddCard(cardInstance);
            }
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
        playerHand.RemoveCard(card);
        Destroy(e.dropObject.gameObject);

        void ProcessCard(CardInstance card)
        {
            _playerInstance.decreaseResources(card.Manacost, card.Staminacost);
            for (int i = 0; i < card.СardEffectSOs.Length; i++)
            {
                ProcessCardEffect(card.СardEffectSOs[i]);
            }
        }
    }

    private void Awake()
    {
        _enemies = Resources.LoadAll<EnemySO>("Characters/Enemies");
        //_cards = Resources.LoadAll<GameCardSO>("GameCardObjects");
        Test();
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

            case CardEffectType.REDUCE_LIFE:
                {
                    int reduceValue = int.Parse(System.Array.Find<EffectValues>(cardEffect.EffectValues, p => p.key == "VALUE").value);
                    _enemyInstance.getDamage(_enemyInstance.Health - 1, "PURE");
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
        InstantiateCards(playerHand.maxHandSize);
        _playerInstance.refreshStamina();
    }

    private void Test()
    {
        var enemy = PickRandomEnemy();
        _enemyInstance.Instantiate(this, OpponentType.ENEMY, enemy);
        enemyUI.Instantiate(_enemyInstance, enemy);
        _playerInstance.Instantiate(this, OpponentType.PLAYER, 30, 10, 10);
        playerUI.Instantiate(_playerInstance);
        InstantiateCards(playerHand.maxHandSize);
    }
}