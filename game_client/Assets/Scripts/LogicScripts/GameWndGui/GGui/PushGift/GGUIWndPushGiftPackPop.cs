using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 推送礼包弹出界面
    /// </summary>
    public class GGUIWndPushGiftPackPop : _ANPGGUIBasicWnd<GGUIMonoPushGiftPackPop>
    {
        private static GGUIWndPushGiftPackPop _g_instance;
        public static GGUIWndPushGiftPackPop instance { get { return _g_instance ??= new GGUIWndPushGiftPackPop(); } }

        private GNodePushGiftPackPop _m_node;
        
        // 当前推送礼包信息
        private PushGiftPackInfo _m_pushGiftPackInfo;
        
        // 当前使用的子窗口
        private GGUIWndPushGiftPackPrefab _m_curPrefabWnd;
        // 子窗口字典，key为ui_res_path_id
        private Dictionary<long, GGUIWndPushGiftPackPrefab> _m_prefabWndDic;

        private long _m_lRefreshWndSerialId;
        
        private GGUIWndPushGiftPackPop() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoPushGiftPackPop.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPushGiftPackPop.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            // 无需在这里构建子窗口，子窗口根据ui_res_path_id动态加载
        }
        
        protected override void _onDiscard()
        {
            // 销毁所有子窗口
            _discardAllPrefabWnd();
            _m_prefabWndDic?.Clear();
            _m_prefabWndDic = null;
            _m_curPrefabWnd = null;
            
            _m_pushGiftPackInfo = null;

            _m_node = null;
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.PUSH_GIFT_PACK_DISABLE, _onPushGiftPackDisable);
            
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lRefreshWndSerialId = ALSerializeOpMgr.next();
            
            WinMsg.UnregisterMsg(WinMsgType.PUSH_GIFT_PACK_DISABLE, _onPushGiftPackDisable);
            
            _dealAllPrefabWnd((_prefabWnd) => { _prefabWnd?.hideWnd(); });
        }

        protected override void _onReset()
        {
            _dealAllPrefabWnd((_prefabWnd) => { _prefabWnd?.resetWnd(); });
        }

        public void setNode(GNodePushGiftPackPop _node)
        {
            _m_node = _node;
        }
        
        /// <summary>
        /// 刷新窗口
        /// </summary>
        /// <param name="_pushGiftPackInfo">推送礼包信息</param>
        public void setData(PushGiftPackInfo _pushGiftPackInfo)
        {
            _m_pushGiftPackInfo = _pushGiftPackInfo;
            refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_pushGiftPackInfo == null)
                return;
            
            PushGiftPackRefObj pushGiftPackRef = _m_pushGiftPackInfo.pushGiftPackRefObj;
            if (pushGiftPackRef == null)
                return;
            
            long uiResPathId = pushGiftPackRef.ui_res_path_id;
            long refreshWndSerialId = _m_lRefreshWndSerialId = ALSerializeOpMgr.next();
            
            // 若当前子窗口不匹配，切换子窗口
            GGUIWndPushGiftPackPrefab targetPrefabWnd = _getPrefabWnd(uiResPathId);
            if (_m_curPrefabWnd != targetPrefabWnd)
            {
                _m_curPrefabWnd?.hideWnd();
                _m_curPrefabWnd = targetPrefabWnd;
            }
            
            if (_m_curPrefabWnd == null)
                return;
            
            // 注册加载完成回调并刷新
            _m_curPrefabWnd.regLoadDoneDelegate(() =>
            {
                // 确保刷新时仍是当前子窗口
                if (_m_curPrefabWnd != targetPrefabWnd || refreshWndSerialId != _m_lRefreshWndSerialId)
                    return;
                
                _m_curPrefabWnd.setData(_m_pushGiftPackInfo);
                _m_curPrefabWnd.showWnd();
            });
        }

        /// <summary>
        /// 处理关闭窗口
        /// </summary>
        private void dealCloseWnd()
        {
            _m_node?.dealCloseNode();
        }
        
        #region 子窗口管理

        /// <summary>
        /// 获取子窗口，若不存在则创建
        /// </summary>
        /// <param name="_uiResPathId">UI资源路径ID</param>
        /// <returns></returns>
        private GGUIWndPushGiftPackPrefab _getPrefabWnd(long _uiResPathId)
        {
            if (_m_prefabWndDic == null)
                _m_prefabWndDic = new Dictionary<long, GGUIWndPushGiftPackPrefab>();
            
            if (!_m_prefabWndDic.TryGetValue(_uiResPathId, out var prefabWnd) || prefabWnd == null)
            {
                prefabWnd = _createPrefabWnd(_uiResPathId);
                if (prefabWnd != null)
                    _m_prefabWndDic[_uiResPathId] = prefabWnd;
            }
            
            return prefabWnd;
        }

        /// <summary>
        /// 创建子窗口
        /// </summary>
        /// <param name="_uiResPathId">UI资源路径ID</param>
        /// <returns></returns>
        private GGUIWndPushGiftPackPrefab _createPrefabWnd(long _uiResPathId)
        {
            if (wnd == null)
                return null;
            
            GGUIWndPushGiftPackPrefab prefabWnd = new GGUIWndPushGiftPackPrefab(_uiResPathId, wnd.pushGiftPackPrefabParent);
            prefabWnd.onCloseBtnClick += _onPrefabWndCloseBtnClick;
            prefabWnd.load();
            
            return prefabWnd;
        }

        /// <summary>
        /// 销毁所有子窗口
        /// </summary>
        private void _discardAllPrefabWnd()
        {
            if (_m_prefabWndDic == null)
                return;
            
            foreach (var prefabWnd in _m_prefabWndDic.Values)
            {
                if (prefabWnd != null)
                {
                    prefabWnd.onCloseBtnClick -= _onPrefabWndCloseBtnClick;
                    prefabWnd.discard();
                }
            }
            _m_prefabWndDic.Clear();
        }

        /// <summary>
        /// 处理所有子窗口
        /// </summary>
        private void _dealAllPrefabWnd(Action<GGUIWndPushGiftPackPrefab> _dealAction)
        {
            if (_m_prefabWndDic == null || _dealAction == null)
                return;
            
            foreach (var prefabWnd in _m_prefabWndDic.Values)
            {
                if(prefabWnd == null)
                    continue;
                
                _dealAction(prefabWnd);
            }
        }

        #endregion

        #region 消息监听

        /// <summary>
        /// 推送礼包失效回调
        /// 参数: _objs[0] = PushGiftPackInfo
        /// </summary>
        private void _onPushGiftPackDisable(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is PushGiftPackInfo _packInfo))
                return;
            
            // 判断是否是当前窗口使用的推送礼包
            if (_m_pushGiftPackInfo == null || _packInfo != _m_pushGiftPackInfo)
                return;
            
            // 礼包失效，关闭窗口
            dealCloseWnd();
        }

        #endregion

        #region 点击事件

        /// <summary>
        /// 子窗口关闭按钮点击回调
        /// </summary>
        private void _onPrefabWndCloseBtnClick()
        {
            dealCloseWnd();
        }

        #endregion
    }
}