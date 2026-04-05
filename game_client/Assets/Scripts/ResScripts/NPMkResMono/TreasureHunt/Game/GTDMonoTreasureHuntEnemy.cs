using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GTDMonoTreasureHuntEnemy : MonoBehaviour
    {
        [ALHeader("敌人碰撞半径")]
        public float radius;
        [ALHeader("随机速度范围")]
        public WCGFloatRange randomSpeedRange;
        [ALHeader("随机移动方向列表"),
         ALInfo("这个敌人将会从列表里随机选择一个对象，生成后朝向这个对象飞")]
        public List<Transform> randomMoveDirectionList;
        [ALHeader("特效父对象和碰撞特效")]
        public Transform sfxTransform;
        public long hitSfxId;
        
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
            if (randomMoveDirectionList != null)
            {
                foreach (Transform item in randomMoveDirectionList)
                {
                    if (item == null)
                        continue;

                    Gizmos.DrawLine(transform.position, item.position);
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
            if (randomMoveDirectionList != null)
            {
                foreach (Transform item in randomMoveDirectionList)
                {
                    if (item == null)
                        continue;

                    Gizmos.DrawLine(transform.position, item.position);
                }
            }
        }
    }
}