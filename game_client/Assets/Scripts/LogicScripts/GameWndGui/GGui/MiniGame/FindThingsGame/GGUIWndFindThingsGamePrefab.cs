using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 不同找东西小游戏加载prefab
    /// </summary>
    public class GGUIWndFindThingsGamePrefab : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoFindThingsGamePrefab>
    {
        private NPCommonAssetPathInfo _m_iAssetPath;
        private Action _m_aOnGameSuccess;//游戏成功回调

        [NotNull] private List<GGUIWndFindThingsGameThing> _m_lFindThingsList = new List<GGUIWndFindThingsGameThing>();//查找的物品列表
        [NotNull] private List<GGUIWndFindThingsGameTeleprompterItem> _m_lTeleprompterItemList = new List<GGUIWndFindThingsGameTeleprompterItem>();//题词列表
        private int _m_iNextFindThingIndex;//下一个要找的物品在_m_lFindThingsList列表中下标
        private int _m_iNowFindThingCount;//当前找到的物品数量

        private NPGGUIWndProgress _m_wProgressWnd;//当前收集进度
        private NPGCommonTipWndCache<NPGGUIWndEmptyTip, NPGGUIMonoCommonTip> _m_TipCache;

        private long _m_lAnimationSerialize;//动画序列号
        
        // 是否所有item都被找到
        public bool allFind { get { return _m_iNowFindThingCount >= _m_lFindThingsList.Count; } }

        public GGUIWndFindThingsGamePrefab(NPCommonAssetPathInfo _assetPath, Transform _parent, Action _onGameSuccess) : base(_parent)
        {
            _m_iAssetPath = _assetPath;
            _m_aOnGameSuccess = _onGameSuccess;
        }

        protected override string _monoAssetPath { get { return _m_iAssetPath?.asset_path; } }
        protected override string _monoObjName { get { return _m_iAssetPath?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if(wnd.monoThingsList == null || wnd.monoThingsList.Count <= 0)
            {
                Debug.LogError($"[GGUIWndFindThingsGamePrefab _onWndInitDone] 未配置monoThingsList, 无法进行游戏", wnd.gameObject);
                return;
            }
            
            if (wnd.monoTeleprompterParentList == null || wnd.monoTeleprompterParentList.Count <= 0)
            {
                Debug.LogError($"[GGUIWndFindThingsGamePrefab _onWndInitDone] 未配置monoTeleprompterParentList, 无法进行游戏", wnd.gameObject);
                return;
            }

            Dictionary<long, int> thingDic = new Dictionary<long, int>();
            for(int i = 0;i < wnd.monoThingsList.Count; i++)
            {
                GGUIMonoFindThingsGameThing thingMono = wnd.monoThingsList[i];
                if(thingMono == null || thingMono.thingInfo == null)
                    continue;

                GGUIWndFindThingsGameThing thingWnd = new GGUIWndFindThingsGameThing(thingMono);
                thingWnd.onThingClick += _onThingPointUp;
                _m_lFindThingsList.Add(thingWnd);

                if (thingDic.TryGetValue(thingMono.thingInfo.getThingId(), out int _index))
                {
                    Debug.LogError($"脚本配置错误, monoThingsList列表中存在同id:{thingMono.thingInfo.getThingId()}物体, 请检查配置monoThingsList列表中元素:{_index}和元素:{i}", wnd.gameObject);
                }
                thingDic[thingMono.thingInfo.getThingId()] = i;
            }
            thingDic.Clear();
            thingDic = null;
            
            foreach (Transform itemParent in wnd.monoTeleprompterParentList)
            {
                GGUIWndFindThingsGameTeleprompterItem teleprompterItem = new GGUIWndFindThingsGameTeleprompterItem(wnd.teleprompterItemAssetPath, itemParent);
                teleprompterItem.load();
                _m_lTeleprompterItemList.Add(teleprompterItem);
            }

            if (wnd.clickErrorTipMono != null)
            {
                _m_TipCache = new NPGCommonTipWndCache<NPGGUIWndEmptyTip, NPGGUIMonoCommonTip>(wnd.tipParent == null ? rectTransform : wnd.tipParent, 1, 5);
                _m_TipCache.init(wnd.clickErrorTipMono);
            }

            if (wnd.monoProgress != null)
                _m_wProgressWnd = new NPGGUIWndProgress(wnd.monoProgress);
            
            ALUGUICommon.combinePointerUp(wnd.clickErrorBtn, _onErrorClick);
        }
        
        protected override void _onDiscard()
        {
            foreach (GGUIWndFindThingsGameThing thingWnd in _m_lFindThingsList)
            {
                if (thingWnd != null)
                {
                    thingWnd.onThingClick -= _onThingPointUp;
                    thingWnd.discard();
                }
            }
            _m_lFindThingsList.Clear();

            foreach (GGUIWndFindThingsGameTeleprompterItem teleprompterItem in _m_lTeleprompterItemList)
            {
                teleprompterItem?.discard();
            }
            _m_lTeleprompterItemList.Clear();
            
            if(_m_wProgressWnd != null)
                _m_wProgressWnd.discard();
            _m_wProgressWnd = null;
            
            if(_m_TipCache != null)
                _m_TipCache.discard();
            _m_TipCache = null;

            if (wnd != null)
            {
                ALUGUICommon.uncombinePointerUp(wnd.clickErrorBtn, _onErrorClick);
            }
        }
        
        protected override void _onShowWnd()
        {
            foreach (GGUIWndFindThingsGameThing thingWnd in _m_lFindThingsList)
            {
                thingWnd?.showWnd();
            }

            _m_iNowFindThingCount = 0;
            _m_iNextFindThingIndex = 0;
            foreach (GGUIWndFindThingsGameTeleprompterItem teleprompterItem in _m_lTeleprompterItemList)
            {
                if(teleprompterItem == null)
                    continue;
                
                teleprompterItem.showWnd();
                teleprompterItem.setThingInfo(_m_lFindThingsList.SafeGet(_m_iNextFindThingIndex++)?.thingInfo);
            }
            
            _m_wProgressWnd?.showWnd();
            _m_wProgressWnd?.setProgress(_m_iNextFindThingIndex, _m_lFindThingsList.Count, EValueFormatType.NORMAL_NOT_LARGE_STR);
        }

        protected override void _onHideWnd()
        {
            foreach (GGUIWndFindThingsGameThing thingWnd in _m_lFindThingsList)
            {
                thingWnd?.hideWnd();
            }

            foreach (GGUIWndFindThingsGameTeleprompterItem teleprompterItem in _m_lTeleprompterItemList)
            {
                teleprompterItem?.hideWnd();
            }
            
            _m_wProgressWnd?.hideWnd();
            _m_TipCache?.pushBackAllCacheItems();

            _m_lAnimationSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            foreach (GGUIWndFindThingsGameThing thingWnd in _m_lFindThingsList)
            {
                thingWnd?.resetWnd();
            }

            foreach (GGUIWndFindThingsGameTeleprompterItem teleprompterItem in _m_lTeleprompterItemList)
            {
                teleprompterItem?.resetWnd();
            }
            
            _m_wProgressWnd?.resetWnd();
            _m_TipCache?.pushBackAllCacheItems();
        }

        /// <summary>
        /// 获取物品对应的TeleprompterItem
        /// </summary>
        /// <returns></returns>
        private GGUIWndFindThingsGameTeleprompterItem _getThingTeleprompterItem(_IFindThingsGameThingInfo _thingInfo)
        {
            if (_m_lTeleprompterItemList.Count <= 0 || _thingInfo == null)
                return null;

            foreach (GGUIWndFindThingsGameTeleprompterItem teleprompterItem in _m_lTeleprompterItemList)
            {
                if (teleprompterItem != null && teleprompterItem.thingInfo != null && teleprompterItem.thingInfo.getThingId() == _thingInfo.getThingId())
                    return teleprompterItem;
            }

            return null;
        }
        
        /// <summary>
        /// 当物品被点击到时
        /// </summary>
        /// <param name="_go"></param>
        private void _onThingPointUp(GGUIWndFindThingsGameThing _thingWnd, Vector2 _position)
        {
            if (_thingWnd == null || _thingWnd.thingInfo == null || _thingWnd.hasFind)
            {
                _showClickErrorTip(_position);
                return;
            }
            
            GGUIWndFindThingsGameTeleprompterItem teleprompterItem = _getThingTeleprompterItem(_thingWnd.thingInfo);
            if (teleprompterItem == null)
            {
                _showClickErrorTip(_position);
                return;
            }

            _onRightThingClick(_thingWnd, teleprompterItem);
        }

        /// <summary>
        /// 错误位置被点击时
        /// </summary>
        /// <param name="_position"></param>
        private void _onErrorClick(Vector2 _position)
        {
            _showClickErrorTip(_position);
        }
        
        /// <summary>
        /// 显示点击错误提示
        /// </summary>
        private void _showClickErrorTip(Vector2 _position)
        {
            if(wnd == null || wnd.tipShowTime <= 0 || _m_TipCache == null)
                return;

            // 因为在hide和discard时有对cache进行销毁和pushback, 所以这里就不对单独的每个item进行存储了
            NPGGUIWndEmptyTip tip = _m_TipCache.popItem();
            if(tip != null)
            {
                tip.setTipPos(_position);
                tip.showAndPlayAnim();
                CommonTaskController.CommonActionAddMonoTask( () =>
                {
                    if (_m_TipCache != null)
                    {
                        _m_TipCache.pushBackCacheItem(tip);
                    }
                    else
                    {
                        tip.discard();
                    }
                    
                    tip = null;
                }, wnd.tipShowTime);
            }
        }

        /// <summary>
        /// 当正确的物品被点击时
        /// </summary>
        private void _onRightThingClick(GGUIWndFindThingsGameThing _thingWnd, GGUIWndFindThingsGameTeleprompterItem _teleprompterItem)
        {
            if (_thingWnd == null || _thingWnd.thingInfo == null || _teleprompterItem == null ||
                _teleprompterItem.thingInfo == null ||
                _thingWnd.thingInfo.getThingId() != _teleprompterItem.thingInfo.getThingId())
            {
                Debug.LogError("[GGUIWndFindThingsGamePrefab _onRightThingClick] _thingWnd 和 _teleprompterItem 对应的物品不匹配");
                return;
            }

            _m_iNowFindThingCount++;//找到物品数量+1
            ALProcess _process = ALProcess.CreateProcess("GGUIWndFindThingsGamePrefab._onRightThingClick");
            _process
                .addDelegateProcess((_complete) =>
                {
                    // 物品item进行找到后表现, 当item飞行完成后, 可以进行下一步表现
                    _thingWnd.setThingFind(_teleprompterItem.flyTarget, _complete, null, ()=> _thingWnd.hideWnd());
                })
                .addDelegateProcess((_complete) =>
                {
                    // _teleprompterItem切换
                    _teleprompterItem.setThingInfo(_m_lFindThingsList.SafeGet(_m_iNextFindThingIndex++)?.thingInfo, true);
                    
                    // 进度条开始变化
                    _m_wProgressWnd?.setProgressChg(_m_iNextFindThingIndex, _m_lFindThingsList.Count, wnd == null ? 0 : wnd.progressChgTime, EValueFormatType.NORMAL);

                    if (allFind)//若所有物品都被找到时
                    {
                        // 进行游戏成功表现
                        _playAnimation(wnd == null ? string.Empty : wnd.gameSuccessAnimationName, () =>
                        {
                            _m_aOnGameSuccess?.Invoke();//调用游戏成功回调
                        });
                    }
                    
                    _complete?.Invoke();
                })
                .deal();
        }

        #region 播放动画

        private void _playAnimation(string _animationName, Action _onPlayDone)
        {
            if (wnd == null)
                return;

            long _serializeId = _m_lAnimationSerialize = ALSerializeOpMgr.next();
            
            if (wnd.gameAnimation == null || string.IsNullOrEmpty(_animationName))
            {
                _onPlayDone?.Invoke();
                return;
            }

            wnd.gameAnimation.Play(_animationName, () =>
            {
                if (_m_lAnimationSerialize != _serializeId)
                    return;

                _onPlayDone?.Invoke();
            });
        }

        private void _sampleAnimation(string _animationName, float _normalizedTime = 0.0f)
        {
            if (wnd == null || wnd.gameAnimation == null || string.IsNullOrEmpty(_animationName))
            {
                return;
            }
            
            wnd.gameAnimation.Sample(_animationName, _normalizedTime);
        }

        #endregion
    }
}