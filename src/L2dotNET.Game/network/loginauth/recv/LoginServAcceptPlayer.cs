﻿using L2dotNET.Models;
using System;

namespace L2dotNET.Game.network.loginauth.recv
{
    class LoginServAcceptPlayer : ReceiveAuthPacket
    {
        private string account;
        public LoginServAcceptPlayer(AuthThread login, byte[] db)
        {
            base.makeme(login, db);
        }

        public override void read()
        {
            account = readS();
        }

        public override void run()
        {
            AccountModel ta = new AccountModel();
            ta.Login = account;

            AuthThread.Instance.awaitAccount(ta);
        }
    }

}
