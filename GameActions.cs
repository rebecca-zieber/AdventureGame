using System;
using System.Collections.Generic;
using LWTech.CSD228.TextMenus;

namespace AdventureGame
{
    public static class GameActions
    {
        public static void DrinkRiverWater(Player player)
        {
            player.HealDamage(4);
            Console.WriteLine("You drink some water from the river and feel refreshed.");
        }

        public static void TalkToTownGuard(Player player)
        {
            if (player.Bag.ContainsKey(GameItems.BookOfSpells)
                && player.Bag.ContainsKey(GameItems.TravelersVoucher)
                    && player.Bag.ContainsKey(GameItems.MarkOfTheShade))
            {
                
                Console.WriteLine("\"I see you have done a great service for the people of this town.\nI would like to offer you safe entry.\"");
                player.GoTo(Locale.Town);
            }
            else
            {
                Console.WriteLine("\"Hello there stranger. Sorry, but I cannot let a stranger enter the town without proof that you are trustworthy.\"");
            }
        }

        public static void GetGiftFromSalesman(Player player)
        {
            Console.WriteLine("Dejected, you wander away from the town, not sure what you'll do next.");
            Console.WriteLine();
            Console.WriteLine("\"Ah, I see you've been turned away by the Guard! Don't worry, he isn't heartless.\"");
            Console.WriteLine();
            Console.WriteLine("You hear a voice! You look up to see an old saleman speaking to you from across the road.");
            Console.WriteLine();
            Console.WriteLine("\"I hear tale of a great evil that plagues these lands. Townspeople have been lured from their homes by its wicked call,");    
            Console.WriteLine("left to wander the woods, never to be seen again. Perhaps if you find this evil, and its source of power,");  
            Console.WriteLine("the guard will look favorably upon you.\"");
            Console.WriteLine();
            Console.WriteLine("\"Here. let me give you something which might be convenient to you later. This is a one-way teleportation stone.");
            Console.WriteLine("Err -- I think that's what it is. I got bored last night and put a hex on it, could be toxic for all I know. But you look brave!");    
            Console.WriteLine("What's the worst that could happen?");  
            Console.WriteLine(); 

            Program.theWorld[player.Location].RemovePreAction();
            player.AddItem(GameItems.TeleportationStone);  
        }

        public static void TeleportToCrossroads(Player player)
        {
            player.GoTo(Locale.Crossroads);
            player.RemoveItem(GameItems.TeleportationStone);
        }

        public static void TalkToSalesman(Player player)
        {
            TextMenu<Player> menu = new TextMenu<Player>();

            bool hasEnoughMoney = false;
            foreach (Item item in Program.theWorld[player.Location].Resident.Belongings)
            {
                menu.AddItem(new TextMenuItem<Player>($"Buy {item.Name}\t${item.MonetaryValue}",
                (p) => { hasEnoughMoney = p.HasEnoughMoney(item);
                         if (hasEnoughMoney) {
                                p.BuyItem(item); 
                                Program.theWorld[player.Location].Resident.RemoveItem(item);
                                Console.WriteLine("\n\"It was a pleasure doing business with you.\"\n");
                         } else {
                                Console.WriteLine();
                                Console.WriteLine("\n\"You do not have enough coin for my product.\"\n");}}));
            }
            menu.AddItem(new TextMenuItem<Player>("Quit", (p)=>{return;}));

            Console.WriteLine($"\"Hello again, {player.Name}. I have wares, if you have coin.\"");
            Console.WriteLine();
            Console.WriteLine($"You have: ${player.GetFinances()}");

            int i = menu.GetMenuChoiceFromUser() - 1;
            menu.Run(i, player);
        }

        public static void FindForestItems(Player player)
        {
            player.Equip(GameItems.LeatherArmor);
            player.Equip(GameItems.RustyAxe);

            Console.WriteLine("You find some leather armor, and a rusty axe! You quickly put it all on before anyone else finds them.");

            Program.theWorld[player.Location].RemoveMenuItem("Search the woods nearby");
        }

        public static void FindBookOfSpells(Player player)
        {
            player.AddItem(GameItems.BookOfSpells);

            Console.WriteLine("You pick up the book of spells. It appears to be very old, and it is rather lightweight.");
            Console.WriteLine("You sense that this book has been used to do great harm to people in the past..."); 
            Console.WriteLine("Reading it, you gain insight on the workings of evil magic.");      

            Program.theWorld[player.Location].RemoveMenuItem("Search the meadow");
        }

        public static void GuideTravelerBackToTown(Player player)
        {
            player.AddItem(GameItems.TravelersVoucher);

            Console.WriteLine("You guide the traveler back to the town. Hopefully the guard will recognize them.");

            Program.theWorld[player.Location].RemoveMenuItem("Guide traveler back to town");
            Program.theWorld[player.Location].RemoveMenuItem("Start a fight");
            Program.theWorld[player.Location].RemoveResident();
            player.GoTo(Locale.Towngates);
        }

