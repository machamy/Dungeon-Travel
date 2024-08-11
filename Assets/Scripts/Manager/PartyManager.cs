using System.Collections.Generic;
using System.Linq;
using Scripts.Entity;
using Scripts.Game;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Manager
{
    /// <summary>
    /// 파티 관리 클래스
    /// </summary>
    /// <remarks>
    /// 임시 매니저임. 갈아엎어도 됨
    /// 추천안 : 
    /// 1. 파티 클래스를 만들고
    /// 2. 파티 매니저에서 파티의 리스트를 관리하는 방식
    /// </remarks>
    public class PartyManager
    {
        private List<Character> party;

        public Inventory Inventory { get; private set; }

        public int MaxAmount = 5;

        public PartyManager()
        {
            party = new List<Character>();
            Inventory = Inventory.CreateInstance(27);
        }

        public void Add(Character character)
        {
            if (party.Count > MaxAmount)
                return;
            party.Add(character);
        }

        public void Remove(Character character)
        {
            party.Remove(character);
        }

        public Character Get(int idx)
        {
            if (idx >= party.Count)
                return null ;
            return party[idx];
        }

        public List<Character> GetAll()
        {
            return party.ToList() ;
        }


        public void RegisterTestParty()
        {
            for (int i = 0; i < MaxAmount; i++)
            {
                Character character = new Character($"TestCharacter_{i}");
                party.Add(character.SetClass(Class.ClassList[i+1]));
            }
        }
    }
}