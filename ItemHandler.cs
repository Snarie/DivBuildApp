using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DivBuildApp
{
    public static class ItemHandler
    {

        public static List<StringItem> AllItemList = new List<StringItem>();

        public static StringItem ItemFromIdentity(string name, string slot)
        {
            return AllItemList.FirstOrDefault(i => i.Name == name && i.Slot == slot);
        }
        public static StringItem ItemFromName(string name)
        {
            return AllItemList.FirstOrDefault(i => i.Name == name);
        }
        public static string BrandFromName(string name)
        {
            return AllItemList.FirstOrDefault(i => i.Name == name).BrandName;
        }
    }
}