        private static void Fight(Player player, bool monsterStarts = false)
        {
            Actor opponent = Program.theWorld[player.Location].Resident;
            bool stillFighting = true;
            if (monsterStarts)
            {
                Console.WriteLine(Program.theWorld[player.Location].Description);
                Console.WriteLine(Program.theWorld[player.Location].ResidentDescription);
                Console.WriteLine();
                Console.WriteLine($"The {opponent.Name} goes in for the attack!");
                opponent.Attack(player);
                Console.WriteLine($"The {opponent.Name} attacks you with their {opponent.Weapon}! You now have {player.CurrentHealth} health points.");
                if (player.CurrentHealth <= 0)
                {
                    stillFighting = false;
                    Console.WriteLine("\nOh no! You are mortally wounded!  You are dead...");
                }
            }

            while (stillFighting)
            {
                Console.WriteLine();
                Console.WriteLine("You are engaged in combat! Choose an action.");

                TextMenu<Player> combatOptions = new TextMenu<Player>();
                combatOptions.AddItem(new TextMenuItem<Player>($"Attack with {player.Weapon}",
                        (p)=> {p.Attack(opponent); }));
                
                player.BuildAttackDescriptions();

                if (player.PlayStyle.Equals("Wizard"))
                {
                    combatOptions.AddItem(new TextMenuItem<Player>("Grab the opponent with your Shocking Grasp",
                        (p)=> {p.Attack(opponent, "Grab the opponent with your Shocking Grasp"); }));
                    combatOptions.AddItem(new TextMenuItem<Player>("Cast Ray of Frost on opponent",
                        (p)=> {p.Attack(opponent, "Cast Ray of Frost on opponent"); }));
                }
                else if (player.PlayStyle.Equals("Warrior"))
                {
                    combatOptions.AddItem(new TextMenuItem<Player>("Taunt the opponent",
                        (p)=> {p.Attack(opponent, "Taunt the opponent"); }));
                    combatOptions.AddItem(new TextMenuItem<Player>("Intimidate the opponent",
                        (p)=> {p.Attack(opponent, "Intimidate the opponent"); }));   
                }
                else if (player.PlayStyle.Equals("Rogue"))
                {
                    combatOptions.AddItem(new TextMenuItem<Player>("Launch sneak attack",
                        (p)=> {p.Attack(opponent, "Launch sneak attack"); }));
                    combatOptions.AddItem(new TextMenuItem<Player>("Play dead",
                        (p)=> {p.Attack(opponent, "Play dead"); }));
                }

                if (player.Bag.ContainsKey(GameItems.BookOfSpells))
                {
                    combatOptions.AddItem(new TextMenuItem<Player>("Cast 'Defend Against Evil'",
                        (p)=> {p.Attack(opponent, "Cast 'Defend Against Evil'"); }));
                }

                combatOptions.AddItem(new TextMenuItem<Player>("Drink a potion",
                    (p)=> {p.ChoosePotion(p);}));

                int choice = combatOptions.GetMenuChoiceFromUser() - 1;
                combatOptions.Run(choice, player);

                if (choice == combatOptions.Size() - 1)
                {
                    Console.WriteLine();
                    Console.WriteLine($"You drink a potion!");
                    Console.WriteLine();
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine(player);
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine();
                }

                Console.WriteLine($" The {opponent.Name} now has {opponent.CurrentHealth} health points.");

                if (opponent.CurrentHealth <= 0)
                {
                    stillFighting = false;

                    if (opponent is Monster)
                    {
                        Monster opponentMonster = (Monster)opponent;
                        foreach (Item item in opponentMonster.TakeLoot())
                        {
                            Console.WriteLine($"You loot the body and take what's rightfully yours. You collect a {item.Name}!");
                            Console.WriteLine($"It is worth ${item.MonetaryValue}");
                            Console.WriteLine();
                            player.AddItem(item);
                        }
                    }

                    Program.theWorld[player.Location].RemoveMenuItem("Start a fight");
                    Program.theWorld[player.Location].RemovePreAction();
                    Program.theWorld[player.Location].RemoveResident();
                    continue;
                }

                if (stillFighting && !monsterStarts)
                {
                    opponent.Attack(player);
                    Console.WriteLine();
                    Console.WriteLine($"The {opponent.Name} attacks you with their {opponent.Weapon}! You now have {player.CurrentHealth} health points.");
                    Console.WriteLine();
                    if (player.CurrentHealth <= 0)
                    {
                        Console.WriteLine("You died! Game over...");
                        stillFighting = false;
                    }
                }

                Console.WriteLine();
                Console.WriteLine("------------------------------------------");
                Console.WriteLine(player.ToString());
                Console.WriteLine("------------------------------------------");
                Console.WriteLine();

                if (stillFighting)
                {
                    string playerResponse;
                    Console.WriteLine("Do you want to keep fighting?\n1. Yes\n2. No");
                    Console.Write("Enter a number from 1 to 2: ");
                    playerResponse = Console.ReadLine();
                    Console.WriteLine();

                    if (monsterStarts == true)
                    {
                        monsterStarts = false;
                    }

                    if (playerResponse.Equals("2"))
                    {
                        Console.WriteLine();
                        Console.WriteLine("You beat a hasty retreat and live to fight another day.");
                        stillFighting = false;
                    }

                    else if (!playerResponse.Equals("1") && !playerResponse.Equals("2"))
                    {
                        bool isValid = false;
                        while (!isValid)
                        {
                            Console.WriteLine("Invalid input. Please type \"1\" for Yes, or \"2\" for No");
                            playerResponse = Console.ReadLine();

                            if (playerResponse.Equals("2"))
                            {
                                stillFighting = false;
                                isValid = true;
                            }
                            else if (playerResponse.Equals("1"))
                            {
                                isValid = true;
                            }
                        }
                    }
                }
            }
        }

        public static void PlayerAttacks(Player player)
        {
            GameActions.Fight(player);
        }

        public static void MonsterAttacks(Player player)
        {
            GameActions.Fight(player, true);
        }
    }
}