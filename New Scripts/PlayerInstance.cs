using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInstance : MonoBehaviour
{
    [SerializeField] private PlayerUI playerUI;
    private BattleController _battleController;
    private bool _instantiated = false;
    private int _health, _stamina, _mana, _maxStamina;
    private int _cardsDrawPerTurn;

    public int Health { get => _health; }
    public int Stamina { get => _stamina; }
    public int Mana { get => _mana; }
    public int CardsDrawPerTurn { get => _cardsDrawPerTurn; }
    public int MaxStamina { get => _maxStamina; }

    public void Instantiate(int health, int mana, int stamina, BattleController battleController, int cardsDrawPerTurn = 3)
    {
        if (_instantiated) return;
        this._battleController = battleController;
        this._instantiated = true;
        this._health = health;
        this._mana = mana;
        this._stamina = this._maxStamina = stamina;
        this._cardsDrawPerTurn = cardsDrawPerTurn;
    }

    public void getDamage(int damage, string type)
    {
        if (damage > 0)
        {
            _health -= damage;
            if (Health <= 0) Destroy(this.gameObject);
            else playerUI.UpdateHealth(Health);
        }
    }

    public void decreaseResources(int mana = 0, int stamina = 0)      //todo: make better
    {
        this._mana -= mana;
        this._stamina -= stamina;
    }

    public void increaseResources(int mana = 0, int stamina = 0)      //todo: make better
    {
        this._mana += mana;
        this._stamina += stamina;
    }
    public void refreshStamina()
    {
        this._stamina = this._maxStamina;
    }

    public void increaseHealth(int health)
    {
        if (health <= 0) return;
        else this._health += health;
    }
}
