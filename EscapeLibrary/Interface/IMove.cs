using EscapeLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeLibrary.Interface
{
    internal interface IMove
    {
        // Hareket alanının dışardan değiştirilmemesi için set yazılmıyor.
        Size MoveAreaSize { get; }

        int HMoveDistance { get; } // Tek seferde ne kadar hareket edeceğini belirlemek için.
        int EMoveDistance { get; }

        /// <summary>
        /// Cismi istenilen yönde hareket ettirir
        /// </summary>
        /// <param name="direction">Hangi yone hareket edilecegi</param>
        /// <returns>Cisim duvara çarparsa true dondurur.</returns>
        bool DoMove (Direction direction); 
    }
}
