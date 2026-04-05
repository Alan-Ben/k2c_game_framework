using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public abstract class _AGGUICustomMonoAchieveStepItemWnd: MonoBehaviour
    {
        [ALHeader("阶段id")]
        public int stepId;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("图标")]
        public RawImage icon;
        [ALHeader("目标数量")]
        private TextEx txtTargetCount;
        [ALHeader("选中显示")]
        public List<GameObject> selectedShow;
        [ALHeader("本身的状态列表")] 
        public List<NPCommonEnumStatInfo<ENPCommonGetStat>> statInfos;
        [ALHeader("选中加载的资源id")]
        public long ui_path_id;
        
        
        #if NP_GAME
        
        private AchieveInfo _m_achieveInfo = null;//效果数据
        private AchieveStepInfo _m_achieveStepInfo;
        private NPGGuiWndTexture _m_iconWnd;
        
        private Action<_AGGUICustomMonoAchieveStepItemWnd> _m_onClickItem;
        private bool _m_isSelected;

        public AchieveInfo achieveInfo { get => _m_achieveInfo; }
        public AchieveStepInfo achieveStepInfo { get => _m_achieveStepInfo; }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_achieveId"></param>
        public void setInfo(long _achieveId)
        {
            _m_achieveInfo = NPPlayer.instance.achieveComp.getAchimentInfo(_achieveId);
            _m_achieveStepInfo = _m_achieveInfo?.getStepInfo(stepId);
            _refreshWnd();
        }

        public void regClickEvent(Action<_AGGUICustomMonoAchieveStepItemWnd> _clickAction)
        {
            _m_onClickItem += _clickAction;
        }

        public void unregClickEvent(Action<_AGGUICustomMonoAchieveStepItemWnd> _clickAction)
        {
            _m_onClickItem -= _clickAction;
        }

        private void _refreshWnd()
        {
            ENPCommonGetStat stepStat = _m_achieveInfo?.getStepRewardState(stepId) ?? ENPCommonGetStat.CAN_NOT_GET;
            NPCommonEnumStatInfo<ENPCommonGetStat>.setStat(statInfos, stepStat);

            if (null == _m_iconWnd && icon != null)
            {
                _m_iconWnd = new NPGGuiWndTexture(icon);
            }
            
            _m_iconWnd?.showWnd();
            _m_iconWnd?.setTexture(_getIcon());

            AchieveStepInfo stepInfo = _m_achieveInfo?.getStepInfo(stepId);
            long targetCount = stepInfo != null ? stepInfo.stepRefObj.process_count : 1;
            string targetCountStr = GCommon.getValueFormatStr(_m_achieveInfo?.achieveRefObj.process_num_format ?? EValueFormatType.NORMAL, targetCount);
            ALUGUICommon.setLabelTxt(txtTargetCount , targetCountStr);
        }

        protected abstract NPGTextureIndex _getIcon();

        private void OnEnable()
        {
            ALUGUICommon.combineBtnClick(btnClick, _clickSelected);
            _refreshWnd();
        }

        /// <summary>
        /// 点击选中的时候
        /// </summary>
        /// <param name="obj"></param>
        private void _clickSelected(GameObject obj)
        {
            _m_onClickItem?.Invoke(this);
        }
        
        //设置选中
        public void setSelected(bool _isSelected)
        {
            _m_isSelected = _isSelected;
            ALUGUICommon.setGameObjEnable(selectedShow, _m_isSelected);
            _onSelected(_m_isSelected);
        }

        protected abstract void _onSelected(bool _isSelected);

        private void OnDisable()
        {
            ALUGUICommon.uncombineBtnClick(btnClick, _clickSelected);
            _discardWnd();
        }

        private void _discardWnd()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;

            _onDiscardWnd();
        }

        protected abstract void _onDiscardWnd();
        public abstract void dealClickInfo();
        public abstract void dealGetReward();

#endif
    }
}