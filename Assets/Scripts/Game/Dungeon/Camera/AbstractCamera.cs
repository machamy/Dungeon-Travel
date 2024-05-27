using UnityEngine;

namespace Scripts.Game.Dungeon
{
    public abstract class AbstractCamera : MonoBehaviour
    {
        public Transform target;
        public bool isMain = false;
        
        /// <summary>
        /// 카메라 기준으로 월드에서의 이동 방향을 구하는 함수
        /// </summary>
        /// <param name="carmeraDirection">카메라 기준 방향/param>
        /// <returns>월드 기준 방향</returns>
        public Vector3 GetWorldDiretion(Vector3 carmeraDirection)
        {
            float angle;
            Vector3 axis;
            transform.rotation.ToAngleAxis(out angle,out axis);

            Vector3 result = Quaternion.AngleAxis(angle, axis) * carmeraDirection;
            result.y = 0;
            result.Normalize();
            return result;
        }
    }
}