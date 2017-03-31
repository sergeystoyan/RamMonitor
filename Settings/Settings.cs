using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Script.Serialization;

namespace Cliver.RamMonitor
{
    public partial class Settings
    {
        [Cliver.Settings.Obligatory]
        public static readonly GeneralSettings General;

        public class GeneralSettings : Cliver.Settings
        {
            public string ProcessName = "notepad";
            public Regex DumpRegex = new Regex(@"OutputBaseFilename=fril.jp.Setup");
            public string EventUrl = "http://localhost/_test/RamMonitorServer.php";
            public uint CheckPeriodInSecs = 120;
            public int EncodingCodePage = 1200;//Unicode
            //[Newtonsoft.Json.JsonIgnore]
            //public System.Text.Encoding Encoding = System.Text.Encoding.Unicode;

            //public override void Loaded()
            //{
            //    Encoding = System.Text.Encoding.GetEncoding(EncodingCodePage);
            //}

            //public override void Saving()
            //{
            //    EncodingCodePage = Encoding.CodePage;
            //}
        }
    }
}