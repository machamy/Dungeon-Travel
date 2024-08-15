using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.User
{
    /// <summary>
    /// 설정 관리 클래스
    /// </summary>
    public class Config : MonoBehaviour
    {
        public enum Resolution
        {
            R1366x768,
            R1920x1080,
            R2560x1440,
            R3840x2160,
        }

        public enum Graphic
        {
            High,
            Mid,
            Low,
        }

        private Resolution resolution;
        private Graphic graphic;


        protected Config() { }

        public static Config CreateInstance()
        {
            Config cfg = new Config();
            cfg.resolution = Resolution.R3840x2160; cfg.graphic = Graphic.High;

            return cfg;
        }

        public void ChangeResolution(Resolution res)
        {
            resolution = res;
            //OnChangeResolution
        }

        public void ChangeGraphic(Graphic gph)
        {
            graphic = gph;
            //OnChangeGraphic
        }


    }

}
