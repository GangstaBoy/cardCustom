using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public enum HighlighterTargetType
    {
        SPELL_TARGET_ALLY,
        SPELL_TARGET_ENEMY,
        ATTACK_TARGET,
        NO_TARGET
    }
    [SerializeField] private Image _logo;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _name, _attack, _health, _manacost, _goldcost;

    [SerializeField] private GameObject _cover, _shield, _provocation;
    [SerializeField] private Color _basic, _attackTarget, _spellTargetAlly, _spellTargetEnemy, _active;
    [SerializeField] private CardSO _card;
    private bool _isShown;
    private bool _isActive;

    public CardDisplay(CardSO card)
    {
        _card = card;
        _isShown = false;
        _isActive = false;
    }
    public void Initialize(CardSO card)
    {
        if (_card != null) return;
        _card = card;
        _isShown = false;
        _isActive = false;
        Display();
    }
    public void ShowCard()
    {
        this._isShown = true;
        this._cover.SetActive(false);
    }
    public void HideCard()
    {
        this._isShown = false;
        this._cover.SetActive(true);
    }
    public void SetActive()
    {
        if (this._isShown && !this._isActive)
        {
            this._isActive = true;
            _background.color = _active;
        }
    }
    public void SetInactive()
    {
        if (this._isShown && this._isActive)
        {
            this._isActive = false;
            _background.color = _basic;
        }
    }
    public void HighlightAsTarget(HighlighterTargetType highlighterTargetType = HighlighterTargetType.NO_TARGET)
    {
        if (!this._isShown) return;
        switch (highlighterTargetType)
        {
            case HighlighterTargetType.ATTACK_TARGET:
                _background.color = _attackTarget;
                break;
            case HighlighterTargetType.SPELL_TARGET_ALLY:
                _background.color = _spellTargetAlly;
                break;
            case HighlighterTargetType.SPELL_TARGET_ENEMY:
                _background.color = _spellTargetEnemy;
                break;
            case HighlighterTargetType.NO_TARGET:
                _background.color = _basic;
                break;
            default:
                break;
        }
    }
    public void ChangeTransparency(bool increaseTransparency = false)
    {
        _canvasGroup.alpha = increaseTransparency ? .5f : 1;
    }
    public void UpdateAttack(int attack)
    {
        _attack.text = attack.ToString();
    }
    public void UpdateHealth(int health)
    {
        _health.text = health.ToString();
    }
    public void HideAttackAndHealth()
    {
        _attack.gameObject.SetActive(false);
        _health.gameObject.SetActive(false);
    }
    private void Display()
    {
        if (_card == null) return;
        _logo.sprite = _card.Artwork;
        _logo.preserveAspect = true;
        _name.text = _card.Name;
        _attack.text = _card.Attack.ToString();
        _health.text = _card.MaxHealth.ToString();
        _manacost.text = _card.Manacost.ToString();
        _goldcost.text = _card.Goldcost.ToString();
    }
    private void Start()
    {
        Display();
    }
}
