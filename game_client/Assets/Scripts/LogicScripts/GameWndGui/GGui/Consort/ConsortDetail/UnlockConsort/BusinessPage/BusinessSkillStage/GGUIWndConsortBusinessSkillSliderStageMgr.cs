using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIWndConsortBusinessSkillSliderStageMgr : _ATALBasicUISubWnd<GGUIMonoConsortBusinessSkillSliderStageMgr>, _IScrollerSmoothMovable
    {
        private _IGGUIWndUnLockConsortDetailBusinessPageParam _m_param;
        private Func<GGUIPrefabSubWndConsortBusinessSkillItem, bool> _m_fNeedShowRedTipFunc;
        
        [NotNull] private List<GGUIWndConsortBusinessSkillSliderStage> _m_lSliderStageList = new List<GGUIWndConsortBusinessSkillSliderStage>();
        
        /// <summary>
        /// //当前选中的技能item, 不要直接使用, 要调用nowSelectSkillItem
        /// </summary>
        private GGUIPrefabSubWndConsortBusinessSkillItem _m_wNowSelectSkillItem;
        private bool _m_bSkillItemWndInitDone = false;
        private Action _m_aOnSkillItemWndInitDone;
        
        private long _m_lScrollMoveSerialize = 0;
        
        public GGUIWndConsortBusinessSkillSliderStageMgr(GGUIMonoConsortBusinessSkillSliderStageMgr _wnd, _IGGUIWndUnLockConsortDetailBusinessPageParam _param, Func<GGUIPrefabSubWndConsortBusinessSkillItem, bool> _needShowRedTipFunc) : base(_wnd)
        {
            _m_param = _param;
            _m_fNeedShowRedTipFunc = _needShowRedTipFunc;
            initWnd();
        }

        public event Action<GGUIPrefabSubWndConsortBusinessSkillItem> onSkillItemClick;
        public long nowSelectSkillId { get { return _m_param?.consortDetailBusinessPageSelectSkillId ?? 0; } }
        public GGUIPrefabSubWndConsortBusinessSkillItem nowSelectSkillItem
        {
            get
            {
                return _m_wNowSelectSkillItem;
            }
        }
        
        public ScrollRect scrollRect { get { return wnd == null ? null : wnd.skillScrollRect; } }
        public long serialize { get { return _m_lScrollMoveSerialize; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _initBusinessSkillSliderStage();
        }
        
        protected override void _onDiscard()
        {
            onSkillItemClick = null;

            _discardBusinessSkillSliderStage();
        }
        
        protected override void _onShowWnd()
        {
            foreach (GGUIWndConsortBusinessSkillSliderStage sliderStage in _m_lSliderStageList)
            {
                sliderStage?.showWnd();
            }
        }

        protected override void _onHideWnd()
        {
            _m_lScrollMoveSerialize = ALSerializeOpMgr.next();
            
            foreach (GGUIWndConsortBusinessSkillSliderStage sliderStage in _m_lSliderStageList)
            {
                sliderStage?.hideWnd();
            }
            
            _m_wNowSelectSkillItem = null;//
        }

        protected override void _onReset()
        {
            foreach (GGUIWndConsortBusinessSkillSliderStage sliderStage in _m_lSliderStageList)
            {
                sliderStage?.resetWnd();
            }
            
            _m_wNowSelectSkillItem = null;//
        }
        
        /// <summary>
        /// 设置当前亲密度
        /// </summary>
        /// <param name="_nowIntimacy"></param>
        public void setNowIntimacy(long _nowIntimacy)
        {
            if(wnd == null)
                return;

            _dealAllBusinessSkillStageList((skillStage) =>
            {
                skillStage?.setNowIntimacy(_nowIntimacy);
            });
        }
        
        /// <summary>
        /// 初始化妃子经营技能进度条阶段
        /// </summary>
        private void _initBusinessSkillSliderStage()
        {
            if(wnd == null)
                return;
            
            _discardBusinessSkillSliderStage();
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(() =>
            {
                _m_bSkillItemWndInitDone = true;
                _m_aOnSkillItemWndInitDone?.Invoke();
                _m_aOnSkillItemWndInitDone = null;
            });
            
            if (wnd.sliderStageList != null)
            {
                foreach (GGUIMonoConsortBusinessSkillSliderStage mono in wnd.sliderStageList)
                {
                    if(mono == null)
                        continue;

                    stepCounter.chgTotalStepCount(1);

                    GGUIWndConsortBusinessSkillSliderStage itemWnd = new GGUIWndConsortBusinessSkillSliderStage(wnd.skillItemParent, _m_fNeedShowRedTipFunc, mono);
                    itemWnd.onSkillItemClick += _onItemClick;
                    _m_lSliderStageList.Add(itemWnd);
                    
                    itemWnd.initWnd();
                    itemWnd.regSkillItemWndInitDone(() =>
                    {
                        stepCounter.addDoneStepCount();
                    });
                }
            }
            
            stepCounter.addDoneStepCount();
        }
        
        /// <summary>
        /// 销毁
        /// </summary>
        private void _discardBusinessSkillSliderStage()
        {
            _m_aOnSkillItemWndInitDone = null;
            
            foreach (var skillSliderStage in _m_lSliderStageList)
            {
                if(skillSliderStage == null)
                    continue;
                
                skillSliderStage.onSkillItemClick -= _onItemClick;
                skillSliderStage.discard();
            }
            _m_lSliderStageList.Clear();

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
        /// 处理所有技能Stage列表
        /// </summary>
        /// <param name="_action"></param>
        private void _dealAllBusinessSkillStageList(Action<GGUIWndConsortBusinessSkillSliderStage> _action)
        {
            if(_action == null)
                return;

            foreach (var skillStage in _m_lSliderStageList)
            {
                if(skillStage == null)
                    continue;

                _action(skillStage);
            }
        }
        
        /// <summary>
        /// 处理所有技能item列表
        /// </summary>
        /// <param name="_action"></param>
        private void _dealAllBusinessSkillItemList(Action<GGUIPrefabSubWndConsortBusinessSkillItem> _action)
        {
            if(_action == null)
                return;

            _dealAllBusinessSkillStageList((_skillStage) =>
            {
                if (_skillStage == null)
                    return;

                _skillStage.dealAllBusinessSkillItemList(_action);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private GGUIPrefabSubWndConsortBusinessSkillItem _findBusinessSkillItem(Predicate<GGUIPrefabSubWndConsortBusinessSkillItem> _match)
        {
            if (_match == null)
                return null;

            GGUIPrefabSubWndConsortBusinessSkillItem skillItem = null;
            foreach (var skillStage in _m_lSliderStageList)
            {
                if(skillStage == null)
                    continue;

                skillItem = skillStage.findBusinessSkillItem(_match);
                if (skillItem != null)
                    return skillItem;
            }

            return null;
        }

        /// <summary>
        /// 获取技能item窗口
        /// </summary>
        /// <returns></returns>
        private GGUIPrefabSubWndConsortBusinessSkillItem _getBusinessSKillItemWnd(long _skillId)
        {
            GGUIPrefabSubWndConsortBusinessSkillItem itemWnd = null;
            foreach (var skillStage in _m_lSliderStageList)
            {
                if (skillStage == null)
                    continue;

                itemWnd = skillStage.getBusinessSKillItemWnd(_skillId);
                
                if(itemWnd != null)
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
        /// <param name="_skillId">技能ID</param>
        /// <param name="_needMoveScrollRect">是否需要滚动到选中的技能</param>
        public void setSelectSkill(long _skillId, bool _needMoveScrollRect = true, bool _moveImmediate = true)
        {
            GGUIPrefabSubWndConsortBusinessSkillItem selectItemWnd = _getBusinessSKillItemWnd(_skillId);

            if (selectItemWnd != null)
                _setSelectSKillItem(selectItemWnd, _needMoveScrollRect, _moveImmediate);
            else
                selectFirstNotMaxLvlSKill(_needMoveScrollRect, _moveImmediate);
        }

        /// <summary>
        /// 选中第一个技能
        /// </summary>
        public void selectFirstSKill(bool _needMoveScrollRect = true, bool _moveImmediate = true)
        {
            GGUIWndConsortBusinessSkillSliderStage firstBusinessSkillStage = _m_lSliderStageList.GetFirst();
            if(firstBusinessSkillStage == null)
                return;

            GGUIPrefabSubWndConsortBusinessSkillItem skillItem = firstBusinessSkillStage.businessSkillItemWndList.GetFirst();
            _setSelectSKillItem(skillItem, _needMoveScrollRect, _moveImmediate);
        }

        /// <summary>
        /// 选中第一个未满级技能
        /// </summary>
        public void selectFirstNotMaxLvlSKill(bool _needMoveScrollRect = true, bool _moveImmediate = true)
        {
            GGUIPrefabSubWndConsortBusinessSkillItem skillItem = _findBusinessSkillItem((skillItem) =>
            {
                return skillItem != null && skillItem.businessSkillInfo != null && !skillItem.businessSkillInfo.isReachAddMax;
            });

            if (skillItem != null)
            {
                _setSelectSKillItem(skillItem, _needMoveScrollRect, _moveImmediate);
            }
            else
            {
                selectFirstSKill(_needMoveScrollRect, _moveImmediate);
            }
        }

        private void _setSelectSKillItem(GGUIPrefabSubWndConsortBusinessSkillItem _skillItem, bool _needMoveScrollRect, bool _moveImmediate)
        {
            if(_skillItem == null)
                return;
            
            _m_wNowSelectSkillItem?.setSelect(false);
            _m_wNowSelectSkillItem = _skillItem;
            if(_m_param != null)
                _m_param.consortDetailBusinessPageSelectSkillId = _m_wNowSelectSkillItem?.businessSkillRefObj?.id ?? 0;
            _m_wNowSelectSkillItem?.setSelect(true);
            
            if(_needMoveScrollRect)
                _scrollToSkillItem(_m_wNowSelectSkillItem, _moveImmediate);
        }
        
        /// <summary>
        /// 滚动到指定的技能item，使其位于可视区域中心
        /// </summary>
        /// <param name="_skillItem">技能item</param>
        private void _scrollToSkillItem(GGUIPrefabSubWndConsortBusinessSkillItem _skillItem, bool _moveImmediate = true)
        {
            if (_skillItem == null || _skillItem.wnd == null || wnd == null || wnd.skillScrollRect == null)
                return;

            // 获取技能item的RectTransform
            RectTransform itemRect = _skillItem.wnd.GetComponent<RectTransform>();
            if (itemRect == null)
                return;

            // 获取ScrollRect的相关组件
            ScrollRect scrollRect = wnd.skillScrollRect;
            if (scrollRect.content == null || scrollRect.viewport == null)
                return;

            // 获取技能item在父节点中的相对位置
            Vector3 itemRectCenterWorldPos = itemRect.TransformPoint(itemRect.rect.center);
            Vector3 itemRectCenterInScrollRectContentPos = scrollRect.content.InverseTransformPoint(itemRectCenterWorldPos);

            _m_lScrollMoveSerialize = ALSerializeOpMgr.next();
            
            // 计算滚动位置，使技能item位于可视区域中心
            if (scrollRect.horizontal)
            {
                // 水平滚动
                float contentWidth = scrollRect.content.rect.width;
                float viewportWidth = scrollRect.viewport.rect.width;
                
                if (contentWidth > viewportWidth)
                {
                    // 计算让item位于中心的normalized位置
                    float normalizedX = (itemRectCenterInScrollRectContentPos.x - viewportWidth * 0.5f) / (contentWidth - viewportWidth);
                    normalizedX = Mathf.Clamp01(normalizedX);

                    if (_moveImmediate)
                    {
                        scrollRect.horizontalNormalizedPosition = normalizedX;
                    }
                    else
                    {
                        new ScrollerSmoothMoveTaskHorizontal(this, normalizedX, wnd.scrollMoveTimeS, () =>
                        {
                        }).deal();
                    }
                }
            }
            
            if (scrollRect.vertical)
            {
                // 垂直滚动
                float contentHeight = scrollRect.content.rect.height;
                float viewportHeight = scrollRect.viewport.rect.height;
                
                if (contentHeight > viewportHeight)
                {
                    // 计算让item位于中心的normalized位置
                    float itemCenterY = itemRectCenterInScrollRectContentPos.y;
                    // 注意：垂直滚动的normalizedPosition是从下到上的，需要转换
                    float normalizedY = (contentHeight + itemCenterY - viewportHeight * 0.5f) / (contentHeight - viewportHeight);
                    normalizedY = Mathf.Clamp01(normalizedY);

                    if (_moveImmediate)
                    {
                        scrollRect.verticalNormalizedPosition = normalizedY;
                    }
                    else
                    {
                        new ScrollerSmoothMoveTaskVertical(this, normalizedY, wnd.scrollMoveTimeS, () =>
                        {
                        }).deal();
                    }
                }
            }
        }
        
        private void _onItemClick(GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd)
        {
            onSkillItemClick?.Invoke(itemPrefabSubWnd);
        }
        
        public void refreshRedTipShow(long _skillId)
        {
            _dealAllBusinessSkillItemList((_itemWnd) =>
            {
                if(_itemWnd == null || _itemWnd.businessSkillRefObj == null || _itemWnd.businessSkillRefObj.id != _skillId)
                    return;
                
                _itemWnd.refreshRedTipShow();
            });
        }
        
        public void refreshRedTipShow()
        {
            _dealAllBusinessSkillItemList((_itemWnd) =>
            {
                _itemWnd?.refreshRedTipShow();
            });
        }
    }
}