using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 不同找东西小游戏加载prefab
    /// </summary>
    public class GGUIWndFindThingsGameTeleprompterItem : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoFindThingsGameTeleprompterItem>
    {
        private NPCommonAssetPathInfo _m_iAssetPath;
        
        private _IFindThingsGameThingInfo _m_iThingInfo;//物品信息
        
        private long _m_lAnimationSerializeId;//序列化id
        
        public Transform flyTarget { get { return wnd == null ? null : wnd.flyTarget; } }
        public _IFindThingsGameThingInfo thingInfo { get { return _m_iThingInfo; } }

        public GGUIWndFindThingsGameTeleprompterItem(NPCommonAssetPathInfo _assetPath, Transform _parent) : base(_parent)
        {
            _m_iAssetPath = _assetPath;
        }

        protected override string _monoAssetPath { get { return _m_iAssetPath?.asset_path; } }
        protected override string _monoObjName { get { return _m_iAssetPath?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lAnimationSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        public void setThingInfo(_IFindThingsGameThingInfo _thingInfo, bool _showChgAnimation = false, Action _onShowDone = null)
        {
            if (wnd == null)
            {
                _onShowDone?.Invoke();
                return;
            }

            long serializeId = _m_lAnimationSerializeId = ALSerializeOpMgr.next();
            if (!_showChgAnimation)
            {
                _m_iThingInfo = _thingInfo;
                _refreshThingInfo();
                _sampleAnimation(wnd.changeItemShowAnimationName, 1f);//不需要显示切换动画时, 动画直接置为显示动画最后一帧
            }
            else
            {
                // 先显示隐藏动画, 再显示显示动画
                _playAnimation(wnd.changeItemHideAnimationName, serializeId, () =>
                {
                    // 窗口被销毁或序列号不同时, 不再继续
                    if (wnd == null || _m_lAnimationSerializeId != serializeId)
                        return;

                    _m_iThingInfo = _thingInfo;
                    _refreshThingInfo();
                    // 播放显示动画
                    _playAnimation(wnd.changeItemShowAnimationName, serializeId, _onShowDone);
                });
            }
        }
        
        private void _refreshThingInfo()
        {
            if (wnd == null)
            {
                return;
            }

            if (_m_iThingInfo == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasThingInfoShow, false);                
                ALUGUICommon.setGameObjEnable(wnd.noThingInfoShow, true);                
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasThingInfoShow, true);                
                ALUGUICommon.setGameObjEnable(wnd.noThingInfoShow, false);
                
                ALUGUICommon.setLabelTxt(wnd.txtThingsName, _m_iThingInfo.getThingName());
            }
        }

        private void _playAnimation(string _animationName, long _serializeId, Action _onPlayDone)
        {
            if(_serializeId != _m_lAnimationSerializeId)
                return;
            
            if (wnd == null || wnd.teleprompterAnimation == null || string.IsNullOrEmpty(_animationName))
            {
                _onPlayDone?.Invoke();
                return;
            }

            wnd.teleprompterAnimation.Play(_animationName, () =>
            {
                if (_m_lAnimationSerializeId != _serializeId)
                    return;

                _onPlayDone?.Invoke();
            });
        }

        private void _sampleAnimation(string _animationName, float _normalizedTime = 0.0f)
        {
            if (wnd == null || wnd.teleprompterAnimation == null || string.IsNullOrEmpty(_animationName))
            {
                return;
            }
            
            wnd.teleprompterAnimation.Sample(_animationName, _normalizedTime);
        }
    }
}