using System;
using System.Collections.Generic;
using LWTech.CSD228.TextMenus;

namespace AdventureGame
{

    public interface IInteractable 
    {
        void Equip(Weapon weapon);
        void Equip(Armor armor);
        void AddItem(Item item);
        Item RemoveItem(Item item);
    }



    public class Actor : IInteractable
    {
        public string Name {get; private set;}
        public int MaxHealth {get; private set;}
        public int CurrentHealth {get; private set;}
        public Locale Location {get; private set;}
        public Armor Armor {get; private set;}
        public Weapon Weapon {get; private set;}
        public List<Item> Belongings {get; private set;}


        public Actor(string name, int health, Locale location = Locale.Nowhere)
        {
            if (name.Length == 0)
            {
                throw new System.ArgumentException("Error -- name cannot be empty");
            }
            else if (name == null)
            {
                throw new System.ArgumentNullException("Error -- name cannot be null");
            }
            if (health < 0)
            {
                throw new System.ArgumentOutOfRangeException($"Error -- cannot have negative health: {health}");
            }
            
            this.Name = name;
            this.MaxHealth = health;
            this.CurrentHealth = MaxHealth;
            this.Location = location;  
            Belongings = new List<Item>();
        }

        public void GoTo(Locale nextLocation)
        {
            this.Location = nextLocation;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                throw new System.ArgumentOutOfRangeException($"Error -- cannot take negative damage: {damage}");
            }

            if (this.CurrentHealth - damage < 0)
            {
                this.CurrentHealth = 0;
            }
            else 
            {
                this.CurrentHealth -= damage;
            }
        } 

        public void HealDamage(int heal)
        {
            if (heal < 0)
            {
                throw new System.ArgumentOutOfRangeException($"Error -- cannot heal negative damage: {heal}");
            }

            if (this.CurrentHealth + heal > this.MaxHealth)
            {
                this.CurrentHealth = this.MaxHealth;
            }
            else
            {
                this.CurrentHealth += heal;
            }
        }

        public void Equip(Armor armor)
        {
            if (armor != null)
            {
                this.Armor = armor;
            }
            else 
            {
                throw new System.ArgumentNullException("Error -- armor cannot be null");
            }
        }

        public void Equip(Weapon weapon)
        {
            if (weapon != null)
            {
                this.Weapon = weapon;
            }
            else
            {
                throw new System.ArgumentNullException("Error -- weapon cannot be null");
            }
        }

        public virtual void AddItem(Item item)
        {
            if (item != null)
            {
                this.Belongings.Add(item);
            }
            else
            {
                throw new System.ArgumentNullException("Error -- item cannot be null");
            }
        }

        public virtual Item RemoveItem(Item item)
        {
            if (item != null)
            {
                if (this.Belongings.Remove(item))
                {
                    return item;
                }
            }
            else 
            {
                throw new System.ArgumentNullException("Error -- cannot remove null item");
            }

            return null;
        }

        public virtual void Attack(Actor actor) // where the parameter is the defender
        {
            Random random = new Random();
            int randomDamage = random.Next(0, this.Weapon.MaxDamage+1);

            actor.Defend(randomDamage);
        }

        public virtual void Defend(int damage)
        {
            Random random = new Random();
            int randomDefense = random.Next(0, this.Armor.MaxProtection+1);

            int totalDamage = damage - randomDefense;
            if (totalDamage <= 0)
            {
                totalDamage = 1;
            }

            TakeDamage(totalDamage);
        }

