namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SEASON_CHALLENGE_SEND_REWARD_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_SEASON_CHALLENGE_SEND_REWARD_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            /*
                case -2147473616:
                  v5 = "EVENT_ERROR_SEASON_CHALLENGE_UNKWON";
                case -2147473615:
                  v5 = "EVENT_ERROR_SEASON_CHALLENGE_DIFFRENT_SEASON_ID";
                case -2147473614:
                  v5 = "EVENT_ERROR_SEASON_CHALLENGE_EXCEED_LEVEL";
                case -2147473613:
                  v5 = "EVENT_ERROR_SEASON_CHALLENGE_ALREADY_GET_REWARD";
                case -2147473612:
                  v5 = "EVENT_ERROR_SEASON_CHALLENGE_SEASON_NOT_GOING";
                case -2147473611:
                  v5 = "EVENT_ERROR_SEASON_CHALLENGE_BUY_ALREADY_SEASON_PASS";
                case -2147473610:
                  v5 = "EVENT_ERROR_SEASON_CHALLENGE_BUY_UNKOWN_FAIL";
                case -2147473609:
                  v5 = "EVENT_ERROR_SEASON_CHALLENGE_BUY_NOT_ENOUGH_MONEY";
                case -2147473608:
                  v5 = "EVENT_ERROR_SEASON_CHALLENGE_BUY_NO_GOODS";
                case -2147473607:
                  v5 = "EVENT_ERROR_SEASON_CHALLENGE_BUY_TRANS_ERROR";
             */
            WriteH(8452);
            WriteH(0);
            WriteD(Error);
            WriteC(1);
            WriteD(10402201); //premium b card good_id

            WriteD(10324401); //normal card good_id

            //WriteD(10324501); //premium b card good_id

            WriteC(1);
            WriteC(1);
            WriteC(1);
        }
    }
}