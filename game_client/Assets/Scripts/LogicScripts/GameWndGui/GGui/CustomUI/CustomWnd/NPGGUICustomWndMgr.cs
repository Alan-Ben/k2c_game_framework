using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 自定义界面管理类
    /// 窗口只在战斗进入退出的时候进行清理，除此之外暂不清理
    /// </summary>
    public class NPGGUICustomWndMgr
    {
        private Dictionary<string, NPGGUIWndCustom> _m_dWnd = new Dictionary<string, NPGGUIWndCustom>();

        private static NPGGUICustomWndMgr _g_instance = new NPGGUICustomWndMgr();
        public static NPGGUICustomWndMgr instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUICustomWndMgr();
                return _g_instance;
            }
        }

        /// <summary>
        /// 显示某个自定义界面
        /// </summary>
        /// <param name="_AssetPath"></param>
        /// <param name="_ObjName"></param>
        /// <returns></returns>
        public NPGGUIWndCustom showWnd(string _AssetPath, string _ObjName)
        {
            NPGGUIWndCustom _wWnd = null;
            if(_m_dWnd.ContainsKey(_ObjName))
            {
                _wWnd = _m_dWnd[_ObjName];
                if(_wWnd != null)
                {
                    if(!_wWnd.isLoaded)
                    {
#if UNITY_EDITOR
                        ALLog.Sys($"有CustomWnd[{_wWnd.ObjName}]在管理器外部被释放，注意检查代码");
#endif
                        _wWnd.load();
                    }

                    _wWnd.regLoadDoneDelegate(_wWnd.showWnd);
                }
            }
            else
            {
                _wWnd = new NPGGUIWndCustom(_AssetPath, _ObjName);
                _wWnd.load(_wWnd.showWnd);
                _m_dWnd.Add(_ObjName, _wWnd);
            }

            return _wWnd;
        }
        public NPGGUIWndCustom showWnd(string _AssetPath, string _ObjName, EALUIWndLayer _layer)
        {
            NPGGUIWndCustom _wWnd = null;
            if(_m_dWnd.ContainsKey(_ObjName))
            {
                _wWnd = _m_dWnd[_ObjName];
                if(_wWnd != null)
                {
                    if (!_wWnd.isLoaded)
                    {
#if UNITY_EDITOR
                        ALLog.Sys($"有CustomWnd[{_wWnd.ObjName}]在管理器外部被释放，注意检查代码");
#endif
                        _wWnd.load();
                    }

                    _wWnd.regLoadDoneDelegate(_wWnd.showWnd);
                }
            }
            else
            {
                _wWnd = new NPGGUIWndCustom(_AssetPath, _ObjName, _layer);
                _wWnd.load(_wWnd.showWnd);
                _m_dWnd.Add(_ObjName, _wWnd);
            }

            return _wWnd;
        }
        /// <summary>
        /// 销毁全部界面
        /// </summary>
        public void discardAll()
        {
            foreach(KeyValuePair<string, NPGGUIWndCustom> _v in _m_dWnd)
            {
                _v.Value.discard();
            }
            _m_dWnd.Clear();
        }

        /// <summary>
        /// 关闭界面，根据界面信息判断是否需要释放
        /// </summary>
        public void hideWnd(NPGGUIWndCustom _wnd)
        {
            if(null == _wnd)
                return;

            //判断窗口是否需要释放
            if(null == _wnd.wnd || _wnd.wnd.needDeleteWhenQuitNode)
            {
                //从数据集删除，然后释放窗口
                _m_dWnd.Remove(_wnd.ObjName);
                //强制释放窗口
                _wnd.forceDiscard();
            }
            else
            {
                _wnd.hideWnd();
            }
        }
    }
}
