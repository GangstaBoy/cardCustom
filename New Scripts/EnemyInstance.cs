using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyInstance : MonoBehaviour
{
    [SerializeField] private EnemyUI enemyUI;
    private BattleController _battleController;
    private bool _instantiated = false;
    private int _health, _stamina, _mana, _maxStamina;

    public int Health { get => _health; }
    public int Stamina { get => _stamina; }
    public int Mana { get => _mana; }
    public int MaxStamina { get => _maxStamina; }


    public void Instantiate(EnemySO enemySO, BattleController battleController)
    {
        if (_instantiated) return;
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
            if (Health <= 0) Destroy(this.gameObject);
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
}
