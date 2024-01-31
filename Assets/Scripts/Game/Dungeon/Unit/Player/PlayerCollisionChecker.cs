using UnityEngine;

namespace Scripts.Game.Dungeon.Unit
{
    public class PlayerCollisionChecker : MonoBehaviour
    {
        public PlayerUnit pu;
        
        /// <summary>
        /// 함정
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerStay(Collider other)
        {
            pu.OnStay(other);
        }
    }
}