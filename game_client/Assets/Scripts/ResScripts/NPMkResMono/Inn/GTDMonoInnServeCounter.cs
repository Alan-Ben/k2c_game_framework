
using UnityEngine;

namespace GOE
{
    public class GTDMonoInnServeCounter : MonoBehaviour
    {
        [ALHeader("服务位置")]
        public Transform servePosition;
        [ALHeader("队列方向向量")]
        public Vector3 queueDirection = Vector3.back;
        [ALHeader("队列间距")]
        public float queueSpacing = 2.0f;
        [ALHeader("最小客人生成距离")]
        public float minSpawnDistance = 5.0f; // 最小生成距离
        [ALHeader("队列偏移值")]
        public WCGFloatRange offsetRange = new WCGFloatRange(-1.0f, 1.0f);
        public Vector3 offsetVector = Vector3.right;
        [ALHeader("跟随目标")]
        public Transform followTarget;
        [ALHeader("动画组件")]
        public Animation serveAnimation;
        public string serveAnimationName;
        [ALHeader("动画播放延迟")]
        public float servingAnimationDelay = 2f;
        [ALHeader("增加服务完成计数的延迟")]
        public float servingCompleteDelay = 3f;
        
        /// <summary>
        /// 获取队列位置
        /// </summary>
        public Vector3 getQueuePosition(int _queueIndex)
        {
            if (servePosition == null)
                return Vector3.zero;

            Vector3 counterPos = servePosition.position;
            return counterPos + queueDirection.normalized * queueSpacing * _queueIndex;
        }
        /// <summary>
        /// 计算动态生成位置
        /// </summary>
        public Vector3 calculateSpawnPosition(int _totalGuestsInQueue)
        {
            if (servePosition == null)
                return Vector3.zero;

            Vector3 counterPos = servePosition.position;
            // Spawn position is further back from the last queue position
            float spawnDistance = queueSpacing * _totalGuestsInQueue;
            return counterPos + queueDirection.normalized * (spawnDistance + minSpawnDistance);
        }
        public Vector3 getQueueOffset()
        {
            if (offsetRange == null)
                return Vector3.zero;
            
            return offsetVector.normalized * offsetRange.getRandomValue();
        }
    }
}