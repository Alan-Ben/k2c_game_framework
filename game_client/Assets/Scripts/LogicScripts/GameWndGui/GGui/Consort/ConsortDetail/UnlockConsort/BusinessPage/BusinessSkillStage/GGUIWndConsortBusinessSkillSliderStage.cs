using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIWndConsortBusinessSkillSliderStage : _ATALBasicUISubWnd<GGUIMonoConsortBusinessSkillSliderStage>
    {
        private long _m_lSldMinValue;//进度条最小值
        private long _m_lSldMaxValue;//进度条最大值
        private Image _m_sldFillImage;//进度条填充区域
        private Transform _m_skillItemParent;//技能item父节点
        
        private float _m_fSldLeftPosX;//进度条左边界位置
        private float _m_fSldWidth;//进度条宽度
        [NotNull] private List<KeyValuePair<long, float>> _m_lSldValuePositionList = new List<KeyValuePair<long, float>>();//进度条特殊值对应位置
        
        [NotNull] private List<GGUIPrefabSubWndConsortBusinessSkillItem> _m_lBusinessSkillItemWndList = new List<GGUIPrefabSubWndConsortBusinessSkillItem>();
        
        private bool _m_bSkillItemWndInitDone = false;
        private Action _m_aOnSkillItemWndInitDone;
        
        private Func<GGUIPrefabSubWndConsortBusinessSkillItem, bool> _m_fNeedShowRedTipFunc;
        
        public GGUIWndConsortBusinessSkillSliderStage(Transform _skillItemParent, Func<GGUIPrefabSubWndConsortBusinessSkillItem, bool> _needShowRedTipFunc, GGUIMonoConsortBusinessSkillSliderStage _wnd) : base(_wnd)
        {
            _m_skillItemParent = _skillItemParent;
            _m_fNeedShowRedTipFunc = _needShowRedTipFunc;
        }

        public event Action<GGUIPrefabSubWndConsortBusinessSkillItem> onSkillItemClick; 
        [NotNull] public List<GGUIPrefabSubWndConsortBusinessSkillItem> businessSkillItemWndList { get { return _m_lBusinessSkillItemWndList; } }
        
        protected override void _onWndInitDone()
        {
            _initSlider();
        }
        
        protected override void _onDiscard()
        {
            onSkillItemClick = null;

            _m_sldFillImage = null;
            _m_skillItemParent = null;
            
            _m_lSldValuePositionList.Clear();
            
            _discardSkillItem();
        }
        
        protected override void _onShowWnd()
        {
            dealAllBusinessSkillItemAfterInitDone((_itemWnd) =>
            {
                _itemWnd?.showWnd();
            });
        }

        protected override void _onHideWnd()
        {
            dealAllBusinessSkillItemAfterInitDone((_itemWnd) =>
            {
                _itemWnd?.hideWnd();
            });
        }

        protected override void _onReset()
        {
            dealAllBusinessSkillItemAfterInitDone((_itemWnd) =>
            {
                _itemWnd?.resetWnd();
            });
        }
        
        /// <summary>
        /// 初始化进度条
        /// </summary>
        private void _initSlider()
        {
            if(wnd == null || wnd.slider == null)
                return;

            _m_lSldMinValue = Math.Min(wnd.minValue, wnd.maxValue);
            _m_lSldMaxValue = Math.Max(wnd.minValue, wnd.maxValue);

            if(wnd.slider.fillRect != null)
                _m_sldFillImage = wnd.slider.fillRect.GetComponent<Image>();
            else
                _m_sldFillImage = null;
            
            _m_fSldLeftPosX = 0;
            RectTransform sldFillRectParent = wnd.slider.fillRect == null ? null : wnd.slider.fillRect.parent as RectTransform;
            if (sldFillRectParent != null)
            {
                _m_fSldWidth = sldFillRectParent.rect.width;// 获取进度条填充区域父节点的宽度
                // 获取进度条填充区域父节点的左边界 在其自身坐标系下的X坐标
                _m_fSldLeftPosX = -sldFillRectParent.pivot.x * _m_fSldWidth;
            }
            
            wnd.slider.enabled = false;//进度条不可用, 由代码直接控制进度条值
            
            _m_lSldValuePositionList.Clear();
            _m_lSldValuePositionList.Add(new KeyValuePair<long, float>(_m_lSldMinValue, _m_fSldLeftPosX)); //添加最小值对应的位置
            _initSKillItemList();
            _m_lSldValuePositionList.Add(new KeyValuePair<long, float>(_m_lSldMaxValue, _m_fSldLeftPosX + _m_fSldWidth));//添加最大值对应的位置
            
            _m_lSldValuePositionList.Sort((_a, _b) =>
            {
                return _a.Key.CompareTo(_b.Key);
            });
            if (_m_lSldValuePositionList.Count > 1)//若有不止一个点位值
            {
                for (int i = 1; i < _m_lSldValuePositionList.Count; i++)
                {
                    KeyValuePair<long, float> a = _m_lSldValuePositionList[i - 1];
                    KeyValuePair<long, float> b = _m_lSldValuePositionList[i];

                    if (a.Value > b.Value)
                    {
                        Debug.LogError($"所需亲密度:{a.Key} 对应的位置值:{a.Value} 大于 所需亲密度:{b.Key} 对应的位置值:{b.Value}", wnd);
                    }
                }
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

            if (_m_sldFillImage != null)
            {
                float anchorMaxPosX = _m_fSldLeftPosX;

                float sldValuePosition = _getValueXPosition(_nowIntimacy);
                anchorMaxPosX = (sldValuePosition - _m_fSldLeftPosX) / _m_fSldWidth;
                
                // Vector2 fillRectAnchorMax = _m_sldFillRect.anchorMax;
                // fillRectAnchorMax.x = anchorMaxPosX;
                // _m_sldFillRect.anchorMax = fillRectAnchorMax;

                _m_sldFillImage.fillAmount = anchorMaxPosX;
            }
            
            dealAllBusinessSkillItemAfterInitDone((_itemWnd) =>
            {
                if (_itemWnd != null)
                {
                    _itemWnd.showWnd();
                    _itemWnd.setNowIntimacy(_nowIntimacy);       
                }
            });
        }

        /// <summary>
        /// 获取某一个值在进度条上的x坐标
        /// </summary>
        /// <returns></returns>
        private float _getValueXPosition(long _sldValue)
        {
            if (_m_lSldValuePositionList.Count < 2)//一般来说会有两个值, 最小值和最大值对应的位置, 所以若没有两个值则直接返回最左边界位置
                return _m_fSldLeftPosX;
            
            KeyValuePair<long, float> _a = _m_lSldValuePositionList[0];
            KeyValuePair<long, float> _b = _m_lSldValuePositionList[^1];
            for (int i = 0; i < _m_lSldValuePositionList.Count - 1; i++)
            {
                if (_m_lSldValuePositionList[i + 1].Key >= _sldValue)
                {
                    _a = _m_lSldValuePositionList[i];
                    _b = _m_lSldValuePositionList[i + 1];
                    
                    break;
                }
            }

            if (_a.Key >= _sldValue)
                return _a.Value;
            if(_b.Key <= _sldValue)
                return _b.Value;
            
            return (_b.Value - _a.Value) * (_sldValue - _a.Key) / (_b.Key - _a.Key) + _a.Value;
        }
        
        #region SKillItem

        /// <summary>
        /// 初始化技能item列表
        /// </summary>
        private void _initSKillItemList()
        {
            _discardSkillItem();//先销毁技能item列表
            
            if (wnd == null || wnd.skillItemParentList == null)
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
            for (int i = 0; i < wnd.skillItemParentList.Count; i++)
            {
                ConsortBusinessSkillItemParent itemParent = wnd.skillItemParentList[i];
                if(itemParent == null)
                    continue;
                
                ConsortBusinessSkillRefObj businessSkillRefObj = GRefdataCoreMgr.instance.consortBusinessSkillRefCore.getRef(itemParent.skillRefId);
                if (businessSkillRefObj == null || businessSkillRefObj.unlock_need_intimacy < _m_lSldMinValue ||
                    businessSkillRefObj.unlock_need_intimacy > _m_lSldMaxValue)
                {
                    Debug.LogError($"itemParent.skillRefId : {itemParent.skillRefId} 配置错误, 找不到对应的经营技能配表数据" +
                                   $", 或经营技能解锁所需亲密度:{businessSkillRefObj?.unlock_need_intimacy} 不在配置的范围:[{_m_lSldMinValue}, {_m_lSldMaxValue}]内", wnd);
                    continue;
                }
                
                _m_lSldValuePositionList.Add(new KeyValuePair<long, float>(businessSkillRefObj.unlock_need_intimacy, itemParent.position == null ? _m_fSldLeftPosX : itemParent.position.localPosition.x));
                
                stepCounter.chgTotalStepCount(1);
                GGUIPrefabSubWndConsortBusinessSkillItem skillItem = new GGUIPrefabSubWndConsortBusinessSkillItem(businessSkillRefObj, _m_fNeedShowRedTipFunc, itemParent.position);
                skillItem.onItemClick += _onItemClick;
                _m_lBusinessSkillItemWndList.Add(skillItem);

                skillItem.load(() =>
                {
                    skillItem.setParentTransform(_m_skillItemParent);
                    
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
            _m_aOnSkillItemWndInitDone = null;

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

        public GGUIPrefabSubWndConsortBusinessSkillItem findBusinessSkillItem(Predicate<GGUIPrefabSubWndConsortBusinessSkillItem> _match)
        {
            if (_match == null)
                return null;
            
            return _m_lBusinessSkillItemWndList.Find(_match);
        }
        
        public void dealAllBusinessSkillItemAfterInitDone(Action<GGUIPrefabSubWndConsortBusinessSkillItem> _action)
        {
            if(_action == null)
                return;

            foreach (var itemWnd in _m_lBusinessSkillItemWndList)
            {
                if(itemWnd == null)
                    continue;
                
                itemWnd.regLoadDoneDelegate(() =>
                {
                    _action?.Invoke(itemWnd);
                });
            }
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