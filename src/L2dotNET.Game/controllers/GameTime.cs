﻿using System;
using System.Runtime.Remoting.Contexts;
using L2dotNET.Game.network;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.world;
using log4net;

namespace L2dotNET.Game.controllers
{
    [Synchronization]
    public class GameTime
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GameTime));

        private static volatile GameTime instance;
        private static object syncRoot = new object();

        public static GameTime Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new GameTime();
                        }
                    }
                }

                return instance;
            }
        }

        private int Time;
        private GameServerNetworkPacket DayPk = new SunRise();
        private GameServerNetworkPacket NightPk = new SunSet();
        private System.Timers.Timer TimeController;
        public DateTime serverStartUp;
        public static bool Night = false;

        private const int SEC_DAY = 10800, SEC_NIGHT = 3600, SEC_HOUR = 600, SEC_DN = 14400;
        private const int SEC_SCALE = 1800;

        public GameTime()
        {

        }

        public void Initialize()
        {
            serverStartUp = DateTime.Now;
            Time = 5800 + 0; // 10800 18:00 вечер
            TimeController = new System.Timers.Timer();
            TimeController.Interval = 1000;
            TimeController.Elapsed += new System.Timers.ElapsedEventHandler(ActionTime);
            TimeController.Enabled = true;
            log.Info("GameTime Controller: started 18:00 PM.");
        }

        private void ActionTime(object sender, System.Timers.ElapsedEventArgs e)
        {
            Time++;

            switch (Time)
            {
                case SEC_DAY + SEC_SCALE: // 21:00
                    NotifyStartNight();
                    break;
                case SEC_SCALE: // 03:00
                    NotifyStartDay();
                    break;
            }

            if (Time == SEC_DN)
                Time = 0;
        }

        private void NotifyStartDay()
        {
            Night = false;

            foreach (L2Player p in L2World.Instance.GetAllPlayers())
                p.NotifyDayChange(DayPk);
        }

        private void NotifyStartNight()
        {
            Night = true;

            foreach (L2Player p in L2World.Instance.GetAllPlayers())
                p.NotifyDayChange(NightPk);
        }

        public void EnterWorld(L2Player p)
        {
            p.NotifyDayChange(Night ? NightPk : DayPk);
        }

        public void ShowInfo(L2Player player)
        {
            DateTime dt = new DateTime(2000, 1, 1, 0, 0, 0).AddSeconds(Time * 6);

            SystemMessage sm = new SystemMessage(Night ? 928 : 927);
            sm.AddString(dt.Hour < 10 ? "0" + dt.Hour : "" + dt.Hour);
            string str = dt.Minute < 10 ? "0" + dt.Minute : "" + dt.Minute;
            str += ":";
            str += dt.Second < 10 ? "0" + dt.Second : "" + dt.Second;
            sm.AddString(str);
            player.sendPacket(sm);
        }
    }
}
