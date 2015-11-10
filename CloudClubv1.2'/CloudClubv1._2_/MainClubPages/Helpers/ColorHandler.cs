using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FrontEnd
{
    class ColorHandler
    {
        public Dictionary<string, Color> colorMap;
        public Dictionary<Color, string> nameMap;
       public Dictionary<string, string> nameToImageStringMap;
        public  List<Color> colorList;
        
        public ColorHandler()
        {

            colorMap = new Dictionary<string, Color>();
            nameMap = new Dictionary<Color, string>();
            nameToImageStringMap = new Dictionary<string, string>();
            colorList = new List<Color>();

            colorMap.Add("green", Color.FromRgb(0, 234, 106));
            colorMap.Add("blue", Color.FromRgb(0, 176, 240));
            colorMap.Add("yellow", Color.FromRgb(255, 192, 0));
            colorMap.Add("gold", Color.FromRgb(255, 192, 0));
            colorMap.Add("red", Color.FromRgb(255, 0, 0));
            colorMap.Add("redPressed", Color.FromRgb(255,200,200));
            colorMap.Add("black", Color.FromRgb(0, 0, 0));
            colorMap.Add("white", Color.White);
            colorMap.Add("navy", Color.FromRgb(31, 78, 121));
            colorMap.Add("purple", Color.FromRgb(210, 61, 235));
            colorMap.Add("purplePressed", Color.FromRgb(150, 61, 205));
            colorMap.Add("gray", Color.FromRgb(127, 127, 127));
            colorMap.Add("grayPressed", Color.FromRgb(127,5 , 127));

            colorMap.Add("lightGray", Color.FromRgb(231, 230, 230));
            colorMap.Add("lightGrayPressed", colorMap["gray"]);


            colorMap.Add("magenta", Color.FromRgb(251, 33, 241));
            colorMap.Add("default", Color.FromRgb(210, 61, 235)); //default set to cloudclub purple

            nameMap.Add(Color.FromRgb(0, 234, 106), "green");
            nameMap.Add(Color.FromRgb(0, 176, 240),"blue");
            nameMap.Add( Color.FromRgb(255, 192, 0),"yellow");
            nameMap.Add( Color.FromRgb(255, 0, 0),"red");
            nameMap.Add( Color.FromRgb(31, 78, 121),"navy");
            nameMap.Add( Color.FromRgb(210, 61, 235),"purple");
            nameMap.Add( Color.FromRgb(127, 127, 127),"gray");
            nameMap.Add( Color.FromRgb(251, 33, 241),"magenta");
            nameMap.Add(Color.FromRgb(231, 230, 230), "lightGray");




            colorList.Add(Color.FromRgb(0, 234, 106));
            colorList.Add(Color.FromRgb(0, 176, 240));
            colorList.Add(Color.FromRgb(255, 192, 0));
            colorList.Add(Color.FromRgb(255, 0, 0));
            colorList.Add(Color.FromRgb(31, 78, 121));
            colorList.Add( Color.FromRgb(210, 61, 235));
            colorList.Add(Color.FromRgb(127, 127, 127));
            colorList.Add(Color.FromRgb(251, 33, 241));

            //for the star images
            nameToImageStringMap.Add("green", "Green.png");
            nameToImageStringMap.Add("blue", "LightBlue.png");
            nameToImageStringMap.Add("Blue", "LightBlue.png");
            nameToImageStringMap.Add("gray", "Gold.png");
            nameToImageStringMap.Add("yellow", "Gold.png");
            nameToImageStringMap.Add("magenta", "Pink.png");
            nameToImageStringMap.Add("purple", "Purple.png");
            nameToImageStringMap.Add("red", "Red.png");
        }
        public Color fromStringToColor(string colorName)
        {
            if (colorMap.ContainsKey(colorName))
            {
                return colorMap[colorName];

            }
            else return colorMap["blue"];
        }
        public string fromColorToString(Color color)
        {
            if(nameMap.ContainsKey(color))
            {
                return nameMap[color];
            }
            else
            {
                return nameMap[Color.FromRgb(210,61,235)];
            }
        }
        public string getStarColorString(string colorString)
        {
            switch (colorString)
            {
                case "red":
                    return ("Star_Red.png");
                case "green":
                    return ("Star_Green.png");
                case "blue":
                    return ("Star_LightBlue.png");
                case "navy":
                    return ("Star_LightBlue.png");
                case "magenta":
                    return ("Star_Pink.png");
                case "gray":
                    return ("Star_Gray.png");
                case "purple":
                    return ("Star_Purple.png");
                default:
                    return "Star_Gold.png";
            }
        }

    }
}
