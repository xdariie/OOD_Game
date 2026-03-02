using System;
using System.Collections.Generic;
using System.Text;

namespace OODGame.Room
{
    public abstract class Cell
    {
        public abstract bool CanEnter { get; }
        public abstract char Symbol { get; }

    }


    public sealed class EmptyCell : Cell
    {
        public override bool CanEnter => true;
        public override char Symbol => ' ';

    }



    public sealed class WallCell : Cell
    {
        public override bool CanEnter => false;
        public override char Symbol => '█';
    }
}
