using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum BuffCategory
{
    BUFF,
    DEBUFF,
    HERO_BUFF,
    HERO_DEBUFF
}

public enum BuffType
{
    ARMOR,
    DAMAGE_INCREASE,
    MAX_DEFENSE_INCREASE,
    HOLY_SHIELD,
    PROVOCATION,
    DOUBLE_ATTACK,
    SELF_HP_REGENERATION,
    NEARBY_ALLIES_REGENERATION,
    ALL_ALLIES_REGENERATION,
    FIRE_SHIELD,
    ARMOR_PACKAGE,
    SKELETON_SUMMONER,
    MANA_FLARE,
    GOLD_INCOME,
    BUFF_DAMAGE,
    WOLF_AURA,
    WOLF_AURA_CAST
}

public class Buff : MonoBehaviour
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
