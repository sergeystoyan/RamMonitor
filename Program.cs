//********************************************************************************************
//Author: Sergey Stoyan, CliverSoft.com
//        http://cliversoft.com
//        stoyan@cliversoft.com
//        sergey.stoyan@gmail.com
//        27 February 2007
//Copyright: (C) 2007, Sergey Stoyan
//********************************************************************************************

using System;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Net.Mail;
using Cliver;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Windows.Input;
using GlobalHotKey;


namespace Cliver.RamMonitor
{
    public class Program
    {
        static Program()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs args)
            {
                Exception e = (Exception)args.ExceptionObject;
                Message.Error(e);
                Application.Exit();
            };

            Log.Initialize(Log.Mode.ONLY_LOG);
            //Cliver.Config.Initialize(new string[] { "General" });
            Cliver.Config.Reload();
            SetTerminatingKeys();
        }        

        public static void SetTerminatingKeys()
        {
            if (key_manager != null)
            {
                key_manager.Dispose();
                key_manager = null;
            }
            if (Settings.General.TerminatingKey != System.Windows.Input.Key.None)
            {
                key_manager = new HotKeyManager();
                System.Windows.Input.ModifierKeys mks;
                if (Settings.General.TerminatingModifierKey1 != ModifierKeys.None)
                {
                    mks = Settings.General.TerminatingModifierKey1;
                    if (Settings.General.TerminatingModifierKey2 != ModifierKeys.None)
                        mks |= Settings.General.TerminatingModifierKey2;
                }
                else
                    mks = ModifierKeys.None;
                var hotKey = key_manager.Register(Settings.General.TerminatingKey, mks);
                key_manager.KeyPressed += delegate (object sender, KeyPressedEventArgs e)
                {
                    if (!Message.YesNo("Do you want to terminate " + ProgramRoutines.GetAppName() + "?"))
                        return;
                    Log.Main.Exit2("Keys pressed.");
                };
            }
        }
        static HotKeyManager key_manager;

        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                ProcessRoutines.RunSingleProcessOnly();

                Dictionary<string, string> clps = ProgramRoutines.GetCommandLineParameters();
                string v;
                if (clps.TryGetValue("Process", out v))
                    Settings.General.ProcessName = v;
                if (clps.TryGetValue("Regex", out v))
                    Settings.General.DumpRegex = new Regex(v);
                if (clps.TryGetValue("Url", out v))
                    Settings.General.EventUrl = v;
                if (clps.TryGetValue("Period", out v))
                    Settings.General.CheckPeriodInSecs = uint.Parse(v);
                //Settings.General.Save();

                Service.Running = true;

                Application.Run(SysTray.This);
            }
            catch (Exception e)
            {
                Message.Error(e);
            }
            finally
            {
                Environment.Exit(0);
            }
        }
    }
}