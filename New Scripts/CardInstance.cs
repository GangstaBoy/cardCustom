using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInstance : MonoBehaviour
{
    private BattleController _battleController;
    private GameCardSO _gameCardSO;
    private CardEffectSO[] _сardEffectSOs;
    private bool _instantiated = false;
    public GameCardSO GameCardSO { get => _gameCardSO; }
    public CardEffectSO[] СardEffectSOs { get => _сardEffectSOs; }
    private int _manacost, _staminacost;
    public int Manacost { get => _manacost; }
    public int Staminacost { get => _staminacost; }

    public void Instantiate(GameCardSO gameCardSO, BattleController battleController)
    {
        if (_instantiated) return;
        this._gameCardSO = gameCardSO;
        this._battleController = battleController;
        this._сardEffectSOs = gameCardSO.CardEffectSOs;
        this._instantiated = true;
        this._staminacost = gameCardSO.Staminacost;
        this._manacost = gameCardSO.Manacost;
    }

    private void Play()
    {
        //_battleController.PlayCard(this);
    }
}
