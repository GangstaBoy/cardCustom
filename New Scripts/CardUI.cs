using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    //[SerializeField] private CanvasGroup _canvasGroup;
    public Color attackColor, actionColor, spellColor, artifactColor; //todo: move to singleton
    public GameCardSO gameCardSO;
    [SerializeField] private Image _logo, _background;
    [SerializeField] private TextMeshProUGUI _name, _description, _manacost, _staminacost;
    public void Display(GameCardSO gameCardSO)
    {
        if (gameCardSO == null) return;
        _logo.sprite = gameCardSO.Artwork;
        _logo.preserveAspect = true;
        _name.text = gameCardSO.Name;
        _description.text = gameCardSO.Description;
        if (gameCardSO.Manacost == 0) _manacost.text = "";
        else _manacost.text = gameCardSO.Manacost.ToString();
        if (gameCardSO.Staminacost == 0) _staminacost.text = "";
        else _staminacost.text = gameCardSO.Staminacost.ToString();

        SetCardColor(gameCardSO);
    }
    private void SetCardColor(GameCardSO gameCardSO)
    {
        switch (gameCardSO.Type)
        {
            case GameCardSO.GameCardType.ACTION:
                {
                    _background.color = actionColor;
                    break;
                }
            case GameCardSO.GameCardType.ARTIFACT:
                {
                    _background.color = artifactColor;
                    break;
                }
            case GameCardSO.GameCardType.ATTACK:
                {
                    _background.color = attackColor;
                    break;
                }
            case GameCardSO.GameCardType.SPELL:
                {
                    _background.color = spellColor;
                    break;
                }
            default: break;
        }
    }

    void Awake()
    {
        if (gameCardSO == null) return;
        Display(gameCardSO);
    }

}
