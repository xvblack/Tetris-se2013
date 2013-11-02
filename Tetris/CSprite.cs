using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    interface CSprite
    {
        int LPos { get; set; }
        int RPos { get; set; }
        int FallSpeed { get; set; }
    }
}
