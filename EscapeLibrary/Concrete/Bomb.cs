using EscapeLibrary.Abstract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeLibrary.Concrete
{
    internal class Bomb : Obj
    {
        public Bomb(Size moveAreaSize) : base(moveAreaSize)
        {
        }
    }
}
