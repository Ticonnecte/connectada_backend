using System.Collections.Generic;

namespace CidConnectada.Entities.Model.Dto
{
    public class ThemeDto
    {
        public ThemeDto(string primaryMainColor, string primaryDarkColor, 
            string primaryLightColor, string secondaryMainColor, 
            string secondaryDarkColor, string secondaryLightColor)
        {
            palette.primary.main = primaryMainColor;
            palette.primary.dark = primaryDarkColor;
            palette.primary.light = primaryLightColor;
            palette.secondary.main = secondaryMainColor;
            palette.secondary.dark = secondaryDarkColor;
            palette.secondary.light = secondaryLightColor;
        }
        
        public Palette palette { get; set; } = new Palette();
        public Typography typography { get; set; } = new Typography();
        public Shape shape { get; set; } = new Shape();
        public List<string> shadows { get; set; } = new List<string>
        {
            "none",
            "1px 0px 18px 6px rgba(32, 33, 36, 0.28)",
            "1px 1px 3px -1px rgb(0 0 0)",
            "1px 0px 18px 6px rgba(32, 33, 36, 0.28)",
            "1px 1px 3px -1px rgb(0 0 0)",
            "1px 0px 18px 6px rgba(32, 33, 36, 0.28)",
            "1px 1px 3px -1px rgb(0 0 0)"
        };
        public Transitions transitions { get; set; } = new Transitions();
    }

    public class Palette
    {
        public ColorGroup primary { get; set; } = new ColorGroup { main = "#0F6DB4", dark = "#0D4B8C", light = "#BFE1F7", contrastText = "#fff" };
        public ColorGroup secondary { get; set; } = new ColorGroup { main = "#006435", light = "#006435", dark = "#006435", contrastText = "#fff" };
        public ColorGroup error { get; set; } = new ColorGroup { main = "#c32525", light = "#c32525", dark = "#c32525", contrastText = "#fff" };
        public ColorGroup warning { get; set; } = new ColorGroup { main = "#b97a06", light = "#b97a06", dark = "#b97a06", contrastText = "#fff" };
        public ColorGroup success { get; set; } = new ColorGroup { main = "#2e7d32", light = "#4caf50", dark = "#1b5e20", contrastText = "#fff" };
        public ColorGroup info { get; set; } = new ColorGroup { main = "#0288d1", light = "##03a9f4", dark = "#01579b", contrastText = "#fff" };
        public CommonColors common { get; set; } = new CommonColors { white = "#fff", black = "black" };
        public GreyColors grey { get; set; } = new GreyColors
        {
            _50 = "#fafafa",
            _100 = "#e7e6e6",
            _200 = "#EAEAEA",
            _300 = "#D0D0D0",
            _600 = "#888790",
            _900 = "#242325"
        };
    }

    public class ColorGroup
    {
        public string main { get; set; }
        public string light { get; set; }
        public string dark { get; set; }
        public string contrastText { get; set; }
    }

    public class CommonColors
    {
        public string white { get; set; }
        public string black { get; set; }
    }

    public class GreyColors
    {
        public string _50 { get; set; }
        public string _100 { get; set; }
        public string _200 { get; set; }
        public string _300 { get; set; }
        public string _600 { get; set; }
        public string _900 { get; set; }
    }

    public class Typography
    {
        public string labelFont { get; set; } = "rgba(0, 0, 0, 0.82)";
        public string blacklabelFont { get; set; } = "rgba(0, 0, 0, 0.61)";
        public string inputTitleFont { get; set; } = "rgba(0, 0, 0, 0.54)";
    }

    public class Shape
    {
        public int borderRadius { get; set; } = 4;
    }

    public class Transitions
    {
        public string primaryColorHover { get; set; } = "#297DBC";
    }
}