using System;
using static System.Console;
using System.Collections.Generic;
using LWTech.CSD228.TextMenus;

namespace AdventureGame
{
    class Program
    {
        public static Dictionary<Locale, Location> theWorld = new Dictionary<Locale, Location>();  
        static void Main(string[] args)
        {
            WriteLine("Adventure Game           -- Rebecca Zieber");
            WriteLine("==========================================");

            List<string> surnames = new List<string>();
            surnames.Add(" The Maloderous");
            surnames.Add(" The Self-Rightous");
            surnames.Add(" The Ill-Tempered");
            surnames.Add(" The Big Nerd");
            surnames.Add(", The One Who's Still Alive");
            surnames.Add(" The Trickster");
            surnames.Add(" The Simple");

            string playerName;
            WriteLine("Enter a name for your hero:");
            playerName = Console.ReadLine();
            if (playerName == null)
            {
                throw new ArgumentNullException("Error -- must enter a player name");
            }
            else if (playerName.Length == 0)
            {
                throw new ArgumentException("Error -- must enter a player name");
            }

            Random random = new Random();
            int playerSurname = random.Next(0, surnames.Count);
            playerName += surnames[playerSurname];
            Player ourHero = new Player(playerName, 20);

            WriteLine($"Welcome to Adventure, {playerName}!");
            WriteLine("==========================================");

            List<string> playerClasses = new List<string>();
            playerClasses.Add("Wizard");
            playerClasses.Add("Warrior");
            playerClasses.Add("Rogue");

            TextMenu<Player> playerClassMenu = new TextMenu<Player>();
            foreach (String playerClass in playerClasses)
            {
                playerClassMenu.AddItem(new TextMenuItem<Player>($"{playerClass}",
                    (p)=> {p.SetPlayStyle(playerClass);}));
            }

            WriteLine("What kind of class would you like to play?");
            int choice = playerClassMenu.GetMenuChoiceFromUser() - 1;
            playerClassMenu.Run(choice, ourHero);

            ourHero.AddItem(GameItems.HealingPotion);
            ourHero.AddItem(GameItems.DamagePotion);
            ourHero.AddItem(GameItems.ArmorPotion);

            Actor guard = new Actor("Guard", 1000);
            guard.Equip(GameItems.GuardsArmor);
            guard.Equip(GameItems.GuardsPike);

            Actor salesman = new Actor("Traveling Salesman", 1000);
            salesman.Equip(GameItems.GuardsArmor);
            salesman.Equip(GameItems.GuardsPike);
            if (ourHero.PlayStyle.Equals("Warrior"))
            {
                salesman.AddItem(GameItems.PlateArmor);
                salesman.AddItem(GameItems.ChainMailArmor);
                salesman.AddItem(GameItems.LongSword);
                salesman.AddItem(GameItems.Mace);
            }
            else if (ourHero.PlayStyle.Equals("Wizard"))
            {
                salesman.AddItem(GameItems.Robes);
                salesman.AddItem(GameItems.Cloak);
                salesman.AddItem(GameItems.Wand);
                salesman.AddItem(GameItems.Staff);
            }
            else if (ourHero.PlayStyle.Equals("Rogue"))
            {
                salesman.AddItem(GameItems.StiffLeather);
                salesman.AddItem(GameItems.Camouflage);
                salesman.AddItem(GameItems.SetOfDaggers);
                salesman.AddItem(GameItems.BowAndArrow);
            }
            salesman.AddItem(GameItems.ArmorPotion1);
            salesman.AddItem(GameItems.ArmorPotion2);
            salesman.AddItem(GameItems.ArmorPotion3);
            salesman.AddItem(GameItems.HealingPotion1);
            salesman.AddItem(GameItems.HealingPotion2);
            salesman.AddItem(GameItems.HealingPotion3);
            salesman.AddItem(GameItems.DamagePotion1);
            salesman.AddItem(GameItems.DamagePotion2);
            salesman.AddItem(GameItems.DamagePotion3);

            Actor traveler = new Actor("Lost Traveler", 1000);
            traveler.Equip(GameItems.GuardsArmor);
            traveler.Equip(GameItems.GuardsPike);

            Monster goblin = new Monster("Goblin", 20);
            goblin.Equip(GameItems.Skin);
            goblin.Equip(GameItems.Fists);
            goblin.AddItem(GameItems.SilverRing);

            Monster zombie = new Monster("Zombie", 20);
            zombie.Equip(GameItems.Bones);
            zombie.Equip(GameItems.BoneyLimbs);
            zombie.AddItem(GameItems.SilverBrooch);

            Monster shade = new Monster("Shade", 50);
            shade.Equip(GameItems.DarkUmbra);
            shade.Equip(GameItems.EvilAura);
            shade.AddItem(GameItems.MarkOfTheShade);

            


            Location townGates = new Location(Locale.Towngates, "You are at the gates of a town.", guard, "A guard stands before you.");
            townGates.MakePath(Direction.North, Locale.Crossroads); 
            theWorld.Add(Locale.Towngates, townGates); 
            guard.GoTo(Locale.Towngates);
            theWorld[Locale.Towngates].AddMenuItem(new TextMenuItem<Player>("Talk to the guard", (p)=> {GameActions.TalkToTownGuard(ourHero);}));
            theWorld[Locale.Towngates].AddMenuItem(new TextMenuItem<Player>("Start a fight", (p)=> {GameActions.PlayerAttacks(ourHero);}));

            Location town = new Location(Locale.Town, "You Win!");
            theWorld.Add(Locale.Town, town);

            Location crossroads = new Location(Locale.Crossroads, "You are at a 4-way crossroads. You cannot see what lies in any direction.",
                                                    salesman, "A traveling salesman seems to have set up camp nearby.", (p)=>{GameActions.GetGiftFromSalesman(ourHero);});
            crossroads.MakePath(Direction.North, Locale.River);
            crossroads.MakePath(Direction.South, Locale.Towngates);
            crossroads.MakePath(Direction.East, Locale.Bridge);
            crossroads.MakePath(Direction.West, Locale.Woods);
            salesman.GoTo(Locale.Crossroads);
            theWorld.Add(Locale.Crossroads, crossroads);
            theWorld[Locale.Crossroads].AddMenuItem(new TextMenuItem<Player>("Talk to the salesman", (p)=> {GameActions.TalkToSalesman(ourHero);}));
            theWorld[Locale.Crossroads].AddMenuItem(new TextMenuItem<Player>("Start a fight", (p)=> {GameActions.PlayerAttacks(ourHero);}));

            Location river = new Location(Locale.River, "You are at a swift-flowing, broad river that cannot be crossed.\nIn the distance, you see what appears to be a collosal cave.");
            river.MakePath(Direction.South, Locale.Crossroads);
            theWorld.Add(Locale.River, river);
            theWorld[Locale.River].AddMenuItem(new TextMenuItem<Player>("Drink from the river", (p)=>{GameActions.DrinkRiverWater(ourHero);}));

            Location woods = new Location(Locale.Woods, "You are in a dark, forboding forest. You are surrounded by tall trees, and fog clouds the way.");
            woods.MakePath(Direction.North, Locale.Thicket);
            woods.MakePath(Direction.East, Locale.Crossroads);
            theWorld.Add(Locale.Woods, woods);
            theWorld[Locale.Woods].AddMenuItem(new TextMenuItem<Player>("Search the woods nearby", (p)=>{GameActions.FindForestItems(ourHero);}));

            Location thicket = new Location(Locale.Thicket, "You stand in a quiet thicket. Brambles tower up to your shoulders.",
                                                    traveler, "A weary traveler wanders around on the path, bewildered in the mist.");
            thicket.MakePath(Direction.North, Locale.Valley);
            thicket.MakePath(Direction.South, Locale.Woods);
            theWorld.Add(Locale.Thicket, thicket);
            traveler.GoTo(Locale.Thicket);
            theWorld[Locale.Thicket].AddMenuItem(new TextMenuItem<Player>("Guide traveler back to town", (p)=>{GameActions.GuideTravelerBackToTown(ourHero);}));
            theWorld[Locale.Thicket].AddMenuItem(new TextMenuItem<Player>("Start a fight", (p)=> {GameActions.PlayerAttacks(ourHero);}));

            Location valley = new Location(Locale.Valley, "You are at the base of a broad valley. Tall grass surrounds you,\nand you can see the path stretching ahead in both directions.",
                                                    zombie, "A Zombie stumbles around in front of you.", (p)=>{GameActions.MonsterAttacks(ourHero);});
            valley.MakePath(Direction.South, Locale.Thicket);
            valley.MakePath(Direction.East, Locale.CollosalCave);
            theWorld.Add(Locale.Valley, valley);
            zombie.GoTo(Locale.Valley);
            theWorld[Locale.Valley].AddMenuItem(new TextMenuItem<Player>("Start a fight", (p)=> {GameActions.PlayerAttacks(ourHero);}));

            Location cave = new Location(Locale.CollosalCave, "You stand in front of the mouth of a collosal cave. \nIt is impossible to see inside. To the south you see the river.\nYou feel a very dark, evil presence inside the cave...");
            cave.MakePath(Direction.East, Locale.Cavern);
            cave.MakePath(Direction.West, Locale.Valley);
            theWorld.Add(Locale.CollosalCave, cave);

            Location cavern = new Location(Locale.Cavern, "You are in an echoing cavern deep within the cave. \nYou cannot see. Your footsteps echo off the stone walls.",
                                                    shade, "You feel the presence of an evil Shade rush out of the depths to meet you.", (p)=>{GameActions.MonsterAttacks(ourHero);});
            cavern.MakePath(Direction.West, Locale.CollosalCave);
            theWorld.Add(Locale.Cavern, cavern);
            shade.GoTo(Locale.Cavern);
            theWorld[Locale.Cavern].AddMenuItem(new TextMenuItem<Player>("Start a fight", (p)=> {GameActions.PlayerAttacks(ourHero);}));

            Location bridge = new Location(Locale.Bridge, "You come up to a bridge.", goblin, "A Goblin is standing nearby.", (p)=>{GameActions.MonsterAttacks(ourHero);});
            bridge.MakePath(Direction.East, Locale.Meadow);
            bridge.MakePath(Direction.West, Locale.Crossroads);
            theWorld.Add(Locale.Bridge, bridge);
            goblin.GoTo(Locale.Bridge);
            theWorld[Locale.Bridge].AddMenuItem(new TextMenuItem<Player>("Start a fight", (p)=> {GameActions.PlayerAttacks(ourHero);}));
            
            Location meadow = new Location(Locale.Meadow, "You are in a sunny meadow. Wildflowers color the landscape.");
            meadow.MakePath(Direction.West, Locale.Bridge);
            theWorld.Add(Locale.Meadow, meadow);
            theWorld[Locale.Meadow].AddMenuItem(new TextMenuItem<Player>("Search the meadow", (p)=>{GameActions.FindBookOfSpells(ourHero);}));

            ourHero.GoTo(Locale.Towngates);


            // -----------------------------------------------------------------------------


            bool done = false; 
            while(!done)
            {
                Location location = new Location(Locale.Nowhere, "Para continuar en enspanol, oprima dos.");
                foreach (Locale locale in theWorld.Keys)
                {
                    if (locale.Equals(ourHero.Location))
                    {
                        location = theWorld[locale];
                        break;
                    }
                }

                location.RunPreAction(ourHero);
                if (ourHero.CurrentHealth <= 0)
                {
                    done = true;
                    WriteLine("You died! Game over...");
                    continue;
                }

                TextMenu<Player> menu = location.GetMenu();

                if (ourHero.Bag.ContainsKey(GameItems.TeleportationStone))
                {
                    menu.AddItem(new TextMenuItem<Player>("Teleport back to the Crossroads",
                        (p)=>{GameActions.TeleportToCrossroads(ourHero);}));
                }

                WriteLine("------------------------------------------");
                WriteLine(ourHero);
                WriteLine("------------------------------------------");
                WriteLine();

                WriteLine(location.ToString());
                WriteLine();
                WriteLine("What would you like to do?");
                int i = menu.GetMenuChoiceFromUser() - 1;

                WriteLine();

                menu.Run(i, ourHero);
                WriteLine();

                if (ourHero.CurrentHealth <= 0 || ourHero.Location.Equals(Locale.Town))
                {
                    done = true;
                }
            }


        }
    }
}
