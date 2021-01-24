using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game 
{
    public Player Player, Enemy;
    public List<Card> EnemyDeck, PlayerDeck, PlayerHand, EnemyHand, PlayerField, EnemyField;
    

    public Game()
    {
        Player = new Player();
        Enemy = new Player();
        //SpellCard card = (SpellCard)CardManager.AllCards.Find(x => x.Name == "mana potion");
        //EnemyDeck = new List<Card>{card.GetCopy(), card.GetCopy(), card.GetCopy()};
        EnemyDeck = GiveDeckCard();
        //PlayerDeck = new List<Card>{card.GetCopy(), card.GetCopy(), card.GetCopy()};
        PlayerDeck = GiveDeckCard();
       
    }

    List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();
        for(int i = 0; i<20; i++)
        {
            var card = CardManager.AllCards[Random.Range(0, CardManager.AllCards.Count)];
            if (card.IsSpell) list.Add(((SpellCard)card).GetCopy());
            else list.Add(card.GetCopy());
        }
        return list;
    }
}

public class GameManagerScr : MonoBehaviour
{
    public AI EnemyAI;
    public static GameManagerScr Instance;
    public Game CurrentGame;
    public Transform PlayerHand, EnemyHand, EnemyField, PlayerField;
    public GameObject CardPref;
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

    void Awake() 
    {
        if(Instance == null) Instance = this;
    }

    void GiveHandCards(List<Card> deck, Transform hand)
    {
        for(int i = 0; i<4; i++)
            GiveCardToHand(deck, hand);
    }

    void GiveCardToHand(List<Card> deck, Transform hand)
    {
        if(deck.Count == 0)
            return;
        
        CreateCardPref(deck[0], hand);
        Debug.Log("Card " + deck[0].Name + " to " + hand );

        deck.RemoveAt(0);

    }

    void CreateCardPref(Card card, Transform hand)
    {
        GameObject cardGO = Instantiate(CardPref, hand, false);
        CardController CardC = cardGO.GetComponent<CardController>();

        CardC.Init(card, hand == PlayerHand);

        if (CardC.IsPlayerCard) PlayerHandCards.Add(CardC);
        else EnemyHandCards.Add(CardC);
    }

    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        UIController.Instance.UpdateTurnTime(TurnTime);
        foreach (var card in PlayerFieldCards)
            {
                card.Info.HighlightCard(false);
            }

        if(IsPlayerTurn)
        {
            foreach (var card in PlayerFieldCards)
            {
                if(card.Card.Attack > 0 && !card.Card.Name.Contains("wall")){
                    card.Info.CanAttack = true;
                    card.Info.HighlightCard(true);
                    card.Ability.OnNewTurn();
                }
                
            }

            while(TurnTime-- > 0)
            {
                UIController.Instance.UpdateTurnTime(TurnTime);
                yield return new WaitForSeconds(1);
                CheckIfCardsPlayable();
            }

            ChangeTurn();
        }
        else
        {
            foreach (var card in EnemyFieldCards)
            {
                if(card.Card.Attack > 0 && !card.Card.Name.Contains("wall")) card.Info.CanAttack = true;
                card.Ability.OnNewTurn();
            }

            EnemyAI.MakeTurn();

            while(TurnTime-- > 0)
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
        UIController.Instance.DisableTurnButton();
        if(IsPlayerTurn)
        {
            CurrentGame.Player.RestoreRoundResoures();
            GiveNewCards();
            CheckIfCardsPlayable();
        } 
        else 
        {
            CurrentGame.Enemy.RestoreRoundResoures();
        }
        UIController.Instance.UpdateResources();

        StartCoroutine(TurnFunc());
    }

    void GiveNewCards()
    {
        GiveCardToHand(CurrentGame.EnemyDeck, EnemyHand);
        GiveCardToHand(CurrentGame.PlayerDeck, PlayerHand);
    }
    public void CardsFight(CardController attacker, CardController defender)
    {
        attacker.Info.CanAttack = false;
        defender.Card.GetDamage(attacker.Card.Attack);
        attacker.OnDamageDeal();
        defender.OnTakeDamage(attacker);

        if(!attacker.Card.Ranged) 
        {
            attacker.Card.GetDamage(defender.Card.Attack);
            attacker.OnTakeDamage();
        }

        attacker.CheckIfAlive();
        defender.CheckIfAlive();
        

        if(!attacker.Card.IsAlive && !IsPlayerTurn)
        {
            CurrentGame.Enemy.Gold++;
        }

        if(!defender.Card.IsAlive && IsPlayerTurn)
        {
            CurrentGame.Player.Gold++;
            
        }
        CheckIfCardsPlayable();
    }

    public void ReduceManaAndGold(bool isPlayer, int manacost, int goldcost)
    {
        if(isPlayer)
        {
            CurrentGame.Player.Mana = Mathf.Clamp(CurrentGame.Player.Mana - manacost,0,int.MaxValue);
            CurrentGame.Player.Gold = Mathf.Clamp(CurrentGame.Player.Gold - goldcost,0,int.MaxValue);
        }
        else
        {
            CurrentGame.Enemy.Mana = Mathf.Clamp(CurrentGame.Enemy.Mana - manacost,0,int.MaxValue);
            CurrentGame.Enemy.Gold = Mathf.Clamp(CurrentGame.Enemy.Gold - goldcost,0,int.MaxValue);
        }

        UIController.Instance.UpdateResources();
    }

    public void DamageHero(CardController card, bool isEnemyAttacked)
    {
        if(isEnemyAttacked) CurrentGame.Enemy.GetDamage(card.Card.Attack);
        else CurrentGame.Player.GetDamage(card.Card.Attack);
        card.OnDamageDeal();
        UIController.Instance.UpdateResources();
        CheckResult();
    }

    public void CheckResult()
    {
        if(CurrentGame.Enemy.HP <=0 || CurrentGame.Player.HP <= 0) 
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

        if(activeCard.IsSpell)
        {
            var spellCard = (SpellCard)activeCard;

            if(spellCard.SpellTarget == SpellCard.TargetType.NO_TARGET)
                targets = new List<CardController>();

            if(spellCard.SpellTarget == SpellCard.TargetType.ALLY_CARD_TARGET)
                targets = PlayerFieldCards;

            if(spellCard.SpellTarget == SpellCard.TargetType.ENEMY_CARD_TARGET)
                targets = EnemyFieldCards;
        }

        else
        {
            if(EnemyFieldCards.Exists(x => x.Card.IsProvocation))
            targets = EnemyFieldCards.FindAll(x => x.Card.IsProvocation);
            else targets = EnemyFieldCards;
        }




        foreach (var card in targets)
        {
            if(activeCard.IsSpell) card.Info.HighlightAsSpellTarget(highlight);
            else card.Info.HighlightAsTarget(highlight);
        }

        if((EnemyFieldCards.Count == 0 || activeCard.Ranged)&&!activeCard.IsSpell)
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

        GiveHandCards(CurrentGame.EnemyDeck, EnemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, PlayerHand);
        UIController.Instance.StartGame();
        UIController.Instance.UpdateResources();
        CheckIfCardsPlayable();

        StartCoroutine(TurnFunc());
    }
}





