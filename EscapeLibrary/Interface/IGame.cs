using EscapeLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeLibrary.Interface
{
    internal interface IGame
    {
        event EventHandler ElapsedTimeChanged;
        bool IsContinue {  get; }
        TimeSpan ElapsedTime { get; }
        void Start();
        void Pause();
        void Move(Direction direction);
    }
}
