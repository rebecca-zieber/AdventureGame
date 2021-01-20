namespace AdventureGame
{
    
    public class Item 
    {
        public string Name {get; private set;}
        public int MonetaryValue {get; private set;}

        public Item(string name, int monetaryValue = 0) 
        {
            if (name == null)
            {
                throw new System.ArgumentNullException("Error -- name cannot be null");
            }
            else if (name.Length == 0)
            {
                throw new System.ArgumentException("Error -- name cannot be empty");
            }

            this.Name = name;

            if (monetaryValue != 0)
            {
                if (monetaryValue < 0)
                {
                    throw new System.ArgumentOutOfRangeException($"Error -- item cannot have negative monetary value: {monetaryValue}");
                }

                this.MonetaryValue = monetaryValue;
            }
        }

        public bool HasMonetaryValue()
        {
            if (this.MonetaryValue != 0)
            {
                return true;
            }

            return false;
        }

        override
        public string ToString() 
        {
            return $"{this.Name}(${this.MonetaryValue})";
        }
    }

    public class Potion : Item
    {
        public int BuffStat {get; private set;}

        public Potion(string name, int monetaryValue, int statBuff=0) : base(name, monetaryValue)
        {
            if (name == null)
            {
                throw new System.ArgumentNullException("Error -- name cannot be null");
            }
            else if (monetaryValue < 0)
            {
                throw new System.ArgumentOutOfRangeException($"Error -- monetary value cannot be negative: {monetaryValue}");
            }
            else if (statBuff < 0)
            {
                throw new System.ArgumentOutOfRangeException($"Error -- cannot buff stats with a negative number: {statBuff}");
            }

            this.BuffStat = statBuff;
        }

        override
        public string ToString()
        {
            return $"{this.Name}({this.BuffStat})\t(${this.MonetaryValue})";
        }
    }

        public class Armor : Item
        {
            public int MaxProtection {get; private set;}

            public Armor(string name, int maxProtection, int monetaryValue = 0) : base(name, monetaryValue)
            {
                if (maxProtection < 0) 
                {
                    throw new System.ArgumentOutOfRangeException($"Error -- armor protection cannot be negative: {maxProtection}");
                }
                if (name.Length == 0)
                {
                    throw new System.ArgumentException($"Error -- name cannot be empty");
                }
                else if (name == null)
                {
                    throw new System.ArgumentNullException("Error -- name cannot be null");
                }

                this.MaxProtection = maxProtection;
            }

            public void BuffArmor(int buff)
            {
                if (buff < 0)
                {
                    throw new System.ArgumentOutOfRangeException($"Error -- buff cannot be negative: {buff}");
                }

                this.MaxProtection += buff;
            }

            override
            public string ToString()
            {
                return $"{this.Name}({this.MaxProtection})";
            }
        }

        public class Weapon : Item
        {
            public int MaxDamage {get; private set;}

            public Weapon(string name, int maxDamage, int monetaryValue = 0) : base(name, monetaryValue)
            {
                if (maxDamage < 0)
                {
                    throw new System.ArgumentException($"Error -- maximum damage on a weapon can't be negative: {maxDamage}");
                }
                if (name.Length == 0)
                {
                    throw new System.ArgumentException($"Error -- name cannot be empty");
                }
                else if (name == null)
                {
                    throw new System.ArgumentNullException("Error -- name cannot be null");
                }

                this.MaxDamage = maxDamage;
            }

            public void BuffWeapon(int buff)
            {
                if (buff < 0)
                {
                    throw new System.ArgumentOutOfRangeException($"Error -- buff cannot be negative: {buff}");
                }

                this.MaxDamage += buff;
            }

            override
            public string ToString()
            {
                return $"{this.Name}({this.MaxDamage})";
            }
        }


}