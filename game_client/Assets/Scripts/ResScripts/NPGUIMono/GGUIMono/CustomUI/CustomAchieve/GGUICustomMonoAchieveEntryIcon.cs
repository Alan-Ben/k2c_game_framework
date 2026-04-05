using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class CustomMonoAchieveEntryIconInfo
    {
        [ALHeader("阶段id")]
        public int stepId;
        [ALHeader("图标")]
        public NPGTextureIndex iconIndex;
    }
    public class GGUICustomMonoAchieveEntryIcon : MonoBehaviour
    {
        [ALHeader("成就id")]
        public int achieveId;
        [ALHeader("图标")]
        public RawImage icon;
        [ALHeader("阶段图标配置列表")]
        public List<CustomMonoAchieveEntryIconInfo> stepIconList;
        
        #if NP_GAME

        private NPGGuiWndTexture _m_iconWnd;
        private AchieveInfo _m_achieveInfo;

        private void OnEnable()
        {
            _m_achieveInfo = NPPlayer.instance.achieveComp.getAchimentInfo(achieveId);
            
#if UNITY_EDITOR
            if (null != _m_achieveInfo && stepIconList.Count != _m_achieveInfo.stepInfoList.Count)
            {
                ALLog.Error($"{gameObject.name}上配置的成就阶段图标列表和refdata里面的的成就阶段数量不一致");
            }
#endif
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_ACHIEVE_INFO_CHG, _refreshWnd);
        }

        private void OnDisable()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
            WinMsg.UnregisterMsgAct(WinMsgType.ON_ACHIEVE_INFO_CHG, _refreshWnd);
        }
        
        private void _refreshWnd()
        {
            if (null == _m_achieveInfo)
                return;
            
            CustomMonoAchieveEntryIconInfo selectedItem = null; 
            foreach (CustomMonoAchieveEntryIconInfo entryIconInfo in stepIconList)
            {
                if(null == entryIconInfo)
                    continue;

                if (selectedItem == null)
                {
                    selectedItem = entryIconInfo;
                }
                else
                {
                    if(_m_achieveInfo == null)
                        continue;
                    if (_m_achieveInfo.getStepRewardState(entryIconInfo.stepId) < _m_achieveInfo.getStepRewardState(selectedItem.stepId))
                    {
                        selectedItem = entryIconInfo;
                    }
                }
            }
            
            if (null == _m_iconWnd && icon != null)
            {
                _m_iconWnd = new NPGGuiWndTexture(icon);
            }
            
            _m_iconWnd?.showWnd();
            _m_iconWnd?.setTexture(selectedItem?.iconIndex);
            
        }

#endif
    }
}