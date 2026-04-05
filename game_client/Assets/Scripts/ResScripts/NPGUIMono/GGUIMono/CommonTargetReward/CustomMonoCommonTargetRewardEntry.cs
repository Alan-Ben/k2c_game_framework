using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 未解锁妃子详情页面tab配置
    /// </summary>
    [Serializable]
    public class CustomMonoCommonTargetRewardEntrySetting
    {
        [ALHeader("对应ID(common_target_reward)")]
        public long id;
        [ALHeader("对应显示时候加载的icon")]
        public NPGTextureIndex textureIndex;
    }
    
    /// <summary>
    /// 目标奖励入口
    /// </summary>
    public class CustomMonoCommonTargetRewardEntry : MonoBehaviour
    {
        [ALHeader("对应的id列表")]
        public List<CustomMonoCommonTargetRewardEntrySetting> idList;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("打开弹窗的资源样式id")]
        public long wndResId;
        [ALHeader("任务全都完成隐藏的go列表")]
        public List<GameObject> hideGoList;
        [ALHeader("头像")]
        public RawImage imgIcon;//头像

#if NP_GAME
        // 头像
        private NPGGuiWndTexture _m_wtIconWnd;
        
        private void Awake()
        {
            if (imgIcon != null)
                _m_wtIconWnd = new NPGGuiWndTexture(imgIcon);
            
            ALUGUICommon.combineBtnClick(btnClick, _onBtnClick);
        }

        private void OnEnable()
        {
            if(null == idList || idList.Count == 0)
                return;

            //默认选中第一个
            NPGTextureIndex selectItemIndex = null;
            foreach (CustomMonoCommonTargetRewardEntrySetting item in idList)
            {
                if(null == item)
                    continue;

                ECommonRewardType commonRewardType = NPPlayer.instance.commonTargetRewardComp.getRewardType(item.id);
                
                //第一个还不可领取的选中
                if (null == selectItemIndex && commonRewardType == ECommonRewardType.NOT_GET_REWARD)
                {
                    selectItemIndex = item.textureIndex;
                    continue;
                }
                
                //有可领奖的直接选中可领取
                if (commonRewardType == ECommonRewardType.CAN_GET_REWARD)
                {
                    selectItemIndex = item.textureIndex;
                    break;
                }
            }

            //找不到说明全都完成了
            if (null == selectItemIndex)
            {
                ALUGUICommon.setGameObjEnable(hideGoList,false);
            }
            //展示找到的icon
            else
            {
                if (_m_wtIconWnd != null)
                {
                    _m_wtIconWnd.setTexture(selectItemIndex);
                    _m_wtIconWnd.showWnd();
                }   
            }
        }

        private void OnDisable()
        {
            if (_m_wtIconWnd != null) 
                _m_wtIconWnd.discardTexture();
        }
        
        private void _onBtnClick(GameObject _)
        {
            openTargetRewardWnd(null);
        }
        
        public void openTargetRewardWnd(Action _onCloseNode)
        {
            GGUIWndCommonTargetReward wnd = new GGUIWndCommonTargetReward(wndResId);
            QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(wnd, _onCloseNode, EUIQueueStageType.MAIN, UINodeTagConst.C_COMMON_TARGET_REWARD));
        }
#endif
    }
}