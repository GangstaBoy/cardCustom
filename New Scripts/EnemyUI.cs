using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUI : MonoBehaviour
{
    //[SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _logo;
    [SerializeField] private TextMeshProUGUI _name, _health, _mana, _stamina;
    private EnemyInstance _enemyInstance;

    void Update()
    {
        if (_enemyInstance == null) return;
        if (_enemyInstance.Health > 0) _health.text = _enemyInstance.Health.ToString();
        else _health.text = "0";
        if (_enemyInstance.Mana > 0) _mana.text = _enemyInstance.Mana.ToString();
        else _mana.text = "0";
        if (_enemyInstance.Stamina > 0) _stamina.text = _enemyInstance.Stamina.ToString();
        else _stamina.text = "0";
    }
    public void Display(EnemySO enemySO, EnemyInstance enemyInstance)
    {
        if (enemySO == null) return;
        _enemyInstance = enemyInstance;
        _logo.sprite = enemySO.Artwork;
        _logo.preserveAspect = true;
        _name.text = enemySO.Name;
    }
}
