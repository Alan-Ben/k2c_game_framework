using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 推送礼包列表窗口
    /// </summary>
    public class GGUIWndPushGiftPackList : _ANPGGUIBasicWnd<GGUIMonoPushGiftPackList>
    {
        private static GGUIWndPushGiftPackList _g_instance;
        public static GGUIWndPushGiftPackList instance { get { return _g_instance ??= new GGUIWndPushGiftPackList(); } }

        // 显示的推送礼包类型列表
        private List<EPushGiftPackType> _m_lShowPushGiftPackTypeList;
        
        // 当前有效的推送礼包信息列表
        private List<PushGiftPackInfo> _m_lValidPushGiftPackInfoList;
        // 当前选中的推送礼包索引
        private PushGiftPackInfo _m_iSelectedGiftPackInfo;
        
        // Banner容器窗口
        private GGUIWndPushGiftPackBannerItemContainer _m_bannerContainer;
        
        // 当前使用的子窗口
        private GGUIWndPushGiftPackPrefab _m_curPrefabWnd;
        // 子窗口字典，key为ui_res_path_id
        private Dictionary<long, GGUIWndPushGiftPackPrefab> _m_prefabWndDic;

        private long _m_lShowSerialId;//
        
        private GGUIWndPushGiftPackList() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoPushGiftPackList.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPushGiftPackList.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            _m_lValidPushGiftPackInfoList = new List<PushGiftPackInfo>();
            
            // 构建Banner容器窗口
            if (wnd.monoPushGiftPackBannerContainer != null)
            {
                _m_bannerContainer = new GGUIWndPushGiftPackBannerItemContainer(wnd.monoPushGiftPackBannerContainer);
                _m_bannerContainer.onSelectPushGiftPackInfo += _onBannerSelectPushGiftPackInfo;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            // 销毁所有子窗口
            _discardAllPrefabWnd();
            _m_prefabWndDic?.Clear();
            _m_prefabWndDic = null;
            _m_curPrefabWnd = null;
            
            // 销毁Banner容器
            if (_m_bannerContainer != null)
            {
                _m_bannerContainer.onSelectPushGiftPackInfo -= _onBannerSelectPushGiftPackInfo;
                _m_bannerContainer.discard();
                _m_bannerContainer = null;
            }
            
            _m_lShowPushGiftPackTypeList = null;
            
            _m_lValidPushGiftPackInfoList?.Clear();
            _m_lValidPushGiftPackInfoList = null;
            
            _m_iSelectedGiftPackInfo = null;
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerialId = ALSerializeOpMgr.next();
            
            WinMsg.RegisterMsg(WinMsgType.PUSH_GIFT_PACK_INFO_CHG, _onPushGiftPackInfoChg);
            WinMsg.RegisterMsg(WinMsgType.PUSH_GIFT_PACK_DISABLE, _onPushGiftPackDisable);
            WinMsg.RegisterMsg(WinMsgType.TRIGGER_NEW_PUSH_GIFT_PACK, _onTriggerNewPushGiftPack);
            
            _m_bannerContainer?.showWnd();
            
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialId = ALSerializeOpMgr.next();
            
            WinMsg.UnregisterMsg(WinMsgType.PUSH_GIFT_PACK_INFO_CHG, _onPushGiftPackInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.PUSH_GIFT_PACK_DISABLE, _onPushGiftPackDisable);
            WinMsg.UnregisterMsg(WinMsgType.TRIGGER_NEW_PUSH_GIFT_PACK, _onTriggerNewPushGiftPack);
            
            _m_bannerContainer?.hideWnd();
            _dealAllPrefabWnd((_prefabWnd) => { _prefabWnd?.hideWnd(); });
        }

        protected override void _onReset()
        {
            _m_bannerContainer?.resetWnd();
            _dealAllPrefabWnd((_prefabWnd) => { _prefabWnd?.resetWnd(); });
        }

        /// <summary>
        /// 设置选中的推送礼包索引
        /// </summary>
        /// <param name="_index"></param>
        public void setSelectGiftPackIndex(int _index)
        {
            _m_iSelectedGiftPackInfo = _m_lValidPushGiftPackInfoList.SafeGet(_index);
            refreshWnd();
        }
        
        public void setSelectGiftPackInfo(PushGiftPackInfo _info)
        {
            _m_iSelectedGiftPackInfo = _info;
            refreshWnd();
        }
        
        /// <summary>
        /// 设置显示的推送礼包类型列表
        /// </summary>
        /// <param name="_showPushGiftPackTypeList">显示的推送礼包类型列表</param>
        public void setShowPushGiftPackTypeList(List<EPushGiftPackType> _showPushGiftPackTypeList)
        {
            _m_lShowPushGiftPackTypeList = _showPushGiftPackTypeList;
            
            // 更新有效推送礼包信息列表
            _updateValidPushGiftPackInfoList();

            refreshWnd();
        }

        /// <summary>
        /// 更新有效推送礼包信息列表
        /// </summary>
        private void _updateValidPushGiftPackInfoList()
        {
            if (_m_lValidPushGiftPackInfoList == null)
                _m_lValidPushGiftPackInfoList = new List<PushGiftPackInfo>();
            _m_lValidPushGiftPackInfoList.Clear();
            
            if (_m_lShowPushGiftPackTypeList == null)
                return;
            
            NPPlayer.instance.pushGiftComp.dealAllPushGiftPack((_groupInfo) =>
            {
                if(_groupInfo == null || _groupInfo.pushGiftGroupRefObj == null || 
                   !_m_lShowPushGiftPackTypeList.Contains(_groupInfo.pushGiftGroupRefObj.gift_pack_type))
                    return;
                
                if(_groupInfo.curPushGiftPackInfo == null || !_groupInfo.curPushGiftPackInfo.isValid)
                    return;
                
                // 添加到有效列表
                _m_lValidPushGiftPackInfoList.Add(_groupInfo.curPushGiftPackInfo);
                // 设置为已读
                NPPlayer.instance.pushGiftComp.reqSetPushGiftPackRead(_groupInfo.curPushGiftPackInfo);
            });

            _sortPushGiftPackInfo();
        }

        /// <summary>
        /// 对推送礼包信息进行排序
        /// </summary>
        private void _sortPushGiftPackInfo()
        {
            // 按剩余时间从短到长排序
            _m_lValidPushGiftPackInfoList?.Sort((_a, _b) =>
            {
                if (_b == null) return -1;
                if (_a == null) return 1;
                if(ReferenceEquals(_a, _b)) return 0;
                
                return _a.remainTimeMs.CompareTo(_b.remainTimeMs);
            });
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            if(_m_lValidPushGiftPackInfoList == null || _m_lValidPushGiftPackInfoList.Count <= 0)
            {
                // 若没有有效礼包，关闭窗口
                _dealCloseWnd();
                return;
            }
            
            // 刷新Banner容器
            _refreshBannerContainer();
         
            // 在刷新BannerContainer时会触发选中回调，因此这里不再刷新选中礼包子窗口
            // _refreshSelectedGiftPackPrefabWnd();
        }

        /// <summary>
        /// 刷新Banner容器
        /// </summary>
        private void _refreshBannerContainer()
        {
            if (_m_bannerContainer == null)
                return;
            
            _m_bannerContainer.setInfoList(_m_lValidPushGiftPackInfoList, _m_lValidPushGiftPackInfoList?.IndexOf(_m_iSelectedGiftPackInfo) ?? 0);
        }

        /// <summary>
        /// 刷新选中礼包子窗口显示
        /// </summary>
        private void _refreshSelectedGiftPackPrefabWnd()
        {
            if (_m_iSelectedGiftPackInfo == null)
            {
                _m_curPrefabWnd?.hideWnd();
                return;
            }
            
            PushGiftPackRefObj pushGiftPackRef = _m_iSelectedGiftPackInfo.pushGiftPackRefObj;
            if (pushGiftPackRef == null)
            {
                _m_curPrefabWnd?.hideWnd();
                return;
            }
            
            // 若当前子窗口不匹配，切换子窗口
            GGUIWndPushGiftPackPrefab targetPrefabWnd = _getPrefabWnd(pushGiftPackRef.ui_res_path_id);
            if (_m_curPrefabWnd != targetPrefabWnd)
            {
                _m_curPrefabWnd?.hideWnd();
                _m_curPrefabWnd = targetPrefabWnd;
            }
            
            if (_m_curPrefabWnd == null)
                return;

            long serialId = _m_lShowSerialId;
            // 注册加载完成回调并刷新
            _m_curPrefabWnd.regLoadDoneDelegate(() =>
            {
                // 确保刷新时仍是当前子窗口
                if (_m_curPrefabWnd != targetPrefabWnd || serialId != _m_lShowSerialId)
                    return;
                
                _m_curPrefabWnd.setData(_m_iSelectedGiftPackInfo);
                _m_curPrefabWnd.showWnd();
            });
        }

        private void _dealCloseWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_PUSH_GIFT_PACK_LIST);
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
            prefabWnd.onCloseBtnClick += _dealCloseWnd;
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
                    prefabWnd.onCloseBtnClick -= _dealCloseWnd;
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
        /// 推送礼包信息变化回调
        /// 参数: _objs[0] = PushGiftPackInfo
        /// </summary>
        private void _onPushGiftPackInfoChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is PushGiftPackInfo _packInfo))
                return;
            
            // 信息变化时, 先不处理
            
            // // 检查是否在有效列表中
            // if (_m_lValidPushGiftPackInfoList == null || !_m_lValidPushGiftPackInfoList.Contains(_packInfo))
            //     return;
            //
            // // 刷新窗口
            // refreshWnd();
        }

        /// <summary>
        /// 推送礼包失效回调
        /// 参数: _objs[0] = PushGiftPackInfo
        /// </summary>
        private void _onPushGiftPackDisable(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is PushGiftPackInfo _packInfo))
                return;
            
            bool removeRes = _m_lValidPushGiftPackInfoList?.Remove(_packInfo) ?? false;
            if(!removeRes)// 若不在有效列表中，则不处理
                return;

            if (_packInfo.leftCanBuyCount <= 0) // 若没有可购买次数，
            {
                // long serialId = _m_lShowSerialId;
                // _packInfo.tryAfterBuyAutoTriggerNextPushGiftPack(() =>
                // {
                //     if(serialId != _m_lShowSerialId)
                //         return;
                //     
                //     // 判断_packInfo所处的礼包组内，当前推送礼包信息是否有效, 若有效说明触发了新的推送礼包，选中这个新的礼包
                //     if(_packInfo.pushGiftGroupInfo != null && _packInfo.pushGiftGroupInfo.curPushGiftPackInfo != null && _packInfo.pushGiftGroupInfo.curPushGiftPackInfo.isValid)
                //     {
                //         setSelectGiftPackInfo(_packInfo.pushGiftGroupInfo.curPushGiftPackInfo);
                //     }
                //     else
                //     {
                // 自动触发逻辑修改, 不需要客户端主动触发, 由服务端控制, 所以这里只需要刷新窗口
                // 刷新窗口
                refreshWnd();
                //     }
                // });
            }
            else
            {
                refreshWnd();
            }
        }

        /// <summary>
        /// 触发新推送礼包回调
        /// 参数: _objs[0] = PushGiftPackInfo
        /// </summary>
        private void _onTriggerNewPushGiftPack(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is PushGiftPackInfo _packInfo))
                return;
            
            // 检查礼包有效 且 类型是否在显示列表中
            if (_m_lShowPushGiftPackTypeList == null || _packInfo.pushGiftPackRefObj == null || !_packInfo.isValid)
                return;
            
            PushGiftGroupInfo groupInfo = _packInfo.pushGiftGroupInfo;
            if (groupInfo == null || groupInfo.pushGiftGroupRefObj == null)
                return;
            
            if (!_m_lShowPushGiftPackTypeList.Contains(groupInfo.pushGiftGroupRefObj.gift_pack_type))
                return;

            if (_m_lValidPushGiftPackInfoList == null)
                _m_lValidPushGiftPackInfoList = new List<PushGiftPackInfo>();
            _m_lValidPushGiftPackInfoList.Add(_packInfo);
            // 设置礼包为已读
            NPPlayer.instance.pushGiftComp.reqSetPushGiftPackRead(_packInfo);
            
            
            _sortPushGiftPackInfo();// 排序

            if (_packInfo.isAutoTriggered)
            {
                // 若是自动触发的，选中该礼包
                setSelectGiftPackInfo(_packInfo);
            }
            else
            {
                refreshWnd();
            }
        }

        #endregion

        #region 事件回调

        /// <summary>
        /// Banner选中推送礼包信息回调
        /// </summary>
        /// <param name="_pushGiftPackInfo">选中的推送礼包信息</param>
        private void _onBannerSelectPushGiftPackInfo(PushGiftPackInfo _pushGiftPackInfo)
        {
            if (_pushGiftPackInfo == null || _m_lValidPushGiftPackInfoList == null)
                return;
            
            int selectedIndex = _m_lValidPushGiftPackInfoList.IndexOf(_pushGiftPackInfo);
            if(selectedIndex >= 0 && selectedIndex < _m_lValidPushGiftPackInfoList.Count)
            {
                _m_iSelectedGiftPackInfo = _m_lValidPushGiftPackInfoList.SafeGet(selectedIndex);
                // 刷新选中礼包子窗口
                _refreshSelectedGiftPackPrefabWnd();
            }
            else
            {
                // 刷新Banner容器以纠正选中索引
                _refreshBannerContainer();
            }
        }
        
        private void _onCloseBtnClick(GameObject _go)
        {
            _dealCloseWnd();
        }

        #endregion
    }
}