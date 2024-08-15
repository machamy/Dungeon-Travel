using Scripts.User;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.User
{
    /// <summary>
    /// 진척도 관리 클래스;
    /// 2차 목표 관련 함수 구현 필요
    /// </summary>
    public class Progress : MonoBehaviour
    {
        public int day { get; private set; }
        private int maxFloor;

        private const int secondMissionDay = 16;
        private const int secondMissionFloor = 4;

        protected Progress() { }

        public static Progress CreateInstance()
        {
            Progress pgr = new Progress();
            pgr.day = 1; pgr.maxFloor = 1;

            return pgr;
        }

        public void NextDay()
        {
            day++;

            if (day == secondMissionDay) SecondMissionEvent();
        }

        public void VisitFloor(int floor)
        {
            if (maxFloor >= floor) return;
            maxFloor = floor;

            if (maxFloor == secondMissionFloor) SecondMissionEvent();
        }

        private void SecondMissionEvent()
        {
            //2차 목표
        }
    }

}
