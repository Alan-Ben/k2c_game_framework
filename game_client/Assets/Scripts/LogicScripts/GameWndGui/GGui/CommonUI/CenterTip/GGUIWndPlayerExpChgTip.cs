using System;
using ALPackage;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家经验变化上浮提示
    /// </summary>
    public class GGUIWndPlayerExpChgTip : _ANPGGUIBasicWnd<GGUIMonoPlayerExpChgTip>
    {
        private static GGUIWndPlayerExpChgTip _g_instance = new GGUIWndPlayerExpChgTip();
        public static GGUIWndPlayerExpChgTip instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndPlayerExpChgTip();
                return _g_instance;
            }
        }

        //操作序列号
        private long _m_lOpSerialize;
        //经验进度
        private NPGGUIWndProgress _m_expProgressWnd;
        //等级图标
        private NPGGuiWndTexture _m_wLevelIcon;

        protected GGUIWndPlayerExpChgTip() : base(EALUIWndLayer.NOTICE)
        {

        }


        protected override string _monoAssetPath { get { return GGUIMonoPlayerExpChgTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerExpChgTip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_lOpSerialize = ALSerializeOpMgr.next();
            if (null != _m_expProgressWnd)
                _m_expProgressWnd.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lOpSerialize = ALSerializeOpMgr.next();
            if (null != _m_expProgressWnd)
                _m_expProgressWnd.hideWnd();

            if (null != _m_wLevelIcon)
                _m_wLevelIcon.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_expProgressWnd)
                _m_expProgressWnd.resetWnd();

            if (null != _m_wLevelIcon)
                _m_wLevelIcon.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (null != _m_expProgressWnd)
                _m_expProgressWnd.discard();
            _m_expProgressWnd = null;

            if (null != _m_wLevelIcon)
                _m_wLevelIcon.discard();
            _m_wLevelIcon = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (null != wnd.expProgress)
                _m_expProgressWnd = new NPGGUIWndProgress(wnd.expProgress);

            if (null != wnd.imgIcon)
                _m_wLevelIcon = new NPGGuiWndTexture(wnd.imgIcon);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_addValue"></param>
        public void setInfo(long _addValue)
        {
            if (wnd == null)
                return;

            _m_lOpSerialize = ALSerializeOpMgr.next();
            long serialize = _m_lOpSerialize;
            long addValue = _addValue;
            long curExpValue = GCommon.getItemCount(ENPItemType.CURRENCY, (int) ECurrency.P_EXP);

            //设置图标
            PlayerLvlRefObj curLevelRef = GRefdataCoreMgr.instance.getPlayerLevelRefByExp(curExpValue);
            if (curLevelRef != null && _m_wLevelIcon != null)
            {
                _m_wLevelIcon.showWnd();
                _m_wLevelIcon.setTexture(curLevelRef.icon);
            }

            //展示变化
            _showValueLerp(curExpValue - addValue, curExpValue);
        
            //定时隐藏
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize != _m_lOpSerialize)
                    return;
                
                hideWnd();
            }, wnd.disableTime);
        }
        
        //一段时间内，值从开始到结束的变化过程
        private void _showValueLerp(long _start, long _end)
        {
            if (null == wnd || _start == _end)
                return;
        
            NPMonoTaskLerpStartEndValueByTime.startLerpTask(() => { return wnd == null || !isShow; }
                , _m_lOpSerialize
                , _start
                , _end
                , Math.Abs(wnd.value_lerp_time) < 0.01 ? 1.0f : wnd.value_lerp_time
                , _dealShowProgress
                , () => { return _m_lOpSerialize; }
                , null);
        }

        //处理进度条展示
        private void _dealShowProgress(long _curValue)
        {
            if (null != _m_expProgressWnd)
            {
                PlayerLvlRefObj curRefObj = GRefdataCoreMgr.instance.getPlayerLevelRefByExp(_curValue);
                if (curRefObj == null)
                    return;

                PlayerLvlRefObj nextRefObj = GRefdataCoreMgr.instance.playerLvlCore.getRef(curRefObj.lvl + 1);
                if (nextRefObj == null)
                    nextRefObj = curRefObj;

                _m_expProgressWnd.setProgress(_curValue - curRefObj.exp, nextRefObj.exp - curRefObj.exp, EValueFormatType.NORMAL_NOT_LARGE_STR);

            }
        }
    }
}
