using Plugin.Core.Utility;

namespace Plugin.Core.Models
{
    public class PlayerMissions
    {
        public long OwnerId;
        public byte[] List1 = new byte[40], List2 = new byte[40], List3 = new byte[40], List4 = new byte[40];
        public int ActualMission, Card1, Card2, Card3, Card4, Mission1, Mission2, Mission3, Mission4;
        public bool SelectedCard;
        public PlayerMissions()
        {
        }
        public byte[] GetCurrentMissionList()
        {
            if (ActualMission == 0)
            {
                return List1;
            }
            else if (ActualMission == 1)
            {
                return List2;
            }
            else if (ActualMission == 2)
            {
                return List3;
            }
            else
            {
                return List4;
            }
        }
        public int GetCurrentCard()
        {
            return GetCard(ActualMission);
        }
        public int GetCard(int index)
        {
            if (index == 0)
            {
                return Card1;
            }
            else if (index == 1)
            {
                return Card2;
            }
            else if (index == 2)
            {
                return Card3;
            }
            else
            {
                return Card4;
            }
        }
        public int GetCurrentMissionId()
        {
            if (ActualMission == 0)
            {
                return Mission1;
            }
            else if (ActualMission == 1)
            {
                return Mission2;
            }
            else if (ActualMission == 2)
            {
                return Mission3;
            }
            else
            {
                return Mission4;
            }
        }
        public void UpdateSelectedCard()
        {
            int CurrentCard = GetCurrentCard();
            if (65535 == ComDiv.GetMissionCardFlags(GetCurrentMissionId(), CurrentCard, GetCurrentMissionList()))
            {
                SelectedCard = true;
            }
        }
    }
}
