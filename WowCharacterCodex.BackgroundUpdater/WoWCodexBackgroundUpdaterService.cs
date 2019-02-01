using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WoWCharacterCodex.Application;

namespace WowCharacterCodex.BackgroundUpdater
{
    public partial class WoWCodexBackgroundUpdaterService : ServiceBase
    {
        private TimeSpan _updateInterval;
        CodexService _codex;
        public WoWCodexBackgroundUpdaterService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LoadResources();
            _updateInterval = new TimeSpan(0,0,0,0,100);
            RefreshNextGuild();
            RefreshNextCharacter();
            InitializeNextCharacter();
        }

        private async void RefreshNextGuild()
        {
            await _codex.RefreshNextGuild();
            Thread.Sleep(_updateInterval);
            RefreshNextGuild();
        }

        private async void RefreshNextCharacter()
        {
            await _codex.RefreshNextCharacter();
            Thread.Sleep(_updateInterval);
            RefreshNextCharacter();
        }

        private async void InitializeNextCharacter()
        {
            await _codex.InitializeNextCharacter();
            Thread.Sleep(_updateInterval);
            InitializeNextCharacter();
        }

        private void LoadResources(bool loadCodex = true)
        {
            if (_codex == null && loadCodex)
            {
                string codexConnectionString;
                string credentialsConnectionString;
#if DEBUG
                codexConnectionString = ConfigurationManager.ConnectionStrings["codexConnectionString.DEV"].ConnectionString;
                credentialsConnectionString = ConfigurationManager.ConnectionStrings["credentialsConnectionString.DEV"].ConnectionString;
#else
            codexConnectionString = ConfigurationManager.ConnectionStrings["codexConnectionString"].ConnectionString;
            credentialsConnectionString = ConfigurationManager.ConnectionStrings["credentialsConnectionString"].ConnectionString;
#endif
                _codex = new CodexService(codexConnectionString, credentialsConnectionString);
            }
        }

        protected override void OnStop()
        {

        }
    }
}
