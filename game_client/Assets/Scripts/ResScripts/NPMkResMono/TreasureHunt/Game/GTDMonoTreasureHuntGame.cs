using UnityEngine;

namespace GOE
{
    public class GTDMonoTreasureHuntGame : MonoBehaviour
    {
        [ALHeader("游戏单位的父节点")]
        public Transform unitRoot;
        [ALHeader("背景的加载位置")] 
        public Transform transGroundPos;
        [ALHeader("玩家对象")]
        public GTDMonoTreasureHuntPlayer monoPlayer;
        [ALHeader("敌人生成的位置，和水平上的随机数")]
        public Transform transEnemySpawnPos;
        public WCGFloatRange enemySpawnXRange;
        [ALHeader("游戏场景的范围，敌人超出这个范围会被销毁")]
        public Transform transGameStageRangeFront;
        public Transform transGameStageRangeBack;
        [ALHeader("世界缩放比例")]
        public float worldScale = 1f;
        [ALHeader("敌人在助跑时间还剩多久时开始生成")]
        public float enemySpawnTimeInRunUpState = 3f;
        [ALHeader("奖励距离线资源")]
        public NPGGoIndex rewardDistanceLineResIndex;
        [ALHeader("游戏结束后延迟多少秒弹出结算效果")]
        public float gameWinDelay = 1f;
        public float gameLoseDelay = 1.5f;
        

        private void OnDrawGizmosSelected()
        {
            _drawSpawnRangeGizmos();
            _drawGameStageRangeGizmos();
        }
        private void OnDrawGizmos()
        {
            _drawSpawnRangeGizmos();
            _drawGameStageRangeGizmos();
        }

        private void _drawSpawnRangeGizmos()
        {
            if (transEnemySpawnPos == null)
                return;

            Vector3 spawnPos = transEnemySpawnPos.position;
            
            // Draw spawn position
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawnPos, 0.5f);
            
            // Draw spawn range
            if (enemySpawnXRange != null)
            {
                Gizmos.color = Color.yellow;
                Vector3 leftPos = spawnPos + Vector3.right * enemySpawnXRange.min;
                Vector3 rightPos = spawnPos + Vector3.right * enemySpawnXRange.max;
                
                // Draw range line
                Gizmos.DrawLine(leftPos, rightPos);
                
                // Draw range endpoints
                Gizmos.DrawWireSphere(leftPos, 0.3f);
                Gizmos.DrawWireSphere(rightPos, 0.3f);
                
                // Draw range area (vertical line indicators)
                Gizmos.DrawLine(leftPos + Vector3.up, leftPos + Vector3.down);
                Gizmos.DrawLine(rightPos + Vector3.up, rightPos + Vector3.down);
            }
        }

        private void _drawGameStageRangeGizmos()
        {
            if (transGameStageRangeFront == null || transGameStageRangeBack == null)
                return;

            Vector3 frontPos = transGameStageRangeFront.position;
            Vector3 backPos = transGameStageRangeBack.position;
            
            // Draw range boundaries
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(frontPos, 0.5f);
            Gizmos.DrawWireSphere(backPos, 0.5f);
            
            // Draw boundary connection line
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(frontPos, backPos);
            
            // Draw boundary planes (cross indicators)
            float crossSize = 2f;
            
            // Front boundary cross
            Gizmos.color = Color.green;
            Vector3 frontUp = frontPos + Vector3.up * crossSize;
            Vector3 frontDown = frontPos + Vector3.down * crossSize;
            Vector3 frontLeft = frontPos + Vector3.left * crossSize;
            Vector3 frontRight = frontPos + Vector3.right * crossSize;
            
            Gizmos.DrawLine(frontUp, frontDown);
            Gizmos.DrawLine(frontLeft, frontRight);
            
            // Back boundary cross
            Vector3 backUp = backPos + Vector3.up * crossSize;
            Vector3 backDown = backPos + Vector3.down * crossSize;
            Vector3 backLeft = backPos + Vector3.left * crossSize;
            Vector3 backRight = backPos + Vector3.right * crossSize;
            
            Gizmos.DrawLine(backUp, backDown);
            Gizmos.DrawLine(backLeft, backRight);
        }
    }
}