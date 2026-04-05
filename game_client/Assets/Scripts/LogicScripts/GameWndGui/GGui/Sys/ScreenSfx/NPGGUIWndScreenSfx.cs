using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 屏幕点击特效窗口
    /// </summary>
    public class NPGGUIWndScreenSfx : _ANPGGUIBasicWnd<NPGGUIMonoScreenSfx>
    {
        private static NPGGUIWndScreenSfx _g_instance = new NPGGUIWndScreenSfx();
        public static NPGGUIWndScreenSfx instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUIWndScreenSfx();
                return _g_instance;
            }
        }

        //记录当前是否开启
        private bool _m_bIsCurOpen = false;

        protected NPGGUIWndScreenSfx() : base(EALUIWndLayer.TOP) { }

        protected override string _monoAssetPath { get { return NPGGUIMonoScreenSfx.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoScreenSfx.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_SCREEN_SFX_SETTING_CHG, _onSettingChg);//屏幕点击特效开关变化
            if (GameSetting.instance.usingScreenClickEffect)
                _startTask();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_SCREEN_SFX_SETTING_CHG, _onSettingChg);//屏幕点击特效开关变化
            _stopTask();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }

        //开启每帧任务
        private void _startTask()
        {
            //避免重复开启
            if (_m_bIsCurOpen)
                return;

            //注册监听
            WinMsg.RegisterMsg(WinMsgType.SCREEN_CLICK, _onScreenClick);
        }

        //关闭每帧任务
        private void _stopTask()
        {
            //可以重复关闭
            //注销监听
            WinMsg.UnregisterMsg(WinMsgType.SCREEN_CLICK, _onScreenClick);

            _m_bIsCurOpen = false;
        }

        //展示特效item
        private void _onScreenClick(params object[] _objs)
        {
            if (null == wnd)
                return;

            //获取坐标信息
            if (null == _objs || _objs.Length <= 0)
                return;

            //获取第一个对象
            TouchInfo touchInfo = (TouchInfo)_objs[0];
            if (null == touchInfo || touchInfo.curPos.x < 0 || touchInfo.curPos.y < 0)
                return;

            CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(wnd.sfxId, wnd.sfxParent);
            if (sfxObj != null)
            {
                //屏幕坐标转换到UGUI坐标
                Vector2 uiPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)wnd.transform,
                    touchInfo.curPos,
                    Game.instance.mainCamera.uiCamera,
                    out uiPos);

                //设置特效坐标
                sfxObj.regLoadDoneDelegate(() =>
                {
                    sfxObj.setLocalPos(uiPos);
                });
            }
        }

        //屏幕点击特效开关变化
        private void _onSettingChg()
        {
            if(GameSetting.instance.usingScreenClickEffect)
                _startTask();
            else
                _stopTask();
        }
    }
}
