using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 资源bar的管理器
    /// </summary>
    public class NPCommonPlayerResBarWndMgr
    {
        private static NPCommonPlayerResBarWndMgr _g_instance;
        [NotNull]public static NPCommonPlayerResBarWndMgr instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPCommonPlayerResBarWndMgr();
                return _g_instance;
            }
        }

        //bar窗口数据结构
        [NotNull]private Dictionary<long, NPGGUIWndMainPlayer> _m_barWndDict = new Dictionary<long, NPGGUIWndMainPlayer>();
        
        //当前显示的bar窗口
        private NPGGUIWndMainPlayer _m_curShowWnd;
        //当前显示的barID
        private long _m_curShowWndId;
        //当前显示的控制序列号，避免非控制场景关闭窗口
        private int _m_iShowBarSerialize;
        
        /// <summary>
        /// 获取当前展示的bar窗口
        /// </summary>
        /// <returns></returns>
        public NPGGUIWndMainPlayer getCurBarWnd()
        {
            return _m_curShowWnd;
        }

        /// <summary>
        /// 显示bar
        /// </summary>
        /// <param name="_resId"></param>
        public int showBar(long _resId = UIResPathConst.C_DEFAULT_PLAYERRESBAR_RES_ID, Action _doneAction = null)
        {
            if (_resId == 0)
            {
                if (_doneAction != null)
                    _doneAction();
                return 0;
            }
            //刷新序列号和控制权
            int serialize = _m_iShowBarSerialize = ALSerializeOpMgr.next();
            
            //同种bar不处理
            if (_m_curShowWndId == _resId)
            {
                //移动到最上层
                GCommon.moveTransformToLastAndRefreshLayer(_m_curShowWnd.wnd);
                
                if (_doneAction != null)
                    _doneAction();

                return _m_iShowBarSerialize;
            }
            
            //隐藏旧的bar
            _hideCurBar();

            _m_curShowWndId = _resId;
            
            //找得到说明加载过了，直接显示
            if (_m_barWndDict.ContainsKey(_resId))
            {
                _m_curShowWnd = _m_barWndDict[_resId];
                if (_m_curShowWnd != null)
                {
                    //移动到最上层
                    GCommon.moveTransformToLastAndRefreshLayer(_m_curShowWnd.wnd);
                    //先移动再显示，
                    _m_curShowWnd.showWnd();
                }

                if (null != _doneAction)
                    _doneAction();
            }
            else
            {
                //找不到先加载
                _m_curShowWnd = new NPGGUIWndMainPlayer(UIResPathAssistant.getAssetPath(_resId), UIResPathAssistant.getObjName(_resId));
                _m_barWndDict.Add(_resId, _m_curShowWnd);
                _m_curShowWnd.load(() =>
                {
                    if (serialize != _m_iShowBarSerialize)
                        return;
                    
                    //移动到最上层
                    GCommon.moveTransformToLastAndRefreshLayer(_m_curShowWnd.wnd);
                    _m_curShowWnd.showWnd();

                    if (_doneAction != null) 
                        _doneAction();
                });
            }
            return _m_iShowBarSerialize;
        }
        
        /// <summary>
        /// 隐藏bar
        /// </summary>
        public void hideCurBar(int _barShowSerialize)
        {
            //判断是否控制对象，不是则不处理
            if (_m_iShowBarSerialize != _barShowSerialize)
                return;

            _hideCurBar();
        }

        /// <summary>
        /// 强制隐藏当前的bar
        /// </summary>
        private void _hideCurBar()
        {
            _m_curShowWndId = 0;
            
            if(null != _m_curShowWnd)
                _m_curShowWnd.hideWnd();
            _m_curShowWnd = null;
        }
    }
}