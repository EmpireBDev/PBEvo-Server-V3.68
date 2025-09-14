using Plugin.Core.Utility;
using System.Collections.Generic;

namespace Plugin.Core.Models
{
    public class EventVisitModel
    {
		public int Id;
		public uint StartDate;
		public uint EndDate;
		public int Checks;
		public string Title;
		public List<VisitBoxModel> Boxes = new List<VisitBoxModel>();
		public EventVisitModel()
		{
			Checks = 31;
			Title = "";
			for (int i = 0; i < 31; i++)
			{
				Boxes.Add(new VisitBoxModel());
			}
		}
		public bool EventIsEnabled()
		{
			uint Now = uint.Parse(DateTimeUtil.Now("yyMMddHHmm"));
			return StartDate <= Now && Now < EndDate;
		}
		public VisitItemModel GetReward(int Index, int RewardIdx)
		{
			try
			{
				return RewardIdx == 0 ? Boxes[Index].Reward1 : Boxes[Index].Reward2;
			}
			catch
			{
				return null;
			}
		}
		public void SetBoxCounts()
		{
			for (int i = 0; i < 31; i++)
			{
				Boxes[i].SetCount();
			}
		}
	}
}
