using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game
{
    public Player Player, Enemy;
    public List<Card> PlayerHand, EnemyHand, PlayerField, EnemyField;


    public Game()
    {
        Player = new Player();
        Enemy = new Player();
        GameManagerScr.Instance.EnemyDeck.RandomizeDeck(25);
        //GameManagerScr.Instance.PlayerDeck.RandomizeDeck(25);
        GameManagerScr.Instance.PlayerDeck.customDeck();
        //SpellCard card = (SpellCard)CardManager.AllCards.Find(x => x.Name == "mana potion");
        //EnemyDeck = new List<Card>{card.GetCopy(), card.GetCopy(), card.GetCopy()};
        //PlayerDeck = new List<Card>{card.GetCopy(), card.GetCopy(), card.GetCopy()};

    }


}

public class GameManagerScr : MonoBehaviour
{
    public DeckController PlayerDeck, EnemyDeck;
    public AI EnemyAI;
    public static GameManagerScr Instance;
    public Game CurrentGame;
    public Transform PlayerHand, EnemyHand, EnemyField, PlayerField;
    public GameObject CardPref, StatePref;
    int Turn, TurnTime = 30;

    public AttackedHero EnemyHero, PlayerHero;

    public bool IsPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
    }

    public List<CardController> PlayerHandCards = new List<CardController>(),
                                EnemyHandCards = new List<CardController>(),
                                PlayerFieldCards = new List<CardController>(),
                                EnemyFieldCards = new List<CardController>();


    void Start()
    {
        StartGame();
    }

    void Update()
    {
        CheckIfCardsPlayable();
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
    }


    public void CreateCardPref(Card card, Transform hand)
    {
        GameObject cardGO = Instantiate(CardPref, hand, false);
        CardController CardC = cardGO.GetComponent<CardController>();

        CardC.Init(card, hand == PlayerHand);

        if (CardC.IsPlayerCard) PlayerHandCards.Add(CardC);
        else EnemyHandCards.Add(CardC);
    }

    public void CreateBuffPref(CardController card, Buff buff)
    {
        GameObject buffGO = Instantiate(StatePref, card.StatusBars.BuffBar.transform, false);
        BuffFactory buffFactory = card.GetComponent<BuffFactory>();
        buffFactory.Init(buff, buffGO);
    }

    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        UIController.Instance.UpdateTurnTime(TurnTime);
        foreach (var card in PlayerFieldCards)
        {
            card.Info.HighlightCard(false);
        }

        if (IsPlayerTurn)
        {
            foreach (var card in PlayerFieldCards)
            {
                if (card.Card.Attack > 0 && !card.Card.Name.Contains("wall"))
                {
                    card.Info.CanAttack = true;
                    card.Info.HighlightCard(true);
                    card.AbilityController.OnNewTurn();
                }

            }

            while (TurnTime-- > 0)
            {
                UIController.Instance.UpdateTurnTime(TurnTime);
                yield return new WaitForSeconds(1);
            }

            ChangeTurn();
        }
        else
        {
            foreach (var card in EnemyFieldCards)
            {
                if (card.Card.Attack > 0 && !card.Card.Name.Contains("wall")) card.Info.CanAttack = true;
                card.AbilityController.OnNewTurn();
            }

            EnemyAI.MakeTurn();

            while (TurnTime-- > 0)
            {
                UIController.Instance.UpdateTurnTime(TurnTime);
                yield return new WaitForSeconds(1);
            }

            ChangeTurn();
        }

    }
    public void ChangeTurn()
    {
        StopAllCoroutines();

        Turn++;
        GiveNewCards(IsPlayerTurn);
        UIController.Instance.DisableTurnButton();
        if (IsPlayerTurn)
        {
            CurrentGame.Player.RestoreRoundResoures();
            GameManagerScr.Instance.PlayerHero.ShowHeroGoldChangedEvent(GameManagerScr.Instance.PlayerHero, CurrentGame.Player.GoldRate);
            GameManagerScr.Instance.PlayerHero.ShowHeroMPChangedEvent(GameManagerScr.Instance.PlayerHero, CurrentGame.Player.ManaRate);
        }
        else
        {
            CurrentGame.Enemy.RestoreRoundResoures();
            GameManagerScr.Instance.EnemyHero.ShowHeroGoldChangedEvent(GameManagerScr.Instance.EnemyHero, CurrentGame.Enemy.GoldRate);
            GameManagerScr.Instance.EnemyHero.ShowHeroMPChangedEvent(GameManagerScr.Instance.EnemyHero, CurrentGame.Enemy.ManaRate);
        }

        StartCoroutine(TurnFunc());
    }

    void GiveNewCards(bool isPlayerTurn)
    {
        if (isPlayerTurn) PlayerDeck.GiveCardToHand(PlayerHand);
        else EnemyDeck.GiveCardToHand(EnemyHand);

    }
    public void CardsFight(CardController attacker, CardController defender)
    {
        attacker.Info.CanAttack = false;
        attacker.GiveDamageTo(defender, attacker.Card.Attack);
        attacker.OnDamageDeal();
        defender.OnTakeDamage(attacker);

        if (!attacker.Card.Ranged)
        {
            defender.GiveDamageTo(attacker, defender.Card.Attack);
            attacker.OnTakeDamage();
        }

        if (!defender.Card.IsAlive && !IsPlayerTurn)
        {
            CurrentGame.Enemy.Gold++;
            GameManagerScr.Instance.EnemyHero.ShowHeroGoldChangedEvent(GameManagerScr.Instance.EnemyHero, 1);
        }

        if (!defender.Card.IsAlive && IsPlayerTurn)
        {
            CurrentGame.Player.Gold++;
            GameManagerScr.Instance.PlayerHero.ShowHeroGoldChangedEvent(GameManagerScr.Instance.PlayerHero, 1);
        }
        CheckIfCardsPlayable();
    }

    public void ReduceManaAndGold(bool isPlayer, int manacost, int goldcost)
    {
        if (isPlayer)
        {
            CurrentGame.Player.Mana = Mathf.Clamp(CurrentGame.Player.Mana - manacost, 0, int.MaxValue);
            CurrentGame.Player.Gold = Mathf.Clamp(CurrentGame.Player.Gold - goldcost, 0, int.MaxValue);
        }
        else
        {
            CurrentGame.Enemy.Mana = Mathf.Clamp(CurrentGame.Enemy.Mana - manacost, 0, int.MaxValue);
            CurrentGame.Enemy.Gold = Mathf.Clamp(CurrentGame.Enemy.Gold - goldcost, 0, int.MaxValue);
        }

    }

    public void DamageHero(CardController card, bool isEnemyAttacked)
    {
        if (isEnemyAttacked)
        {
            CurrentGame.Enemy.GetDamage(card.Card.Attack);
            EnemyHero.ShowHeroHPChangedEvent(EnemyHero, card.Card.Attack, true);
        }
        else
        {
            CurrentGame.Player.GetDamage(card.Card.Attack);
            PlayerHero.ShowHeroHPChangedEvent(PlayerHero, card.Card.Attack, true);
        }
        card.OnDamageDeal();
        CheckResult();
    }

    public void CheckResult()
    {
        if (CurrentGame.Enemy.HP <= 0 || CurrentGame.Player.HP <= 0)
        {
            StopAllCoroutines();
            UIController.Instance.ShowResult();
        }
    }

    public void CheckIfCardsPlayable()
    {
        foreach (var card in PlayerHandCards)
        {
            card.Info.CheckIfCanBePlayed(CurrentGame.Player.Mana, CurrentGame.Player.Gold);
        }
    }

    public void HighlightTargets(bool highlight, Card activeCard)
    {
        List<CardController> targets = new List<CardController>();

        if (activeCard.IsSpell)
        {
            var spellCard = (SpellCard)activeCard;

            if (spellCard.SpellTarget == SpellCard.TargetType.NO_TARGET)
                targets = new List<CardController>();

            if (spellCard.SpellTarget == SpellCard.TargetType.ALLY_CARD_TARGET)
                targets = PlayerFieldCards;

            if (spellCard.SpellTarget == SpellCard.TargetType.ENEMY_CARD_TARGET)
                targets = EnemyFieldCards;
        }

        else
        {
            if (EnemyFieldCards.Exists(x => x.Card.IsProvocation) && !activeCard.Ranged)
                targets = EnemyFieldCards.FindAll(x => x.Card.IsProvocation);
            else targets = EnemyFieldCards;
        }




        foreach (var card in targets)
        {
            if (activeCard.IsSpell) card.Info.HighlightAsSpellTarget(highlight);
            else card.Info.HighlightAsTarget(highlight);
        }

        if ((EnemyFieldCards.Count == 0 || activeCard.Ranged) && !activeCard.IsSpell)
            EnemyHero.HighlightAsTarget(highlight);


    }

    public void RestartGame()
    {
        StopAllCoroutines();

        foreach (var card in PlayerHandCards)
            Destroy(card.gameObject);
        foreach (var card in PlayerFieldCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyFieldCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyHandCards)
            Destroy(card.gameObject);

        PlayerHandCards.Clear();
        PlayerFieldCards.Clear();
        EnemyFieldCards.Clear();
        EnemyHandCards.Clear();

        StartGame();
    }

    public void StartGame()
    {

        Turn = 0;
        CurrentGame = new Game();


        EnemyDeck.GiveCardToHand(EnemyHand, 4); //todo: fix
        PlayerDeck.GiveCardToHand(PlayerHand, 4); //todo: fix
        UIController.Instance.StartGame();

        StartCoroutine(TurnFunc());
    }
}





