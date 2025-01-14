﻿using L2dotNET.Game.model.vehicles;
using L2dotNET.Game.network.l2send;
using log4net;

namespace L2dotNET.Game.network.l2recv
{
    class RequestMoveToLocationInVehicle : GameServerNetworkRequest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestMoveToLocationInVehicle));
        private int boatId;
        private int dx;
        private int dy;
        private int dz;
        private int x;
        private int y;
        private int z;
        public RequestMoveToLocationInVehicle(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            boatId = readD();   //objectId of boat
            dx = readD();
            dy = readD();
            dz = readD();
            x = readD();
            y = readD();
            z = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            //You do not possess the correct ticket to board the boat.

            if (player.Summon != null)
            {
                player.sendSystemMessage(1523);//You should release your pet or servitor so that it does not fall off of the boat and drown!
                player.sendActionFailed();
                return;
            }

            L2Boat boat = null;
            if (player.Boat != null)
            {
                if (player.Boat.ObjID == boatId)
                    boat = player.Boat;
                else
                {
                    player.sendActionFailed();
                    return;
                }
            }
            else if (player.knownObjects.ContainsKey(boatId))
                boat = (L2Boat)player.knownObjects[boatId];

            if (boat == null)
            {
                log.Error($"User requested null boat { boatId }");
                player.sendActionFailed();
                return;
            }

            if (player.Boat == null)
                player.Boat = boat;

            player.BoatX = dx;
            player.BoatY = dy;
            player.BoatZ = dz;
            player.broadcastPacket(new MoveToLocationInVehicle(player, x, y, z));
        }
    }
}
