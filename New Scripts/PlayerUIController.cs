using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private Image _logo;
    [SerializeField] private TextMeshProUGUI _name, _health, _mana, _stamina;
    private PlayerInstance _playerInstance;

    public void Instantiate(PlayerInstance playerInstance, EnemySO enemySO = null)
    {
        _playerInstance = playerInstance;
        if (enemySO == null) return;
        _logo.sprite = enemySO.Artwork;
        _logo.preserveAspect = true;
        _name.text = enemySO.Name;
    }
    void Update()
    {
        Display();
    }
    public void Display()
    {
        _health.text = _playerInstance.Health.ToString();
        _mana.text = _playerInstance.Mana.ToString();
        _stamina.text = _playerInstance.Stamina.ToString();
    }
    public void UpdateHealth(int health)
    {
        if (health > 0) _health.text = health.ToString();
    }
}
