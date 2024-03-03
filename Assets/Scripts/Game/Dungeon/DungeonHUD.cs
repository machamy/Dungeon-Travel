using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Dungeon
{
    public class DungeonHUD : MonoBehaviour
    {
        [Header("DungeonInfo")]
        public TextMeshProUGUI nameTMP;
        public TextMeshProUGUI dayTMP;
        
        [Header("Minimap")]
        public RawImage minimap;
        public Image minimapPanel;
        public RawImage minimapBackground;
    }
}