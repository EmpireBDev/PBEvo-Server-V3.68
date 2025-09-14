using System.Collections.Generic;

namespace Plugin.Core.Models
{
    public class PlayerCharacters
    {
        public List<CharacterModel> Characters = new List<CharacterModel>();
		public int GetCharacterIdx(int Slot)
		{
			lock (Characters)
			{
				for (int i = 0; i < Characters.Count; i++)
				{
					if (Characters[i].Slot == Slot)
					{
						return i;
					}
				}
			}
			return -1;
		}
		public CharacterModel GetCharacter(int Id)
		{
			lock (Characters)
			{
				foreach (CharacterModel Character in Characters)
				{
					if (Character.Id == Id)
					{
						return Character;
					}
				}
			}
			return null;
		}
		public CharacterModel GetCharacterSlot(int Slot)
		{
			lock (Characters)
			{
				foreach (CharacterModel Character in Characters)
				{
					if (Character.Slot == Slot)
					{
						return Character;
					}
				}
			}
			return null;
		}
		public CharacterModel GetCharacter(int Slot, out int Index)
		{
			lock (Characters)
			{
				for (int i = 0; i < Characters.Count; i++)
				{
					CharacterModel Character = Characters[i];
					if (Character.Slot == Slot)
					{
						Index = i;
						return Character;
					}
				}
			}
			Index = -1;
			return null;
		}
		public void AddCharacter(CharacterModel Character)
		{
			lock (Characters)
			{
				Characters.Add(Character);
			}
		}
		public bool RemoveCharacter(CharacterModel Character)
		{
			lock (Characters)
			{
				return Characters.Remove(Character);
			}
		}
	}
}
