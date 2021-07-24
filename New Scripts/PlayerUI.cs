using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public PlayerInstance playerInstance;
    void Update()
    {
        Display(playerInstance);
    }
    [SerializeField] private TextMeshProUGUI _health, _mana, _stamina;
    public void Display(PlayerInstance playerInstance)
    {
        if (playerInstance == null) return;
        _health.text = playerInstance.Health.ToString();
        _mana.text = playerInstance.Mana.ToString();
        _stamina.text = playerInstance.Stamina.ToString();
    }

    public void UpdateHealth(int health)
    {
        if (health > 0) _health.text = health.ToString();
    }
}
