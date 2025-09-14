namespace Server.Game.Network.ServerPacket
{
    internal class PROTOCOL_SEASON_CHALLENGE_INFO_ACK : GameServerPacket
    {
        private readonly string SeasonName;
        private readonly short SeasonDays;
        public PROTOCOL_SEASON_CHALLENGE_INFO_ACK()
        {
            SeasonName = "Incoming New Year!";
            SeasonDays = 8; //max 99
        }
        public override void Write()
        {
            WriteH(8449);
            WriteH(0);
            WriteD(1); //unk (enable or disable season)
            WriteC(6); //Current level

            WriteD(2580); //Total earned points

            WriteC(5); //Normal levels complete
            WriteC(5); //Premium levels complete
            WriteC(1); //Enable premium
            WriteC(0); //unk
            WriteD(4); //Current buy level
            WriteC(1); //Season (1-continuing, 2-fnished, 0 or 3 can't buy pass)
            WriteU(SeasonName, 42); //Name
            WriteH(SeasonDays); //Total Days
            WriteD(0); //unk

            //Card 1
            WriteD(10324401); //Normal card
            WriteD(10402201); //Premium card B
            WriteD(10324501); //Premium card A
            WriteD(0);        //Total points for open card

            //Card 2
            WriteD(10514101); //Normal card
            WriteD(10331501); //Premium card B
            WriteD(10514201); //Premium card A
            WriteD(500);      //Total points for open card


            //Card 3
            WriteD(10606301); //Normal card
            WriteD(60141201); //Premium card B
            WriteD(10606401); //Premium card A
            WriteD(1000);     //Total points for open card

            //Card 4
            WriteD(20207901); //Normal card
            WriteD(60240901); //Premium card B
            WriteD(20208001); //Premium card A
            WriteD(1500);     //Total points for open card

            //Card 5
            WriteD(30112301); //Normal card
            WriteD(10401401); //Premium card B
            WriteD(30112401); //Premium card A
            WriteD(2500);     //Total points for open card

            //Card 6
            WriteD(80031101); //Normal card
            WriteD(20202701); //Premium card B
            WriteD(80031201); //Premium card A
            WriteD(3000);     //Total points for open card

            //Card 7
            WriteD(160002701); //Normal card
            WriteD(180004101); //Premium card B
            WriteD(180004901); //Premium card A
            WriteD(3500);      //Total points for open card

            for (int i = 0; i < SeasonDays; i++)
            {
                WriteD(160002612); // NORMAL CARD
                WriteD(160000608); // PREMIUM CARD A
                WriteD(160003512); // PREMIUM CARD B
                WriteD(0);         //Total points for open card
            }

            int remainingBytes = 92 - SeasonDays;
            for (int i = 0; i < remainingBytes; i++)
            {
                WriteB(new byte[16]);
            }

            WriteB(new byte[12]);

            WriteD(2412010559); //Start date
            WriteD(2501312359); //Snd date
        }
    }
}
