using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;


namespace GOE
{
    /// <summary>
    /// 实例化透明背景窗口的显示管理对象
    /// </summary>
    public class NPUIInstanceTransparentBkController
    {
        private static NPUIInstanceTransparentBkController _g_instance = new NPUIInstanceTransparentBkController();
        public static NPUIInstanceTransparentBkController instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPUIInstanceTransparentBkController();
                return _g_instance;
            }
        }

        //展示的透明背景列表
        private List<NPPGUIWndInstanceTransparentBk> _m_lShowInstanceList;

        public NPUIInstanceTransparentBkController()
        {
            _m_lShowInstanceList = new List<NPPGUIWndInstanceTransparentBk>();
        }

        /// <summary>
        /// 取出一个透明背景实例进行展示
        /// </summary>
        /// <returns></returns>
        public NPPGUIWndInstanceTransparentBk showInstanceTransparentBk(Action<NPPGUIWndInstanceTransparentBk> _setAction, Action _clickDelegate)
        {
            NPPGUIWndInstanceTransparentBk instance = NPUICacheTransparentBk.instance?.popItem();
            if(null == instance)
            {
                ALLog.Error("Pop InstanceTransparentBk when cache is not inited!");
                return null;
            }

            //将现有队列中所有透明背景都隐藏
            _hideAllTransparentBk();

            //处理设置操作
            if(null != _setAction)
                _setAction(instance);

            //设置回调
            instance.setClickOp(_clickDelegate);
            //显示窗口
            instance.regLoadDoneDelegate(instance.showWnd);

            

            //放入队列
            _m_lShowInstanceList.Add(instance);

            //将窗口设置为最上一层子对象
            if(null != instance.wnd)
                GCommon.moveTransformToLastAndRefreshLayer(instance.wnd.transform);
            
            //返回
            return instance;
        }

        /// <summary>
        /// 隐藏实例化出来的透明背景窗口
        /// </summary>
        /// <param name="_bkWnd"></param>
        public void hideTransparentBk(NPPGUIWndInstanceTransparentBk _bkWnd)
        {
            if(null == _bkWnd)
                return;

            //从队列删除，如删除失败则不做后续处理
            if(!_m_lShowInstanceList.Remove(_bkWnd))
                return;
            
            //先删除再隐藏窗口，这样当逻辑有错误的时候可以比较明显的展示出来
            _bkWnd.hideWnd();
            //将窗口放回缓存
            NPUICacheTransparentBk.instance.pushBackCacheItem(_bkWnd);

            //从剩余队列中取出最后一个展示
            if(_m_lShowInstanceList.Count > 0)
            {
                NPPGUIWndInstanceTransparentBk lastWnd = _m_lShowInstanceList[_m_lShowInstanceList.Count - 1];
                
                //显示最后一个窗口
                lastWnd.showWnd();
            }
        }

        /// <summary>
        /// 隐藏所有透明背景
        /// </summary>
        protected void _hideAllTransparentBk()
        {
            for(int i = 0; i < _m_lShowInstanceList.Count; i++)
            {
                //逐个隐藏
                _m_lShowInstanceList[i].hideWnd();
            }
        }
    }
}
