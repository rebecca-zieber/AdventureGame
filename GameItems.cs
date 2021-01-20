using System;

namespace AdventureGame
{
    // Useful, pre-created Items, Armor, and Weapons that can be used anywhere in the game

    class GameItems
    {
        // Other item items go here ----------------------------------------
        public static readonly Item RabbitsFoot = new Item("Rabbit's Foot");
        public static readonly Item SilverRing = new Item("Silver Ring", 3);   
        public static readonly Item BookOfSpells = new Item("Book of Evil Spells");  
        public static readonly Item SilverBrooch = new Item("Silver Brooch", 3);
        public static readonly Item GoldDoubloon = new Item("Gold Doubloon", 5);
        public static readonly Item TravelersVoucher = new Item("Traveler's Voucher");
        public static readonly Item MarkOfTheShade = new Item("Mark of the Shade");
        public static readonly Item TeleportationStone = new Item("Teleportation Stone");
        public static readonly Potion HealingPotion1 = new Potion("Healing Potion", 2, 5);  // Want user to see total inventory of available items
        public static readonly Potion HealingPotion2 = new Potion("Healing Potion", 2, 5);
        public static readonly Potion HealingPotion3 = new Potion("Healing Potion", 2, 5);
        public static readonly Potion DamagePotion1 = new Potion("Damage Potion", 2, 5);
        public static readonly Potion DamagePotion2 = new Potion("Damage Potion", 2, 5);
        public static readonly Potion DamagePotion3 = new Potion("Damage Potion", 2, 5);
        public static readonly Potion ArmorPotion1 = new Potion("Armor Potion", 2, 5);
        public static readonly Potion ArmorPotion2 = new Potion ("Armor Potion", 2, 5);
        public static readonly Potion ArmorPotion3 = new Potion("Armor Potion", 2, 5);
        public static readonly Potion HealingPotion = new Potion("Healing Potion", 2, 5);
        public static readonly Potion DamagePotion = new Potion("Damage Potion", 2, 5);
        public static readonly Potion ArmorPotion = new Potion("Armor Potion", 2, 5);

        
        
        
        // Armor items go here ------------------------------------------
        public static readonly Armor Skin = new Armor("Goblin Skin", 1);
        public static readonly Armor Bones = new Armor("Bones", 2);
        public static readonly Armor DarkUmbra = new Armor("Dark Umbra", 5);
        public static readonly Armor ClothArmor = new Armor("Cloth Armor", 1);
        public static readonly Armor LeatherArmor = new Armor("Leather Armor", 4);
        public static readonly Armor ChainMailArmor = new Armor("Chain Mail Armor", 8, 5); // warrior
        public static readonly Armor PlateArmor = new Armor("Plate Mail", 12, 6); // warrior
        public static readonly Armor Camouflage = new Armor("Camouflage", 8, 5); // rogue
        public static readonly Armor StiffLeather = new Armor("Stiff Leather", 12, 6); // rogue
        public static readonly Armor Cloak = new Armor("Cloak", 8, 5); // wizard
        public static readonly Armor Robes = new Armor("Robes", 12, 6); // wizard
        public static readonly Armor GuardsArmor = new Armor("NPC Plot Armor", 1000);


        // Weapon items go here ----------------------------------------------
        public static readonly Weapon Fists = new Weapon("Fists", 1);
        public static readonly Weapon BoneyLimbs = new Weapon("Boney Limbs", 2);
        public static readonly Weapon EvilAura = new Weapon("Evil Aura", 10);
        public static readonly Weapon RustyAxe = new Weapon("Rusty Axe", 3);
        public static readonly Weapon LongSword = new Weapon("Long Sword", 8, 5); // warrior
        public static readonly Weapon Mace = new Weapon("Two-Handed Mace", 12, 6); // warrior
        public static readonly Weapon BowAndArrow = new Weapon("Bow and Arrows", 8, 5); // rogue
        public static readonly Weapon SetOfDaggers = new Weapon("Set of Daggers", 12, 6); // rogue
        public static readonly Weapon Wand = new Weapon("Wand", 8, 5); // wizard
        public static readonly Weapon Staff = new Weapon("Staff", 12, 6); // wizard
        public static readonly Weapon GuardsPike = new Weapon("NPC Plot Armor", 20);



    }
}