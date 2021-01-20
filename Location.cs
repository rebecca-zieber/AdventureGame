using System;
using System.Collections.Generic;
using LWTech.CSD228.TextMenus;

namespace AdventureGame
{
    public enum Direction
    {
        North, South, East, West
    }

    public enum Locale
    {
        Town, Towngates, Crossroads, River, Bridge, Woods, Nowhere, Meadow, Thicket, Valley, CollosalCave, Cavern
    }


    public class Location 
    {
        public Locale CurrentLocation {get; private set;}
        public string Description {get; private set;}
        public Dictionary<Locale, Direction> Pathways {get; private set;}
        public Actor Resident {get; private set;}
        public string ResidentDescription {get; private set;}
        public List<TextMenuItem<Player>> MenuItems {get; private set;}
        public Action<Player> PreAction {get; private set;}


        public Location(Locale location, string description, Actor resident = null, string residentDescription = null, Action<Player> preaction = null)
        {
            this.CurrentLocation = location;
            Pathways = new Dictionary<Locale, Direction>();
            MenuItems = new List<TextMenuItem<Player>>();
            
            if (description.Length == 0)
            {
                throw new System.ArgumentException("Error -- description cannot be empty");
            }
            else if (description == null)
            {
                throw new System.ArgumentNullException("Error -- description cannot be null");
            }

            this.Description = description;

            if (resident != null)
            {
                this.Resident = resident;
                this.ResidentDescription = residentDescription;
            }

            if (preaction != null)
            {
                this.PreAction = preaction;
                
            }
        }  

        public void AddPreAction(Action<Player> preaction)
        {
            if (preaction == null)
            {
                return;
            }

            this.PreAction = preaction;
        }

        public void RemovePreAction()
        {
            this.PreAction = null;
        }

        public void RemoveResident()
        {
            this.Resident = null;
            this.ResidentDescription = "";
        }

        public void RunPreAction(Player player)
        {
            if (this.PreAction == null)
            {
                return;
            }

            PreAction(player);
        }

        public void MakePath(Direction direction, Locale location)
        {
            Pathways.Add(location, direction);
        }

        public void AddMenuItem(TextMenuItem<Player> item)
        {
            if (item == null)
            {
                throw new System.ArgumentNullException("Error -- cannot add null items");
            }
            this.MenuItems.Add(item);
        }

        public void RemoveMenuItem(String menuItemDescription)
        {
            if (menuItemDescription == null)
            {
                throw new System.ArgumentNullException("Error -- cannot remove null items");
            }
            
            foreach (TextMenuItem<Player> menuItem in MenuItems)
            {
                if (menuItem.Description == menuItemDescription)
                {
                    MenuItems.Remove(menuItem);
                    return;
                }
            }

            throw new System.ArgumentException($"Error -- the description of this menu item could not be found:\n{menuItemDescription}");

        }

        public TextMenu<Player> GetMenu()
        {
            TextMenu<Player> menu = new TextMenu<Player>();

            foreach (Locale nextLocation in Pathways.Keys)
            {
                menu.AddItem(new TextMenuItem<Player>($"Go {Pathways[nextLocation]}", 
                (p)=>{ p.GoTo(nextLocation); }));
            }
            foreach (TextMenuItem<Player> menuItem in MenuItems)
            {
                menu.AddItem(menuItem);
            }


            return menu;
        }
        
        override
        public string ToString()
        {
            string str = "";
            if (this.Resident != null)
            {
                str = this.ResidentDescription;
            }
            return $"{this.Description}\n{str}";
        }
    }
}