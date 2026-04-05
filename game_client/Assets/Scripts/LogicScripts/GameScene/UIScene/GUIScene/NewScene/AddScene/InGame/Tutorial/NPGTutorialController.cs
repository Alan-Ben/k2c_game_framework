using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /************************
    * 游戏教程AdditionScene
    **/
    public class NPGTutorialController
    {
        private static NPGTutorialController _g_instance = new NPGTutorialController();
        public static NPGTutorialController instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGTutorialController();

                return _g_instance;
            }
        }

        //引导加载数据
        private NPCommonAssetPathInfo _m_tTutorialResInfo;
        //引导窗口对象
        private NPGGUIWndTutorial _m_wnd;

        public NPGTutorialController()
            : base()
        {
        }

        public NPGGUIWndTutorial wnd { get { return _m_wnd; } }

        /// <summary>
        /// 退出当前引导对象
        /// </summary>
        public void quitCurTutorial()
        {
            if (_m_wnd != null)
                _m_wnd.discard();
            _m_wnd = null;

            _m_tTutorialResInfo = null;
        }
        public void quitCurTutorial(NPGGUIWndTutorial _wnd)
        {
            //判断窗口是否一致
            if (_wnd != _m_wnd)
                return;

            if (_m_wnd != null)
                _m_wnd.discard();
            _m_wnd = null;

            _m_tTutorialResInfo = null;
        }

        //显示教程窗口
        public void showTutorial(NPCommonAssetPathInfo _info, Action<bool> _showResult)
        {
            if(_info == null)
                return;
            
            //发送埋点-展示引导窗口
            GCommon.sendStepReport(TraceConst.START_TUTORIAL_WND.setMarkParam(_info.ToString()));
            
            //开启引导状态，提前开启，避免加载延迟
            Game.instance.openIsInTutorial();

            //展示窗口
            _showTutorial(_info, (_showSucc) =>
            {
                // 若展示失败，关闭引导状态
                if(!_showSucc)
                    Game.instance.closeIsInTutorial();
                
                _showResult?.Invoke(_showSucc);
            });
        }

        /// <summary>
        /// 加载并显示引导
        /// </summary>
        /// <param name="_info"></param>
        private void _showTutorial(NPCommonAssetPathInfo _info, Action<bool> _showResult)
        {
            //引导窗口不一样，需要释放掉重新加载
            if(_m_tTutorialResInfo != _info)
            {
                if (_m_wnd != null)
                    _m_wnd.discard();
                _m_wnd = null;
            }

            _m_tTutorialResInfo = _info;

            //加载
            _m_wnd = new NPGGUIWndTutorial(_m_tTutorialResInfo.asset_path, _m_tTutorialResInfo.obj_name);
            _m_wnd.load(()=>
            {
                if (_m_wnd.wnd == null)
                {
                    _showResult?.Invoke(false);
                }
                else
                {
                    _m_wnd.showWnd();
                    _showResult?.Invoke(true);
                }
            });
        }
    }
}
