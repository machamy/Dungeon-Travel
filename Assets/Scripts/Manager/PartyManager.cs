using System.Collections.Generic;
using System.Linq;
using Scripts.Player;
using Unity.VisualScripting;

namespace Scripts.Manager
{
    public class PartyManager
    {
        private List<Character> party;

        public void Add(Character character)
        {
            party.Add(character);
        }

        public void Remove(Character character)
        {
            party.Remove(character);
        }

        public Character Get(int idx)
        {
            return party[idx];
        }

        public List<Character> GetAll()
        {
            return party.ToList() ;
        }
    }
}