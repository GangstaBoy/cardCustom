
using UnityEngine;

public class Player
{
    public int HP, Mana, ManaRate, Gold, GoldRate;

    public Player()
    {
        HP = 20;
        Mana = 0;
        ManaRate = 1;
        GoldRate = 3;
        Gold = 20;
    }

    public void RestoreRoundResoures()
    {
        Mana += ManaRate;
        Gold += GoldRate;
    }

    public void GetDamage(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, int.MaxValue);
    }
}
