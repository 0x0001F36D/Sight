// Author: Viyrex(aka Yuyu)
// Contact: mailto:viyrex.aka.yuyu@gmail.com
// Github: https://github.com/0x0001F36D

namespace Sight.Console
{
    using System;
    using System.Linq;
    using CommandLine;

    public class Options
    {
        #region Properties

        private string _password;

        [Option('l', "logpath", HelpText = "Sets the log file path.", Required = true)]
        public string LogPath { get; set; }

        
        [Option('v', "password", HelpText = "Sets the password.", Required = true)]
        public string Password
        {
            get => this._password;
            set
            {
                if (!value.All(char.IsLetterOrDigit))
                    throw new FormatException("password");

                this._password = value.WithHash();
            }
        }

        [Option('p', "port", HelpText = "Sets the server binding port.", Required = true)]
        public short Port { get; set; }

        #endregion Properties
    }
}