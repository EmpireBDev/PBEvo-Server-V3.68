namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_EVENT_PORTAL_ACK : GameServerPacket
    {
        private readonly bool Enable;
        public PROTOCOL_BASE_EVENT_PORTAL_ACK(bool Enable)
        {
            this.Enable = Enable;
        }
        public override void Write()
        {
            WriteH(719);
            WriteC(1); //enable (true/false)
            WriteC(1); //unk

            WriteD(8192); //unk (size?)
            WriteC(5); //events count (+1)

            byte type1 = 6;
            WriteC(type1); //event type
            WriteD(0); //event id
            WriteC(1); //always event (true/false)
            WriteD(1901010000); //start date
            WriteD(3312312359); //end date
            WriteD(1901010000); //unk date
            WriteC(1); //type?
            WriteU("Hello World", 120);
            WriteU("Text Here", 200);
            WriteH(1); //unk
            if (type1 == 6) 
            {
                WriteD(1800); //30 min
                WriteD(1200); //20 min
                WriteD(3000); //50 min
            }
            WriteD(370000101); //reward for 30min
            if (type1 == 6)
            {
                WriteB(new byte[76]); //reward for 30min
                WriteD(340022001); //reward for 20min
                WriteD(320018701); //reward for 20min
                WriteB(new byte[72]); //reward for 20min
                WriteD(63265601); //reward for 50min
                WriteD(66465701);  //reward for 50min
                WriteD(70225601); //reward for 50min
                WriteB(new byte[68]); //reward for 50min
            }

            //event 2
            WriteC(2); //event type
            WriteD(1); //event id
            WriteC(0);
            WriteD(2409010000); //start date
            WriteD(2411302359); //end date
            WriteD(2409010000); //unk date
            WriteC(0);
            WriteU("Hello event 1!", 120);
            WriteU("Text event 1!", 200);
            WriteH(1); //unk
            WriteD(80000501); //reward

            //event 3
            WriteC(2); //event type
            WriteD(2); //event id
            WriteC(0);
            WriteD(2409010000); //start date
            WriteD(2411302359); //end date
            WriteD(2409010000); //unk date
            WriteC(0);
            WriteU("Hello event 2!", 120);
            WriteU("Text event! 2", 200);
            WriteH(1); //unk
            WriteD(70019501); //reward

            //event 4
            WriteC(2); //event type
            WriteD(3); //event id
            WriteC(0);
            WriteD(2409010000); //start date
            WriteD(2411302359); //end date
            WriteD(2409010000); //unk date
            WriteC(0);
            WriteU("Hello event 3!", 120);
            WriteU("Text event! 3", 200);
            WriteH(1); //unk
            WriteD(31503801); //reward

            //event 5
            WriteC(2); //event type
            WriteD(4); //event id
            WriteC(0);
            WriteD(2409010000); //start date
            WriteD(2411302359); //end date
            WriteD(2409010000); //unk date
            WriteC(0);
            WriteU("Hello event 4!", 120);
            WriteU("Text event! 4", 200);
            WriteH(1); //unk
            WriteD(80035301); //reward
        }
    }
}
