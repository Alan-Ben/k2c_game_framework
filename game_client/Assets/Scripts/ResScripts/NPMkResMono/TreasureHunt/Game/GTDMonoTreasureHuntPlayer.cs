using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GTDMonoTreasureHuntPlayer : MonoBehaviour
    {
        [ALHeader("动画组件")]
        public Animator animator;
        [ALHeader("碰撞半径")]
        public float radius;
        [ALHeader("前进速度，和加速度")]
        public float idleMoveSpeedForward;
        public float boostMoveSpeedForward;
        public float searchingMoveSpeedForward;
        public float accelerationForward;
        [ALHeader("水平移动速度")]
        public float moveSpeedHorizontal;
        [ALHeader("水平移动时的倾斜角度")]
        public float moveLeanAngle = 60;
        [ALHeader("倾斜速度")]
        public float leanSpeed = 90;
        [ALHeader("碰撞时的倾斜震动")]
        public float collisionLeanIntensity = 30f;
        public float collisionLeanTime = 0.5f;
        public float collisionShakeFrequency = 8f;
        [ALHeader("特效相关")
        ,ALInfo("依次是：父节点，获得奖励特效，受击特效")]
        public Transform sfxParent;
        public long rewardGetSfxId;
        public long damageHitSfxId;
        [ALHeader("键盘操作时的移动位移量")]
        public float keyboardHorizontalMovement = 2;
        [ALHeader("拥有护盾时显示的对象列表")]
        public List<GameObject> listHaveShieldShow;
        public List<GameObject> listHaveShieldHide;
        [ALHeader("护盾破损时音效id")]
        public long shieldDamagedAudioId;
        [ALHeader("加速时的音效id")]
        public long boostAudioId;


        public void setHaveShield(bool _have)
        {
            ALUGUICommon.setGameObjEnable(listHaveShieldShow, false);
            ALUGUICommon.setGameObjEnable(listHaveShieldHide, false);
            ALUGUICommon.setGameObjEnable(_have ? listHaveShieldShow : listHaveShieldHide, true);
        }
        
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}