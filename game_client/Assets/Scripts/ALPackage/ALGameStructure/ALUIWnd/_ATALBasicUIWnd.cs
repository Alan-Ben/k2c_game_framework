using System;

using UnityEngine;

/**********************
 * 基本的窗口加载与处理对象
 **/
namespace ALPackage
{
    /********************
     * 窗口所处的层级对象枚举
     **/
    public enum EALUIWndLayer
    {
        NONE,       //无效层级
        GAME_WORLD_UI_NOOP,    //3d世界中的UI无交互部分
        GAME_WORLD_UI,    //3d世界中的交互UI，经常变动部分使用此UI
        BACKGROUND, //背景窗口
        NORMAL_NOOP,     //普通窗口无操作部分
        NORMAL,     //普通窗口
        ADDITION_NOOP,   //附加窗口
        ADDITION,   //附加窗口
        NOTICE_NOOP,     //提示信息
        NOTICE,     //提示信息
        TOP_NOOP,   //顶层窗口无操作部分
        TOP,        //永远顶层
    }

    public abstract class _ATALBasicUIWnd<T> : _ATALBasicLoadUIWnd<T> where T : _AALBasicUIWndMono
    {
        private static long _g_lSerialize = 1;

        /** 本窗口的序列号 */
        private long _m_lSerialize;

        /** 窗口归属层级 */
        private EALUIWndLayer _m_eWndLayer = EALUIWndLayer.NORMAL;
        
        protected _ATALBasicUIWnd(EALUIWndLayer _layer)
        {
            _m_lSerialize = _g_lSerialize++;
            _m_eWndLayer = _layer;
        }

        public long serialize { get { return _m_lSerialize; } }
        public EALUIWndLayer wndLayer { get { return _m_eWndLayer; } }

        /// <summary>
        /// 获取本对象加载后所挂载的父节点
        /// </summary>
        protected override Transform _getParentTransForm()
        {
            return _AALMonoMain.instance.getUILayerRootTrans(_m_eWndLayer);
        }
    }
}
