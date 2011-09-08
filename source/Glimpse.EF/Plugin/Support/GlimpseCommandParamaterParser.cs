﻿using Glimpse.EF.Extensibility;

namespace Glimpse.EF.Plugin.Support
{
    internal class GlimpseCommandParamaterParser : ICommandParamaterParser
    {
        private const string QuotedFormat = "'{0}' /* {1} */";
        private const string UnquotedFormat = "{0} /* {1} */";

        public GlimpseCommandParamaterParser(bool useQuotes)
        { 
            UseQuotes = useQuotes;
        } 

        private bool UseQuotes { get; set; }

        public string Parse(string command, string parameterName, object parameterValue, string parameterType, int parameterSize)
        {
            if (parameterValue == null || parameterValue.ToString().Length > 50)
                return command;

            var fomatter = UseQuotes ? QuotedFormat : UnquotedFormat;

            return command.Replace(parameterName, string.Format(fomatter, parameterValue, parameterName));
        }
    }
}