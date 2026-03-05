namespace OODGame.Player
{
    public sealed class Player
    {
        public int X {get; private set;}
        public int Y {get; private set;}

        public int Strength {get; private set;}
        public int Dexterity {get; private set;}
        public int Health {get; private set;}
        public int Luck {get; private set;}
        public int Aggression {get; private set;}
        public int Wisdom {get; private set;}

        public object? RightHand {get; private set;}
        public object? LeftHand {get; private set;}


        public Player(int startX, int startY)
        {
            X=startX;
            Y=startY;

            Strength = 5;
            Dexterity =5;
            Health =10;
            Luck=3;
            Aggression =2;
            Wisdom = 1;
        }

        public void MoveTo(int newX, int newY)
        {
            X=newX;
            Y=newY;
        }
    }
}