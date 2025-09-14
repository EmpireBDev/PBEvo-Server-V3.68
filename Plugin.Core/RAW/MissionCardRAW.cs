using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.RAW
{
    public class MissionCardRAW
    {
        private static List<MissionItemAward> Items = new List<MissionItemAward>();
        private static List<MissionCardModel> List = new List<MissionCardModel>();
        private static List<MissionCardAwards> Awards = new List<MissionCardAwards>();

        private static void Load(string file, int type)
        {
            string path = "Data/Missions/" + file + ".mqf";
            if (File.Exists(path))
            {
                Parse(path, file, type);
            }
            else
            {
                CLogger.Print($"File not found: {path}", LoggerType.Warning);
            }
        }
        public static void LoadBasicCards(int type)
        {
            Load("TutorialCard_Russia", type);
            Load("Dino_Tutorial", type);
            Load("Human_Tutorial", type);
            Load("AssaultCard", type);
            Load("BackUpCard", type);
            Load("InfiltrationCard", type);
            Load("SpecialCard", type);
            Load("DefconCard", type);
            Load("Commissioned_o", type);
            Load("Company_o", type);
            Load("Field_o", type);
            Load("EventCard", type);
            Load("Dino_Basic", type);
            Load("Human_Basic", type);
            Load("Dino_Intensify", type);
            Load("Human_Intensify", type);
            CLogger.Print($"Plugin Loaded: {List.Count} Mission Card List", LoggerType.Info);
            if (type == 1)
            {
                CLogger.Print($"Plugin Loaded: {Awards.Count} Mission Card Awards", LoggerType.Info);
            }
            else if (type == 2)
            {
                CLogger.Print($"Plugin Loaded: {Items.Count} Mission Reward Items", LoggerType.Info);
            }
        }
        private static int ConvertStringToInt(string missionName)
        {
            int missionId = 0;
            if (missionName == "TutorialCard_Russia") missionId = 1;
            else if (missionName == "Dino_Tutorial") missionId = 2;
            else if (missionName == "Human_Tutorial") missionId = 3;
            else if (missionName == "AssaultCard") missionId = 5;
            else if (missionName == "BackUpCard") missionId = 6;
            else if (missionName == "InfiltrationCard") missionId = 7;
            else if (missionName == "SpecialCard") missionId = 8;
            else if (missionName == "DefconCard") missionId = 9;
            else if (missionName == "Commissioned_o") missionId = 10;
            else if (missionName == "Company_o") missionId = 11;
            else if (missionName == "Field_o") missionId = 12;
            else if (missionName == "EventCard") missionId = 13;
            else if (missionName == "Dino_Basic") missionId = 14;
            else if (missionName == "Human_Basic") missionId = 15;
            else if (missionName == "Dino_Intensify") missionId = 16;
            else if (missionName == "Human_Intensify") missionId = 17;
            return missionId;
        }
        public static List<ItemsModel> GetMissionAwards(int MissionId)
        {
            List<ItemsModel> Result = new List<ItemsModel>();
            lock (Items)
            {
                foreach (MissionItemAward Mission in Items)
                {
                    if (Mission.MissionId == MissionId)
                    {
                        Result.Add(Mission.Item);
                    }
                }
            }
            return Result;
        }
        public static List<MissionCardModel> GetCards(int MissionId, int CardBasicId)
        {
            List<MissionCardModel> Result = new List<MissionCardModel>();
            lock (List)
            {
                foreach (MissionCardModel Card in List)
                {
                    if (Card.MissionId == MissionId && (CardBasicId >= 0 && Card.CardBasicId == CardBasicId || CardBasicId == -1))
                    {
                        Result.Add(Card);
                    }
                }
            }
            return Result;
        }
        public static List<MissionCardModel> GetCards(List<MissionCardModel> Cards, int CardBasicId)
        {
            if (CardBasicId == -1)
            {
                return Cards;
            }
            List<MissionCardModel> Result = new List<MissionCardModel>();
            foreach (MissionCardModel Card in List)
            {
                if (CardBasicId >= 0 && Card.CardBasicId == CardBasicId || CardBasicId == -1)
                {
                    Result.Add(Card);
                }
            }
            return Result;
        }
        public static List<MissionCardModel> GetCards(int MissionId)
        {
            List<MissionCardModel> Result = new List<MissionCardModel>();
            lock (List)
            {
                foreach (MissionCardModel Card in List)
                {
                    if (Card.MissionId == MissionId)
                    {
                        Result.Add(Card);
                    }
                }
            }
            return Result;
        }
        private static void Parse(string path, string missionName, int typeLoad)
        {
            int missionId = ConvertStringToInt(missionName);
            if (missionId == 0)
            {
                CLogger.Print($"Invalid: {missionName}", LoggerType.Warning);
            }
            byte[] Buffer;
            try
            {
                Buffer = File.ReadAllBytes(path);
            }
            catch
            {
                Buffer = new byte[0];
            }
            if (Buffer.Length == 0)
            {
                return;
            }
            try
            {
                SyncClientPacket C = new SyncClientPacket(Buffer);
                C.ReadS(4);
                int questType = C.ReadD();
                C.ReadB(16);
                int valor1 = 0, valor2 = 0;
                for (int i = 0; i < 40; i++)
                {
                    int missionBId = valor2++, cardBId = valor1;
                    if (valor2 == 4)
                    {
                        valor2 = 0;
                        valor1++;
                    }
                    int reqType = C.ReadUH();
                    int type = C.ReadC();
                    int mapId = C.ReadC();
                    int limitCount = C.ReadC();
                    ClassType weaponClass = (ClassType)C.ReadC();
                    int weaponId = C.ReadUH();
                    MissionCardModel MissionCard = new MissionCardModel(cardBId, missionBId)
                    {
                        MapId = mapId,
                        WeaponReq = weaponClass,
                        WeaponReqId = weaponId,
                        MissionType = (MissionType)type,
                        MissionLimit = limitCount,
                        MissionId = missionId
                    };
                    List.Add(MissionCard);
                    if (questType == 1)
                    {
                        C.ReadB(24);
                    }
                }
                int vai = (questType == 2 ? 5 : 1);
                for (int i = 0; i < 10; i++)
                {
                    int gp = C.ReadD();
                    int xp = C.ReadD();
                    int medals = C.ReadD();
                    for (int i2 = 0; i2 < vai; i2++)
                    {
                        int unk = C.ReadD();
                        int type = C.ReadD();
                        int itemId = C.ReadD();
                        int itemCount = C.ReadD();
                    }
                    if (typeLoad == 1)
                    {
                        MissionCardAwards card = new MissionCardAwards()
                        { 
                            Id = missionId, 
                            Card = i, 
                            Exp = (questType == 1 ? (xp * 10) : xp), 
                            Gold = gp 
                        };
                        GetCardMedalInfo(card, medals);
                        if (!card.Unusable())
                        {
                            Awards.Add(card);
                        }
                    }
                }
                if (questType == 2)
                {
                    int goldResult = C.ReadD();
                    C.ReadB(8);
                    for (int i = 0; i < 5; i++)
                    {
                        int unkI = C.ReadD();
                        int typeI = C.ReadD(); //1 - unidade | 2 - dias
                        int itemId = C.ReadD();
                        uint itemCount = C.ReadUD();
                        if (unkI > 0 && typeLoad == 1)
                        {
                            MissionItemAward item = new MissionItemAward()
                            { 
                                MissionId = missionId, 
                                Item = new ItemsModel(itemId, "Mission Item", ItemEquipType.Durable, itemCount) 
                            };
                            Items.Add(item);
                        }
                    }
                }
            }
            catch (XmlException ex)
            {
                CLogger.Print($"File error: {path} \r\n {ex.Message}", LoggerType.Error, ex);
            }
        }
        private static void GetCardMedalInfo(MissionCardAwards card, int medalId)
        {
            if (medalId == 0)
            {
                return;
            }
            if (medalId >= 1 && medalId <= 50) //v >= 1 && v <= 50
            {
                card.Ribbon++;
            }
            else if (medalId >= 51 && medalId <= 100) //v >= 51 && v <= 100
            {
                card.Ensign++;
            }
            else if (medalId >= 101 && medalId <= 116) //v >= 101 && v <= 116
            {
                card.Medal++;
            }
            //v >= 117 && v <= 239
        }
        public static MissionCardAwards GetAward(int mission, int cartao)
        {
            foreach (MissionCardAwards card in Awards)
            {
                if (card.Id == mission && card.Card == cartao)
                {
                    return card;
                }
            }
            return null;
        }
    }
}
