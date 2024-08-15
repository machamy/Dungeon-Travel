using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts.Entity;
using UnityEngine.UIElements;

namespace Scripts.User
{
    /// <summary>
    /// 파티 관리 클래스;
    /// PartyManager에서 그대로 옮겼다.
    /// </summary>
    public class Party
    {
        private List<Character> party;

        public int MaxAmount = 5;

        protected Party() { }

        public static Party CreateInstance()
        {
            Party pty = new Party();
            pty.party = new List<Character>();
            pty.RegisterTestParty();

            return pty;
        }

        public void AddCharacter(Character character)
        {
            if (party.Count > MaxAmount)
                return;
            party.Add(character);
        }

        public void RemoveCharacter(Character character)
        {
            party.Remove(character);
        }

        public Character GetCharacter(int idx)
        {
            if (idx >= party.Count)
                return null;
            return party[idx];
        }

        public List<Character> GetCharacters()
        {
            return party.ToList();
        }

        public void RegisterTestParty()
        {
            for (int i = 0; i < MaxAmount; i++)
            {
                Character character = new Character($"TestCharacter_{i}");
                party.Add(character.SetClass(Class.ClassList[i + 1]));
            }
        }
    }
}
