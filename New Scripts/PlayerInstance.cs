using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum OpponentType
{
    PLAYER,
    ENEMY
}

public class PlayerInstance : MonoBehaviour
{
    public static event System.EventHandler<PlayerDiedArgs> PlayerDied;
    public class PlayerDiedArgs : System.EventArgs
    {
        public PlayerInstance diedInstance;
    }
    private OpponentType _opponentType;
    private BattleController _battleController;
    private bool _instantiated = false;
    private int _health, _stamina, _mana, _maxStamina;
    public int Health { get => _health; }
    public int Stamina { get => _stamina; }
    public int Mana { get => _mana; }
    public int MaxStamina { get => _maxStamina; }
    public OpponentType OpponentType { get => _opponentType; }

    public void Instantiate(BattleController battleController, OpponentType opponentType, int health, int mana, int stamina)
    {
        //if (_instantiated) return;
        this._opponentType = opponentType;
        this._battleController = battleController;
        this._instantiated = true;
        this._health = health;
        this._mana = mana;
        this._stamina = this._maxStamina = stamina;
    }
    public void Instantiate(BattleController battleController, OpponentType opponentType, EnemySO enemySO)
    {
        //if (_instantiated) return;
        this._opponentType = opponentType;
        this._battleController = battleController;
        this._instantiated = true;
        this._health = enemySO.Health;
        this._mana = enemySO.Mana;
        this._stamina = this._maxStamina = enemySO.Stamina;
    }

    public void getDamage(int damage, string type)
    {
        if (damage > 0)
        {
            _health -= damage;
            if (Health <= 0)
            {
                //Destroy(this.gameObject);
                if (PlayerDied != null) PlayerDied.Invoke(this, new PlayerDiedArgs { diedInstance = this });
            }
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
