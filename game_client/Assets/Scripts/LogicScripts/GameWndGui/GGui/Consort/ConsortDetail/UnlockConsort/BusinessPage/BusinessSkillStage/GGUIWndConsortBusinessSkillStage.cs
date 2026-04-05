using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子经营技能阶段显示信息
    /// </summary>
    public class ConsortBusinessSkillStageShowInfo
    {
        private long _m_lSliderShowMinValue;//进度条显示的最小值
        private long _m_lSliderShowMaxValue;//进度条显示的最大值
        
        //进度条最大值和最小值的差值
        private long _m_lSliderShowValueRange;
        
        private List<ConsortBusinessSkillRefObj> _m_lBusinessSkillRefObjList;//显示的经营技能列表
        
        public ConsortBusinessSkillStageShowInfo(long _minValue, long _maxValue, List<ConsortBusinessSkillRefObj> _businessSkillRefList)
        {
            _m_lSliderShowMinValue = Math.Min(_minValue, _maxValue);
            _m_lSliderShowMaxValue = Math.Max(_minValue, _maxValue);
            
            _m_lSliderShowValueRange = _m_lSliderShowMaxValue - _m_lSliderShowMinValue;
            
            if(_businessSkillRefList != null)
                _m_lBusinessSkillRefObjList = new List<ConsortBusinessSkillRefObj>(_businessSkillRefList);
        }
        
        public long sliderShowMinValue { get { return _m_lSliderShowMinValue; } }
        public long sliderShowMaxValue { get { return _m_lSliderShowMaxValue; } }
        public long sliderShowValueRange { get { return _m_lSliderShowValueRange; } }
        public List<ConsortBusinessSkillRefObj> businessSkillRefObjList { get { return _m_lBusinessSkillRefObjList; } }
    }
    
    /// <summary>
    /// 妃子经营技能阶段
    /// </summary>
    public class GGUIWndConsortBusinessSkillStage : _ATALBasicUISubWnd<GGUIMonoConsortBusinessSkillStage>
    {
        private ConsortBusinessSkillStageShowInfo _m_iBusinessSkillStageShowInfo;//经营技能阶段显示信息
        private float _m_fSliderWidth;//进度条的宽度
        
        [NotNull] private List<GGUIPrefabSubWndConsortBusinessSkillItem> _m_lBusinessSkillItemWndList = new List<GGUIPrefabSubWndConsortBusinessSkillItem>();
        
        private bool _m_bSkillItemWndInitDone = false;
        private Action _m_aOnSkillItemWndInitDone;
        
        public GGUIWndConsortBusinessSkillStage(GGUIMonoConsortBusinessSkillStage _wnd) : base(_wnd)
        {
        }
        
        public ConsortBusinessSkillStageShowInfo businessSkillStageShowInfo { get { return _m_iBusinessSkillStageShowInfo; } }
        public event Action<GGUIPrefabSubWndConsortBusinessSkillItem> onSkillItemClick; 
        [NotNull] public List<GGUIPrefabSubWndConsortBusinessSkillItem> businessSkillItemWndList { get { return _m_lBusinessSkillItemWndList; } }
        
        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            onSkillItemClick = null;
            
            _discardSkillItem();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }
        
        /// <summary>
        /// 初始化进度条
        /// </summary>
        public void initSlider(ConsortBusinessSkillStageShowInfo _businessSkillStageShowInfo)
        {
            _m_iBusinessSkillStageShowInfo = _businessSkillStageShowInfo;
         
            if(wnd == null || wnd.slider == null)
                return;

            // 获取进度条的宽度
            _m_fSliderWidth = ((RectTransform) wnd.slider.gameObject.transform).rect.width;
            
            wnd.slider.minValue = _m_iBusinessSkillStageShowInfo?.sliderShowMinValue ?? 0;
            wnd.slider.maxValue = _m_iBusinessSkillStageShowInfo?.sliderShowMaxValue ?? 0;
            wnd.slider.wholeNumbers = true;

            _initSKillItemList();
        }
     
        /// <summary>
        /// 设置当前亲密度
        /// </summary>
        /// <param name="_nowIntimacy"></param>
        public void setNowIntimacy(long _nowIntimacy)
        {
            if(wnd == null)
                return;

            if (wnd.slider != null)
            {
                wnd.slider.value = _nowIntimacy;
            }
            
            dealAllBusinessSkillItemList((_itemWnd) =>
            {
                if (_itemWnd != null)
                {
                    _itemWnd.showWnd();
                    _itemWnd.setNowIntimacy(_nowIntimacy);
                }
            });
        }
        
        #region SKillItem

        /// <summary>
        /// 初始化技能item列表
        /// </summary>
        private void _initSKillItemList()
        {
            _discardSkillItem();//先销毁技能item列表
            if (wnd == null || wnd.itemParent == null || _m_iBusinessSkillStageShowInfo == null || _m_iBusinessSkillStageShowInfo.businessSkillRefObjList == null)
            {
                _m_aOnSkillItemWndInitDone?.Invoke();
                _m_aOnSkillItemWndInitDone = null;
                return;
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(() =>
            {
                _m_bSkillItemWndInitDone = true;
                _m_aOnSkillItemWndInitDone?.Invoke();
                _m_aOnSkillItemWndInitDone = null;
            });
            
            // 遍历经营技能配表列表数据
            for (int i = 0; i < _m_iBusinessSkillStageShowInfo.businessSkillRefObjList.Count; i++)
            {
                ConsortBusinessSkillRefObj businessSkillRefObj = _m_iBusinessSkillStageShowInfo.businessSkillRefObjList[i];
                if(businessSkillRefObj == null)
                    continue;
                
                stepCounter.chgTotalStepCount(1);
                GGUIPrefabSubWndConsortBusinessSkillItem skillItem = new GGUIPrefabSubWndConsortBusinessSkillItem(businessSkillRefObj, null, wnd.itemParent);
                skillItem.onItemClick += _onItemClick;
                _m_lBusinessSkillItemWndList.Add(skillItem);

                float itemXPosition = 0f;//item的x坐标
                if (_m_iBusinessSkillStageShowInfo.sliderShowValueRange > 0)
                {
                    long needIntimacyInterval = businessSkillRefObj.unlock_need_intimacy - _m_iBusinessSkillStageShowInfo.sliderShowMinValue;
                    itemXPosition = 1f * needIntimacyInterval / _m_iBusinessSkillStageShowInfo.sliderShowValueRange * _m_fSliderWidth;
                }
                
                skillItem.load(() =>
                {
                    if (skillItem.wnd == null)
                    {
                        stepCounter.addDoneStepCount();
                        return;
                    }

                    // 设置item位置
                    RectTransform itemWndRectTransform = (RectTransform) skillItem.wnd.transform;
                    itemWndRectTransform.anchoredPosition = new Vector2(itemXPosition, itemWndRectTransform.anchoredPosition.y);
                    
                    stepCounter.addDoneStepCount();
                });
            }

            stepCounter.addDoneStepCount();
        }
        
        /// <summary>
        /// 销毁技能item列表
        /// </summary>
        private void _discardSkillItem()
        {
            foreach (var item in _m_lBusinessSkillItemWndList)
            {
                if(item == null)
                    continue;
                
                item.onItemClick -= _onItemClick;
                item.setSelect(false);
                item.discard();
            }
            _m_lBusinessSkillItemWndList.Clear();

            _m_bSkillItemWndInitDone = false;
        }

        public void regSkillItemWndInitDone(Action _action)
        {
            if(_m_bSkillItemWndInitDone)
            {
                _action?.Invoke();
                return;
            }
            
            _m_aOnSkillItemWndInitDone += _action;
        }

        /// <summary>
        /// 处理所有技能item列表
        /// </summary>
        /// <param name="_action"></param>
        public void dealAllBusinessSkillItemList(Action<GGUIPrefabSubWndConsortBusinessSkillItem> _action)
        {
            if(_action == null)
                return;
            
            _m_lBusinessSkillItemWndList.ForEach(_action);
        }

        /// <summary>
        /// 获取技能item窗口
        /// </summary>
        /// <returns></returns>
        public GGUIPrefabSubWndConsortBusinessSkillItem getBusinessSKillItemWnd(long _skillId)
        {
            foreach (GGUIPrefabSubWndConsortBusinessSkillItem itemWnd in _m_lBusinessSkillItemWndList)
            {
                if (itemWnd != null && itemWnd.businessSkillRefObj != null &&
                    itemWnd.businessSkillRefObj.id == _skillId)
                    return itemWnd;
            }

            return null;
        }
        
        /// <summary>
        /// 设置当前服务端经营技能信息
        /// </summary>
        /// <param name="_businessSkillInfo"></param>
        public void setBusinessSkillInfo(ConsortBusinessSkillInfo _businessSkillInfo)
        {
            if(_businessSkillInfo == null)
                return;

            GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd = getBusinessSKillItemWnd(_businessSkillInfo.skillId);
            if (itemPrefabSubWnd != null)
            {
                itemPrefabSubWnd.showWnd();
                itemPrefabSubWnd.setBusinessSkillInfo(_businessSkillInfo);
            }
        }

        public void setBusinessSkillInfo(List<ConsortBusinessSkillInfo> _businessSkillInfoList)
        {
            if(_businessSkillInfoList == null)
                return;

            foreach (ConsortBusinessSkillInfo skillInfo in _businessSkillInfoList)
            {
                setBusinessSkillInfo(skillInfo);
            }
        }

        /// <summary>
        /// 播放加成提升成功特效
        /// </summary>
        /// <param name="_skillId"></param>
        public void playProAddSuccessSfx(long _skillId)
        {
            GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd = getBusinessSKillItemWnd(_skillId);
            if (itemPrefabSubWnd != null)
            {
                itemPrefabSubWnd.playProAddSuccessSfx();
            }
        }
        
        /// <summary>
        /// 播放加成提升失败特效
        /// </summary>
        /// <param name="_skillId"></param>
        public void playProAddFailSfx(long _skillId)
        {
            GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd = getBusinessSKillItemWnd(_skillId);
            if (itemPrefabSubWnd != null)
            {
                itemPrefabSubWnd.playProAddFailSfx();
            }
        }
        
        /// <summary>
        /// 重置所有item显示信息
        /// </summary>
        public void resetAllItemShowInfo()
        {
            dealAllBusinessSkillItemList((_itemWnd) =>
            {
                _itemWnd?.setBusinessSkillInfo(null);
            });
        }
        
        private void _onItemClick(GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd)
        {
            onSkillItemClick?.Invoke(itemPrefabSubWnd);
        }
        
        #endregion
    }
}