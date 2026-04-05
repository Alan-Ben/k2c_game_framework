using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 经营技能进度条
    /// </summary>
    public class GGUIWndConsortBusinessSkillSlider : _ANPGGUIBasicSubWnd<GGUIMonoConsortBusinessSkillSlider>
    {
        private _IGGUIWndUnLockConsortDetailBusinessPageParam _m_param;
        private GGottenConsortInfo _m_iConsortInfo;//妃子信息

        [NotNull] private List<ConsortBusinessSkillRefObj> _m_lBusinessRefObjList = new List<ConsortBusinessSkillRefObj>();//经营技能的配表数据
        private long _m_lSliderMaxValue = 0;//进度条最大值
        
        [NotNull] private List<GGUIPrefabSubWndConsortBusinessSkillItem> _m_lBusinessSkillItemWndList = new List<GGUIPrefabSubWndConsortBusinessSkillItem>();
        
        /// <summary>
        /// //当前选中的技能item, 不要直接使用, 要调用nowSelectSkillItem
        /// </summary>
        private GGUIPrefabSubWndConsortBusinessSkillItem _m_wNowSelectSkillItem;
        
        private bool _m_bSkillItemWndInitDone = false;
        private Action _m_aOnSkillItemWndInitDone;
        
        public GGUIWndConsortBusinessSkillSlider(GGUIMonoConsortBusinessSkillSlider _wnd, _IGGUIWndUnLockConsortDetailBusinessPageParam _param) : base(_wnd)
        {
            _m_param = _param;
            initWnd();
        }

        public event Action<GGUIPrefabSubWndConsortBusinessSkillItem> onSkillItemClick; 
        
        public long nowSelectSkillId { get { return _m_param?.consortDetailBusinessPageSelectSkillId ?? 0; } }
        public GGUIPrefabSubWndConsortBusinessSkillItem nowSelectSkillItem
        {
            get
            {
                if (_m_wNowSelectSkillItem == null || _m_wNowSelectSkillItem.businessSkillRefObj == null ||
                    _m_wNowSelectSkillItem.businessSkillRefObj.id != nowSelectSkillId)
                {
                    _m_wNowSelectSkillItem = _getBusinessSKillItemWnd(nowSelectSkillId);
                }
                
                return _m_wNowSelectSkillItem;
            }
        }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            _m_lBusinessRefObjList.Clear();
            _m_lBusinessRefObjList.AddRange(GRefdataCoreMgr.instance.consortBusinessSkillRefCore.refList);
            _m_lBusinessRefObjList.Sort(ConsortBusinessSkillRefObj.sort);
            _m_lSliderMaxValue = _m_lBusinessRefObjList.GetLast()?.unlock_need_intimacy ?? 0;//记录进度条最大值
            
            _initSlider();
            _initSKillItemList();//初始化技能item
        }
        
        protected override void _onDiscard()
        {
            onSkillItemClick = null;

            _m_lBusinessRefObjList.Clear();

            _discardSkillItem();
        }
        
        protected override void _onShowWnd()
        {
            _dealAllBusinessSkillItemList(_itemWnd =>
            {
                if (_itemWnd != null)
                {
                    _itemWnd.regLoadDoneDelegate(() =>
                    {
                        _itemWnd?.showWnd();
                    });
                }
            });
            
            setSelectSkill(_m_param?.consortDetailBusinessPageSelectSkillId ?? 0);
        }

        protected override void _onHideWnd()
        {
            _dealAllBusinessSkillItemList((_itemWnd) =>
            {
                if (_itemWnd != null)
                {
                    _itemWnd.setSelect((false));
                    _itemWnd.hideWnd();
                }
            });
            _m_wNowSelectSkillItem = null;//
        }

        protected override void _onReset()
        {
            _dealAllBusinessSkillItemList((_itemWnd) =>
            {
                if (_itemWnd != null)
                {
                    _itemWnd.setSelect((false));
                    _itemWnd.resetWnd();
                }
            });
            _m_wNowSelectSkillItem = null;//
        }
        
        /// <summary>
        /// 初始化进度条
        /// </summary>
        private void _initSlider()
        {
            if(wnd == null)
                return;

            if (wnd.slider != null)
            {
                wnd.slider.wholeNumbers = true;
                wnd.slider.minValue = 0;
                wnd.slider.maxValue = _m_lSliderMaxValue;

                // _m_fPerValueSliderLength = wnd.perValueSliderLength;
                // if (wnd.perValueSliderLength <= 0)
                // {
                //     Debug.LogError($"[GGUIWndConsortBusinessSkillSlider _initSlider] perValueSliderLength应该配置大于0的值, 当前值为:{wnd.perValueSliderLength}, 代码会使用默认1", wnd);
                //     _m_fPerValueSliderLength = 1;
                // }
                //
                // float sliderLen = _m_fPerValueSliderLength * _m_lSliderMaxValue;
                // RectTransform sliderRectTransform = (RectTransform) wnd.slider.transform;
                // if (sliderRectTransform != null)
                // {
                //     sliderRectTransform.sizeDelta = new Vector2(sliderLen, sliderRectTransform.sizeDelta.y);
                // }
            }
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
            
            _dealAllBusinessSkillItemList((_itemWnd) =>
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
            if (wnd == null || wnd.itemPrefab == null)
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
            for (int i = 0; i < _m_lBusinessRefObjList.Count; i++)
            {
                ConsortBusinessSkillRefObj businessSkillRefObj = _m_lBusinessRefObjList[i];
                if(businessSkillRefObj == null)
                    continue;
                
                stepCounter.chgTotalStepCount(1);
                GGUIPrefabSubWndConsortBusinessSkillItem skillItem = new GGUIPrefabSubWndConsortBusinessSkillItem(businessSkillRefObj, null, wnd.itemPrefab);
                skillItem.onItemClick += _onItemClick;
                _m_lBusinessSkillItemWndList.Add(skillItem);
                float itemXPosition = i * wnd.itemInterval;//计算item的x坐标
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
            setSelectSkill(nowSelectSkillId);

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

            _m_wNowSelectSkillItem = null;
            _m_bSkillItemWndInitDone = false;
        }

        private void _regSkillItemWndInitDone(Action _action)
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
        private void _dealAllBusinessSkillItemList(Action<GGUIPrefabSubWndConsortBusinessSkillItem> _action)
        {
            if(_action == null)
                return;
            
            _m_lBusinessSkillItemWndList.ForEach(_action);
        }

        /// <summary>
        /// 获取技能item窗口
        /// </summary>
        /// <returns></returns>
        private GGUIPrefabSubWndConsortBusinessSkillItem _getBusinessSKillItemWnd(long _skillId)
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

            GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd = _getBusinessSKillItemWnd(_businessSkillInfo.skillId);
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
            GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd = _getBusinessSKillItemWnd(_skillId);
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
            GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd = _getBusinessSKillItemWnd(_skillId);
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
            _dealAllBusinessSkillItemList((_itemWnd) =>
            {
                _itemWnd?.setBusinessSkillInfo(null);
            });
        }

        /// <summary>
        /// 设置选中某一技能
        /// </summary>
        /// <param name="_skillId"></param>
        public void setSelectSkill(long _skillId)
        {
            nowSelectSkillItem?.setSelect(false);

            if(_m_param != null)
                _m_param.consortDetailBusinessPageSelectSkillId = _skillId;

            GGUIPrefabSubWndConsortBusinessSkillItem selectItemWnd = nowSelectSkillItem;
            if (selectItemWnd != null)//若找到对应技能item, 则选中
            {
                selectItemWnd.setSelect(true);
            }
            else//否则直接选中第一个技能
            {
                selectFirstSKill();
            }
        }

        /// <summary>
        /// 选中第一个技能
        /// </summary>
        public void selectFirstSKill()
        {
            nowSelectSkillItem?.setSelect(false);
            
            if(_m_param != null)
                _m_param.consortDetailBusinessPageSelectSkillId = _m_lBusinessRefObjList.GetFirst()?.id ?? 0;
            
            GGUIPrefabSubWndConsortBusinessSkillItem selectItemWnd = nowSelectSkillItem;
            if (selectItemWnd != null)
            {
                selectItemWnd?.setSelect(true);
            }
            else
            {
                selectItemWnd = _m_lBusinessSkillItemWndList.GetFirst();
                
                if(_m_param != null)
                    _m_param.consortDetailBusinessPageSelectSkillId = selectItemWnd?.businessSkillRefObj?.id ?? 0;
                selectItemWnd?.setSelect(true);
            }
        }
        
        private void _onItemClick(GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd)
        {
            onSkillItemClick?.Invoke(itemPrefabSubWnd);
        }
        
        #endregion
        
    }
}