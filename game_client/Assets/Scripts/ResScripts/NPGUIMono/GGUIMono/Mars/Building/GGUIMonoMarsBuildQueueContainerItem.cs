using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildQueueContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("队列名字")]
        public Text txtQueueName;
        [ALHeader("使用中的描述")]
        public Text txtUsingDesc;
        [ALHeader("剩余时间")]
        public Text txtRemainTime;
        public Slider sldRemainTime;
        [ALHeader("临时解锁剩余时间")]
        public Text txtTempUnlockRemainTime;
        [ALHeader("跳转按钮")]
        public GameObject btnJump;
        [ALHeader("是否有队列的状态显示")]
        public List<GameObject> listIdleShow;
        public List<GameObject> listUsingShow;
        [ALHeader("解锁相关的状态显示")]
        public List<GameObject> listLockShow;
        public List<GameObject> listTempUnlockShow;
        public List<GameObject> listUnlockShow;


        public void setIdleState(bool _isIdle)
        {
            ALUGUICommon.setGameObjEnable(listIdleShow, false);
            ALUGUICommon.setGameObjEnable(listUsingShow, false);
            ALUGUICommon.setGameObjEnable(_isIdle ? listIdleShow : listUsingShow, true);
        }
        public void setLockState(bool _isLocked, bool _isTempUnlock)
        {
            ALUGUICommon.setGameObjEnable(listLockShow, false);
            ALUGUICommon.setGameObjEnable(listTempUnlockShow, false);
            ALUGUICommon.setGameObjEnable(listUnlockShow, false);

            if (_isTempUnlock)
                ALUGUICommon.setGameObjEnable(listTempUnlockShow, true);
            else
                ALUGUICommon.setGameObjEnable(_isLocked ? listLockShow : listUnlockShow, true);
        }
    }
}