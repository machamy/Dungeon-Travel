using UnityEngine;

namespace Scripts
{
    public class Utility
    {

        
        /// <summary>
        /// 확률 배열이 주어지면 그 확률에 따라 인덱스를 받아온다.
        /// </summary>
        /// <remarks>
        /// 다음 코드에서는 0, 1, 2, 3 이 각 10,20,30,40% 확률로 나온다.
        /// <code>
        /// int num = WeightedRandom(10,20,30,40);
        /// </code>
        /// </remarks>
        /// <param name="weights"></param>
        /// <returns></returns>
        public static int WeightedRandom(params int[] weights)
        {
            int total = 0;
            foreach (int weight in weights)
            {
                total += weight;
            }

            int random = Random.Range(0, total);
            int idx = 0;

            // 당첨 될때까지 반복
            while (!(random < weights[idx]))
            {
                idx++;
                random -= weights[idx];
            }
            
            return idx;
        }
    }
}