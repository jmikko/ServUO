namespace Server.Items
{
    // Talisman "slayer" classification. The bonus-damage mechanic these drive has no D&D
    // equivalent and is permanently inert (see SlayerGroup.cs and BaseWeapon.CheckTalismanSlayer),
    // but the enum itself is still referenced by talisman/weapon item definitions and by the loot
    // property tables, so the identity values are preserved.
    //
    // Member order is load-bearing: tooltip code derives clilocs arithmetically as
    // 1072503 + (int)value for the Bear..Bovine range, so those must stay at 1..9.
    public enum TalismanSlayerName
    {
        None = 0,
        Bear = 1,
        Vermin = 2,
        Bat = 3,
        Mage = 4,
        Beetle = 5,
        Bird = 6,
        Ice = 7,
        Flame = 8,
        Bovine = 9,
        Wolf = 10,
        Goblin = 11,
        Undead = 12,
        Repond = 13,
        Elemental = 14,
        Demon = 15,
        Arachnid = 16,
        Reptile = 17,
        Fey = 18
    }
}
