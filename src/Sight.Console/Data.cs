// Author: Viyrex(aka Yuyu)
// Contact: mailto:viyrex.aka.yuyu@gmail.com
// Github: https://github.com/0x0001F36D

namespace Sight.Console
{
    using System;

    public class Data
    {
        #region Properties

        public double Humidity { get; set; }

        public double Temperature { get; set; }

        public DateTime Time { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString() => $"[{Time.ToString("yyyy/MM/dd hh:mm:ss.ffff")}] Temperature:{Temperature}, Humidity:{Humidity}";
        //--port 1234 --logpath log.txt --password sight
        #endregion Methods
    }
}