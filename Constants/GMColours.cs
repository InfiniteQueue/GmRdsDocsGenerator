using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Constants
{
    /// <summary>Colours from the GM BIW palette (LibraryRoot\Libraries\BIW\BIN\COLOR_PALETTE\ColorsDef.xml)</summary>
    internal static class GMColours
    {
        public static double DcsTransparency => 0.2; // 20% transparent
        public static TxColor Motor => new TxColor (90, 90, 90, 1.0);             // #5A5A5A
        public static TxColor Piston => new TxColor (192, 220, 192,1.0);         // #C0DCC0
        public static TxColor Frames => new TxColor (0, 0, 255,1.0);             // #0000FF
        public static TxColor MainBody => new TxColor (160, 160, 164, 1.0);       // #A0A0A4
        public static TxColor MountPlates => new TxColor (128, 128, 128, 1.0);    // #808080
        public static TxColor Armsshanks => new TxColor (140, 120, 50, 1.0);      // #8C7832

        public static TxColor White => new TxColor (255, 255, 255, 1.0);          // #FFFFFF
        public static TxColor Gray1 => new TxColor (192, 192, 192, 1.0);          // #C0C0C0
        public static TxColor Gray2 => new TxColor (160, 160, 164, 1.0);          // #A0A0A4
        public static TxColor Gray3 => new TxColor (128, 128, 128, 1.0);          // #808080
        public static TxColor Gray4 => new TxColor (94, 94, 94, 1.0);             // #5E5E5E
        public static TxColor Gray5 => new TxColor (56, 56, 56, 1.0);             // #383838
        public static TxColor Black1 => new TxColor (40, 40, 40, 1.0);            // #282828
        public static TxColor Black2 => new TxColor (30, 30, 30, 1.0);            // #1E1E1E
        public static TxColor Black3 => new TxColor (15, 15, 15, 1.0);            // #0F0F0F
        public static TxColor PureBlack => new TxColor (0, 0, 0, 1.0);            // #000000

        public static TxColor Red => new TxColor (255, 0, 0, 1.0);                // #FF0000
        public static TxColor Orange => new TxColor (255, 127, 0, 1.0);           // #FF7F00
        public static TxColor Yellow => new TxColor (255, 255, 0, 1.0);           // #FFFF00
        public static TxColor Green => new TxColor (0, 138, 53, 1.0);             // #008A35
        public static TxColor Blue => new TxColor (0, 0, 255, 1.0);               // #0000FF

        public static TxColor RobotYellow => new TxColor (184, 184, 0, 1.0);      // #B8B800
        public static TxColor RobotMotor => new TxColor (139, 0, 0, 1.0);         // #8B0000
        public static TxColor RobotBase => new TxColor (30, 30, 30, 1.0);         // #1E1E1E
        public static TxColor Risers => new TxColor (225, 225, 225, 1.0);         // #E1E1E1

        public static TxColor Charcoal => new TxColor (54, 69, 79, 1.0);          // #36454F
        public static TxColor DarkTaupe => new TxColor (72, 60, 50, 1.0);         // #483C32
        public static TxColor AntiqueBronze => new TxColor (102, 93, 30, 1.0);    // #665D1E
        public static TxColor DarkMagenta => new TxColor (139, 0, 139, 1.0);      // #8B008B
        public static TxColor Cyan => new TxColor (0, 255, 255, 1.0);             // #00FFFF
        public static TxColor Magenta => new TxColor (255, 0, 255, 1.0);          // #FF00FF
        public static TxColor Tan => new TxColor (255, 168, 128, 1.0);            // #FFA880
        public static TxColor SlateGrey => new TxColor (90, 110, 110, 1.0);       // #5A6E6E
        public static TxColor Teal => new TxColor (0, 128, 128, 1.0);             // #008080
        public static TxColor DodgerBlue => new TxColor (0, 77, 140, 1.0);        // #004D8C

        public static TxColor Cobalt => new TxColor (26, 26, 87, 1.0);            // #1A1A57
        public static TxColor DarkPlum => new TxColor (64, 26, 64, 1.0);          // #401A40
        public static TxColor FireBrick => new TxColor (179, 33, 33, 1.0);        // #B32121
        public static TxColor LightOrange => new TxColor (255, 153, 13, 1.0);     // #FF990D
        public static TxColor CadetBlue => new TxColor (94, 145, 158, 1.0);       // #5E919E
        public static TxColor SlateGreen => new TxColor (110, 140, 120, 1.0);     // #6E8C78
        public static TxColor LightGreen => new TxColor (30, 120, 60, 1.0);       // #1E783C
        public static TxColor ArmyGreen => new TxColor (115, 120, 84, 1.0);       // #737854
        public static TxColor Putty => new TxColor (100, 80, 60, 1.0);            // #64503C
        public static TxColor Khaki => new TxColor (190, 150, 100, 1.0);          // #BE9664
        public static TxColor DcsModel => new TxColor(255, 255, 255, 0.2);        // White with reduced opacity
    }
}
