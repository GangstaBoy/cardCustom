
using UnityEngine;

public class Player
{
    public int HP, Mana, ManaRate, Gold, GoldRate;

    public Player(int hp = 20, int mp = 0, int gold = 20, int goldRate = 3, int manaRate = 2)
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
