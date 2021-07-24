
using UnityEngine;

public class Player
{
    public int HP, Mana, ManaRate, Gold, GoldRate;

    public Player(int hp = 20, int mp = 5, int gold = 20, int goldRate = 2, int manaRate = 1)
    {
        HP = hp;
        Mana = mp;
        ManaRate = manaRate;
        GoldRate = goldRate;
        Gold = gold;
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
