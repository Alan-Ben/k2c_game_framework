using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 提示处理基类
    /// </summary>
    /// <typeparam name="_T_WND"></typeparam>
    /// <typeparam name="_T_MONO"></typeparam>
    public abstract class _ATNPTipDealer<_T_WND, _T_MONO> : _INPTipDealerInterface
            where _T_WND : _ATNPGGUIWndTip<_T_MONO>
            where _T_MONO : NPGGUIMonoCommonTip
    {
        private NPCenterTipsRefObj _m_tipRef;//tip配置信息

        private _TNPGGUITipWndCache<_T_WND, _T_MONO> _m_cTipCache;//使用的缓存池对象
        private _T_WND _m_wTipWnd;//控制的窗口对象

        private bool _m_bHasShow;//是否已经显示
        private bool _m_bHasDiscard;//是否已经销毁
        private Action<_INPTipDealerInterface> _m_discardAction;//完成回调
        private ETipDealerTagType _m_tag;//标记

        protected _ATNPTipDealer(NPCenterTipsRefObj _tipRef, ETipDealerTagType _tag)
        {
            _m_tipRef = _tipRef;
            _m_tag = _tag;

            _m_cTipCache = null;
            _m_wTipWnd = null;
            _m_bHasShow = false;
            _m_bHasDiscard = false;
            _m_discardAction = default(Action<_INPTipDealerInterface>);
        }

        protected _T_WND _wnd { get { return _m_wTipWnd; } }
        public ETipDealerTagType tag { get { return _m_tag; } }

        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="_serialze"></param>
        /// <param name="_parent"></param>
        /// <param name="_speedRate"></param>
        /// <param name="_onCdDone"></param>
        public void showTip(long _serialze, Transform _parent, float _speedRate, Action<long> _onCdDone, Action<_INPTipDealerInterface> _doneAction)
        {
            //已经显示或者已经销毁，不做操作
            if (_m_bHasShow || _m_bHasDiscard || _m_tipRef == null)
            {
                _onCdDone?.Invoke(_serialze);
                if (_doneAction != null) 
                    _doneAction(this);
                return;
            }

            //标志已经显示
            _m_bHasShow = true;

            //加载并显示
            NPGGUITipWndCacheMgr.instance.loadCache<_T_WND, _T_MONO>(_m_tipRef, (_cache) =>
            {
                if (_m_bHasDiscard)
                    return;

                _m_discardAction += _doneAction;
                
                //判断缓存是否有效
                if (_cache == null)
                {
                    Debug.LogError($"【{GetType()}.showTip Error】showTip：cacheMgr加载cache失败，tipId:{_m_tipRef.id}");
                    _onCdDone?.Invoke(_serialze);
                    discard();
                    return;
                }

                //设置缓存池对象
                _m_cTipCache = _cache;

                //获取控制窗体
                _m_wTipWnd = _m_cTipCache.popItem();

                //判断窗口是否有效
                if (_m_wTipWnd == null)
                {
                    Debug.LogError($"【{GetType()}.showTip Error】showTip：cache获取tipWnd失败，cache类型<{typeof(_T_WND)},{typeof(_T_MONO)}>，tipId:{_m_tipRef.id}");
                    _onCdDone?.Invoke(_serialze);
                    discard();
                    return;
                }

                //播放并设置播放速率
                _m_wTipWnd.rectTransform.SetParent(_parent, false);
                _m_wTipWnd.setAnimSpeed(_speedRate);
                _m_wTipWnd.showAndPlayAnim();

                //调用弹出事件
                _setTipData(_m_wTipWnd);

                //调用pop处理
                _onPop();

                float space_time = _m_tipRef.space_time_need_rate ? _m_tipRef.space_time / _speedRate : _m_tipRef.space_time;
                ALCommonActionMonoTask.addMonoTask(() => { _onCdDone?.Invoke(_serialze); }, space_time);
                
                float discardTime = _m_tipRef.disable_time_need_rate ? _m_tipRef.disable_time / _speedRate : _m_tipRef.disable_time;
                ALCommonActionMonoTask.addMonoTask(discard, discardTime);
            });
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void discard()
        {
            //还未显示或者已经销毁，不做操作
            if (!_m_bHasShow || _m_bHasDiscard)
                return;

            //标识已经销毁
            _m_bHasDiscard = true;

            //缓存对象有效则放入缓存
            _m_cTipCache?.pushBackCacheItem(_m_wTipWnd);

            if (null != _m_discardAction)
                _m_discardAction(this);
            _m_discardAction = null;

            //调用虚函数
            _onDiscard();
        }

        /// <summary>
        /// 弹出tip时调用
        /// </summary>
        protected virtual void _onPop()
        {

        }

        /// <summary>
        /// 释放函数，子类可重载
        /// </summary>
        protected virtual void _onDiscard()
        {

        }

        /// <summary>
        /// 设置窗口数据的处理
        /// </summary>
        /// <param name="_tipWnd"></param>
        protected abstract void _setTipData(_T_WND _tipWnd);
    }
}