        override
        public string ToString()
        {
            return $"{this.Name}@{this.Location}\tHealth:{this.CurrentHealth}";
        }

    }




    public class Player : Actor
    {
        public Dictionary<Item,int> Bag {get; private set;} // the use of a dictionary is redundant, should be a list -- tried to change, but weird errors kept happening
        public string PlayStyle {get; private set;}
        public Dictionary<string, string> AttackDescriptions {get; private set;}

        public Player(string name, int health = 20, Locale location = Locale.Nowhere) : base(name, health, location)
        {
            if (name.Length == 0)
            {
                throw new System.ArgumentException("Error -- name cannot be empty");
            }
            else if (name == null)
            {
                throw new System.ArgumentNullException("Error -- name cannot be null");
            }

            Bag = new Dictionary<Item, int>();
            this.Bag.Add(GameItems.RabbitsFoot, GameItems.RabbitsFoot.MonetaryValue);
            Equip(GameItems.ClothArmor);
            Equip(GameItems.Fists);
            this.PlayStyle = "";
            this.AttackDescriptions = new Dictionary<string, string>();
        }

        public void SetPlayStyle(string playStyle)
        {
            if (playStyle == null)
            {
                throw new System.ArgumentNullException("Error -- playstyle cannot be null");
            }
            else if (playStyle.Length == 0)
            {
                throw new System.ArgumentException("Error -- playStyle cannot be empty");
            }

            this.PlayStyle = playStyle;
        }

        public void BuildAttackDescriptions()
        {
            foreach(string description in AttackDescriptions.Keys)
            {
                AttackDescriptions.Remove(description);
            }

            if (this.PlayStyle.Equals("Wizard"))
            {
                AttackDescriptions.Add("Cast Ray of Frost on opponent", "A pillar of ice shoots from your fingertips, and spears the opponent!");
                AttackDescriptions.Add("Grab the opponent with your Shocking Grasp", "Lightning springs from your hands to deliver an electrical shot to the opponent!");
            }
            else if (this.PlayStyle.Equals("Warrior"))
            {
                List<String> insults = new List<String>();
                    insults.Add("\"If I gave you a penny for your thoughts, I'd get change!\"");
                    insults.Add("\"I refuse to have a battle of wits with an unarmed person.\"");
                    insults.Add("\"Anybody who told you to just be yourself gave you terrible advice.\"");
                    insults.Add("\"You call that a knife? THIS is a knife!\"");

                    Random insult = new Random();
                    int i = insult.Next(0, insults.Count);

                AttackDescriptions.Add("Taunt the opponent", insults[i]);
                AttackDescriptions.Add("Intimidate the opponent", "You give out a mighty roar, leaving the opponent quaking in their boots.");
            }
            else
            {
                AttackDescriptions.Add("Launch sneak attack", "You slither through the shadows like the sneaky sneak you are, attacking the {actor.Name} from behind!");
                AttackDescriptions.Add("Play dead", "Possums and grizzly bear encounters have taught you well.\nThe opponent foolishly saunters off, their guard down. You lunge!");
            }

            AttackDescriptions.Add("", $"You attack with your {this.Weapon}!");
            AttackDescriptions.Add("Cast 'Defend Against Evil'", "You stand brave in the face of Evil. You feel a great courage flow through you as you strike!");
        }

        public void Attack(Actor actor, String description = "") // where the actor is the defender (one you're attacking)
        {
            Random random = new Random();
            int randomDamage = random.Next(0, this.Weapon.MaxDamage+1);

            Console.Write($"\n{AttackDescriptions[description]}");  

            if (description.Equals("Cast 'Defend Against Evil'"))
            {
                randomDamage += 10;
            }

            actor.Defend(randomDamage);
        }

        public void ChoosePotion(Player player)
        {
            TextMenu<Player> potionChoices = new TextMenu<Player>();

            foreach (Item item in Bag.Keys)
            {
                if (item.Name.Contains("Potion"))
                {
                    try 
                    {
                        Potion thisItem = (Potion)item;
                        potionChoices.AddItem(new TextMenuItem<Player>($"{thisItem.Name}({thisItem.BuffStat})",
                            (p) => {p.DrinkPotion(thisItem, p);}));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            if (potionChoices.Size() == 0)
            {
                Console.WriteLine("You have no potions.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Which potion would you like to choose?");
            int choice = potionChoices.GetMenuChoiceFromUser() - 1;
            potionChoices.Run(choice, player);
        }
        
        public void DrinkPotion(Potion potion, Player player)
        {
            foreach (Item member in Bag.Keys)
            {
                if (member.Equals(potion))
                {
                    Bag.Remove(member);

                    if (potion.Name.Contains("Healing"))
                    {
                        HealDamage(potion.BuffStat);
                    }
                    else if (potion.Name.Contains("Armor"))
                    {
                        player.Armor.BuffArmor(potion.BuffStat);
                    }
                    else if (potion.Name.Contains("Damage"))
                    {
                        player.Weapon.BuffWeapon(potion.BuffStat);
                    }
                    else 
                    {
                        throw new System.ArgumentException($"Error -- the potion is not recognized: {potion}");
                    }
                }
            }
        }

        public override Item RemoveItem(Item item)
        {
            if (item != null)
            {
                if (this.Bag.Remove(item))
                {
                    return item;
                }
            }
            else 
            {
                throw new System.ArgumentNullException("Error -- cannot remove null item");
            }

            return null;
        }

        public bool At(Locale location)
        {
            if (location.Equals(this.Location))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void AddItem(Item item)
        {
            if (item != null)
            {
                Bag.Add(item, item.MonetaryValue);
            }
            else
            {
                throw new System.ArgumentNullException("Error -- item cannot be null");
            }
        }

        public int GetFinances()
        {
            int finances = 0;
            foreach (Item item in Bag.Keys)
            {
                if (item.Name.Contains("Potion"))
                {
                    continue;   // don't want to sell potions
                }

                finances += Bag[item];
            }
            return finances;
        }

        public bool HasEnoughMoney(Item item)
        {
            if (GetFinances() - item.MonetaryValue >= 0)
            {
                return true;
            }

            return false;
        }

        public int SpendMoney(int price) 
        {
            if (price < 0)
            {
                throw new System.ArgumentOutOfRangeException($"Error -- cannot spend negative money: {price}");
            }
            else if (GetFinances() - price < 0)
            {
                throw new System.ArgumentException("Error -- you do not have enough money to buy the item");
            }

            int runningTotal = 0;
            foreach (Item item in Bag.Keys)
            {
                if (Bag[item] == 0)
                {
                    continue;  // so as not to sell important in-game items
                }
                else if (item.Name.Contains("Potion"))
                {
                    continue;   // so as not to sell potions
                }

                if (runningTotal < price)
                {
                    runningTotal += Bag[item];
                    Bag.Remove(item);
                }
                if (runningTotal >= price)
                {
                    break;
                }
            }

            return runningTotal;
        }

        public void BuyItem(Item item)
        {
            if (item == null)
            {
                throw new System.ArgumentNullException("Error -- item cannot be null");
            }

            int amountSpentByPlayer = SpendMoney(item.MonetaryValue);
            AddItem(item);   

            if (amountSpentByPlayer > item.MonetaryValue)
            {
                AddItem(new Item("Cash Back", amountSpentByPlayer - item.MonetaryValue));
            }

            if (item is Weapon)
            {
                try 
                {
                    Weapon thisItem = (Weapon)item;
                    Equip(thisItem);
                    RemoveItem(thisItem);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if (item is Armor)
            {
                try
                {
                    Armor thisItem = (Armor)item;
                    Equip(thisItem);
                    RemoveItem(thisItem);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        override
        public string ToString()
        {
            string str = "";
            string comma = "";
            foreach (Item item in this.Bag.Keys)
            {
                if (item.Name.Contains("Potion"))
                {
                    try 
                    {
                        Potion potion = (Potion)item;
                        str += $"{comma}[{potion.Name}({potion.BuffStat})]";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    str += $"{comma}[{item.Name}(${item.MonetaryValue})]";
                    comma = ", ";
                }
            }

            return $"{this.Name} (@{this.Location})\nClass: {this.PlayStyle}\tHealth: {this.CurrentHealth}\nArmor: {this.Armor}\nWeapon: {this.Weapon}\nBag: {str}";
        }
    }





    public class Monster : Actor
    {
        public Monster(string name, int health, Locale location = Locale.Nowhere) : base(name, health, location)
        {
            if (name.Length == 0)
            {
                throw new System.ArgumentException("Error -- name cannot be empty");
            }
            else if (name == null)
            {
                throw new System.ArgumentNullException("Error -- name cannot be null");
            }
        }

        public List<Item> TakeLoot()
        {
            if (this.CurrentHealth == 0)
            {
                return this.Belongings;
            }
            else
            {
                throw new ArgumentException("Error -- can't take loot while opponant is still alive.");
            }
        }

        override
        public string ToString()
        {
            string str = "";
            string comma = "";
            foreach(Item item in this.Belongings)
            {
                str += $"{comma}{item}";
                comma = ", ";
            }

            return $"{this.Name}@{this.Location}\tHealth:{this.CurrentHealth}\nArmor: {this.Armor}\nWeapon: {this.Weapon}\nLoot: [{str}]";
        }
    }
}