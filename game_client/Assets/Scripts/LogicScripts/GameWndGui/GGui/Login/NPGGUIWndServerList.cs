using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ALPackage;
using Common.NpPlayerInfoObj;
using NPCommon;
using NPEnum;

namespace GOE
{
    /*******************
     *登录界面的功能按钮界面
     **/
    public class NPGGUIWndServerList : _ANPGGUIBasicWnd<NPGGUIMonoServerList>
    {
        private static NPGGUIWndServerList _g_instance = new NPGGUIWndServerList();

        public static NPGGUIWndServerList instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGGUIWndServerList();
                return _g_instance;
            }
        }

        private NPGGUIWndServerAreaContainer _m_areaContainer; //区域列表
        private NPGGUIWndServerGroupGrid _m_serverGroupGrid; //服务器组列表
        private NPGGUIWndServerGrid _m_serverGrid; //服务器列表
        private NPGGUIWndServerGrid _m_selfServerGrid; //自己服务器列表
        private long _m_lShowSerializeOp;//展示序列号
        private Dictionary<string, Dictionary<string, ServerGroupShowData>> _m_dServerAreaDic;//服务器大区数据<大区AreaTag,服务器列表数据<组名称，组数据>>


        protected NPGGUIWndServerList() : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPGGUIMonoServerList.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoServerList.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
            _m_lShowSerializeOp = ALSerializeOpMgr.next();
            _refreshWnd();
        }

        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
            _m_lShowSerializeOp = ALSerializeOpMgr.next();
            if (null != _m_areaContainer)
                _m_areaContainer.hideWnd();

            if (null != _m_serverGroupGrid)
                _m_serverGroupGrid.hideWnd();

            if (null != _m_serverGrid)
                _m_serverGrid.hideWnd();

            if (null != _m_selfServerGrid)
                _m_selfServerGrid.hideWnd();

            if(null != _m_dServerAreaDic)
                _m_dServerAreaDic.Clear();
        }

        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
            if (null != _m_areaContainer)
                _m_areaContainer.resetWnd();

            if (null != _m_serverGroupGrid)
                _m_serverGroupGrid.resetWnd();

            if (null != _m_serverGrid)
                _m_serverGrid.resetWnd();

            if (null != _m_selfServerGrid)
                _m_selfServerGrid.resetWnd();
        }

        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            if (null != _m_areaContainer)
            {
                _m_areaContainer.discard();
                _m_areaContainer = null;
            }

            if (null != _m_serverGroupGrid)
            {
                _m_serverGroupGrid.discard();
                _m_serverGroupGrid = null;
            }

            if (null != _m_serverGrid)
            {
                _m_serverGrid.discard();
                _m_serverGrid = null;
            }

            if (null != _m_selfServerGrid)
            {
                _m_selfServerGrid.discard();
                _m_selfServerGrid = null;
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _onClickClose);
        }

        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            //绑定进入按钮操作
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _onClickClose);

            _m_dServerAreaDic = new Dictionary<string, Dictionary<string, ServerGroupShowData>>();

            if (null != wnd.areaContainer)
            {
                _m_areaContainer = new NPGGUIWndServerAreaContainer(wnd.areaContainer);
                _m_areaContainer.OnSelectedArea += _onSelectedArea;
            }

            if (null != wnd.serverGroupGrid)
            {
                _m_serverGroupGrid = new NPGGUIWndServerGroupGrid(wnd.serverGroupGrid);
                _m_serverGroupGrid.OnSelectedGroup += _onSelectedGroup;
            }

            if (null != wnd.serverGrid)
                _m_serverGrid = new NPGGUIWndServerGrid(wnd.serverGrid);
            
            if(null != wnd.serverSelfGrid)
                _m_selfServerGrid = new NPGGUIWndServerGrid(wnd.serverSelfGrid);
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
                
            //刷新大区显示
            _refreshArea();
            //刷新服务器组与列表
            _refreshGroupAndServerList(CDNSetting_AreaInfo.instance.areaTag);
        }

        /// <summary>
        /// 刷新大区显示
        /// </summary>
        private void _refreshArea()
        {
            if (_m_areaContainer != null)
            {
                _m_areaContainer.showWnd();
                _m_areaContainer.refreshWnd();
            }
        }

        /// <summary>
        /// 刷新服务器组与列表
        /// </summary>
        private void _refreshGroupAndServerList(string _selectedAreaTag)
        {
            if (wnd == null)
                return;

            //展示加载中GO
            ALUGUICommon.setGameObjEnable(wnd.loadingHideGoList, false);
            ALUGUICommon.setGameObjEnable(wnd.loadingShowGoList, true);

            //展示序列号
            long showSerialize = _m_lShowSerializeOp;
            //获取数据并展示
            _getServerData(_selectedAreaTag, groupShowDataDic =>
            {
                if (showSerialize != _m_lShowSerializeOp || wnd == null || !isShow || groupShowDataDic == null)
                    return;

                //已获取数据，不显示加载中
                ALUGUICommon.setGameObjEnable(wnd.loadingHideGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.loadingShowGoList, false);

                //获取服务器组列表
                List<ServerGroupShowData> serverGroupShowDataList = new List<ServerGroupShowData>();
                foreach (ServerGroupShowData objValue in groupShowDataDic.Values)
                {
                    if (objValue != null)
                        serverGroupShowDataList.Add(objValue);
                }
                serverGroupShowDataList.Sort(_sortGroupShowData);

                //设置组列表展示
                if (_m_serverGroupGrid != null)
                    _m_serverGroupGrid.showWnd(_selectedAreaTag, serverGroupShowDataList);
                //默认选中第一组
                ServerGroupShowData curSelData = serverGroupShowDataList.SafeGet(0);
                if (curSelData != null)
                    _refreshServerList(_selectedAreaTag, curSelData.isSelf, curSelData.getServerList());
            });

        }

        //刷新服务器列表
        private void _refreshServerList(string _selectedAreaTag, bool _isSelf, List<ServerShowData> _serverList)
        {
            if (_serverList == null)
                return;

            if (_isSelf)
            {
                if(_m_serverGrid != null)
                    _m_serverGrid.hideWnd();
                if (_m_selfServerGrid != null)
                    _m_selfServerGrid.showWnd(_selectedAreaTag, _serverList);
            }
            else
            {
                if (_m_selfServerGrid != null)
                    _m_selfServerGrid.hideWnd();
                if (_m_serverGrid != null)
                    _m_serverGrid.showWnd(_selectedAreaTag, _serverList);
            }
        }

        #region 获取数据

        //获取服务器相关数据
        private void _getServerData(string _selectedAreaTag, Action<Dictionary<string, ServerGroupShowData>> _onComplete)
        {
            if (string.IsNullOrEmpty(_selectedAreaTag))
            {
                Debug.LogError("[NPGGUIWndServerList][_setServerData]大区标识为空");
                return;
            }

            //是否已经缓存了对应大区数据，是则直接获取
            if (_m_dServerAreaDic.TryGetValue(_selectedAreaTag, out Dictionary<string, ServerGroupShowData> serverGroupShowDataDic))
            {
                if (_onComplete != null)
                    _onComplete(serverGroupShowDataDic);
                return;
            }

            //展示序列号
            long showSerialize = _m_lShowSerializeOp;
            //服务器列表
            List<ServerShowData> serverList = new List<ServerShowData>();
            //角色列表
            Dictionary<int, NP_SYS_PlayerJoinedUSInfo> playerCharacterList = new Dictionary<int, NP_SYS_PlayerJoinedUSInfo>();

            //获取数据，分两步获取，获取服务器列表和获取角色信息列表
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                if (showSerialize != _m_lShowSerializeOp || wnd == null || !isShow)
                    return;

                //获取服务器及角色信息
                if (_onComplete != null)
                    _onComplete(_getServerGroupShowData(_selectedAreaTag, serverList, playerCharacterList));

            });
            //-------1、获取服务器列表-------
            GameCDNServerListMgr.instance.getServerList(_selectedAreaTag, list =>
            {
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] == null)
                            continue;

                        //不是白名单并且服务器在维护或者测试时，不展示该服务器
                        if(!GameSetting.instance.isWhite && (EServerOnlineState)list[i].onlineState == EServerOnlineState.TEMP_CLOSED)
                            continue;

                        ServerShowData data = new ServerShowData();
                        data.serverDataInfo = list[i];
                        data.areaTag = _selectedAreaTag;
                        serverList.Add(data);
                    }
                }
                stepCounter.addDoneStepCount();
            });
            //-------2、获取角色列表-------
            GameCDNServerListMgr.instance.getPlayerJoinedUSList(_selectedAreaTag, list =>
            {
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] == null)
                            continue;

                        playerCharacterList[list[i].getServerItem().getServerLogicId()] = list[i];
                    }
                }
                stepCounter.addDoneStepCount();
            });
        }

        //获取服务器组列表数据
        private Dictionary<string, ServerGroupShowData> _getServerGroupShowData(string _selectedAreaTag, List<ServerShowData> _serverShowDataList, Dictionary<int, NP_SYS_PlayerJoinedUSInfo> _characterDic)
        {
            if (_m_dServerAreaDic == null)
                _m_dServerAreaDic = new Dictionary<string, Dictionary<string, ServerGroupShowData>>();
            //服务器组数据
            Dictionary<string, ServerGroupShowData>  serverGroupShowDataList = new Dictionary<string, ServerGroupShowData>();
            //自己的服务器列表
            List<ServerShowData> mySeverList = new List<ServerShowData>();

            if (_serverShowDataList != null)
            {
                //合并服务器列表与玩家角色信息
                if (_characterDic != null && _characterDic.Count > 0)
                {
                    for (int i = 0; i < _serverShowDataList.Count; i++)
                    {
                        if (_serverShowDataList[i] == null)
                            continue;

                        if (_characterDic.TryGetValue(_serverShowDataList[i].serverDataInfo.serverId, out NP_SYS_PlayerJoinedUSInfo playerJoinedUsInfo))
                        {
                            _serverShowDataList[i].playerCharacterInfo = playerJoinedUsInfo;
                            mySeverList.Add(_serverShowDataList[i]);
                        }
                    }
                }

                //根据分组保存服务器列表数据到字典中
                for (int i = 0; i < _serverShowDataList.Count; i++)
                {
                    if (_serverShowDataList[i] == null || _serverShowDataList[i].serverDataInfo == null || string.IsNullOrEmpty(_serverShowDataList[i].serverDataInfo.groupTag))
                        continue;

                    if (serverGroupShowDataList.TryGetValue(_serverShowDataList[i].serverDataInfo.groupTag, out ServerGroupShowData serverGroupShowData))
                    {
                        serverGroupShowData.addSeverData(_serverShowDataList[i]);
                    }
                    else
                    {
                        ServerGroupShowData groupShowData = new ServerGroupShowData();
                        groupShowData.isSelf = false;
                        groupShowData.groupName = _serverShowDataList[i].serverDataInfo.groupTag;
                        groupShowData.addSeverData(_serverShowDataList[i]);
                        serverGroupShowDataList[_serverShowDataList[i].serverDataInfo.groupTag] = groupShowData;
                    }
                }

                //添加自己的服务器列表分组
                ServerGroupShowData myGroupShowData = new ServerGroupShowData();
                myGroupShowData.isSelf = true;
                myGroupShowData.groupName = TransKeyConst.login_selfServerGroup;//我的服务器
                myGroupShowData.addSeverDataList(mySeverList);
                serverGroupShowDataList[TransKeyConst.login_selfServerGroup] = myGroupShowData;

                //设置该大区服务器列表相关数据
                _m_dServerAreaDic[_selectedAreaTag] = serverGroupShowDataList;
            }
            return serverGroupShowDataList;
        }

        //服务器组数据排序：自己在最前面，后面根据id从大到小
        private int _sortGroupShowData(ServerGroupShowData _a, ServerGroupShowData _b)
        {
            if (_a == null || _b == null)
                return 0;

            int cmpIsSelf = _a.isSelf.CompareTo(_b.isSelf);
            int cmpId = _a.maxServerId.CompareTo(_b.maxServerId);
            if (cmpIsSelf != 0)
                return -cmpIsSelf;
            else if (cmpId != 0)
                return -cmpId;
            else
                return 0;
        }

        #endregion

        #region 点击事件

        /// <summary>
        /// 选中区域的时候
        /// </summary>
        /// <param name="_selectedAreaId"></param>
        private void _onSelectedArea(string _selectedAreaTag)
        {
            _refreshGroupAndServerList(_selectedAreaTag);
        }

        /// <summary>
        /// 选中服务器组
        /// </summary>
        /// <param name="_onSelectedGroup"></param>
        private void _onSelectedGroup(string _curSelectAreaTag, ServerGroupShowData _onGroupInfo)
        {
            _refreshServerList(_curSelectAreaTag, _onGroupInfo.isSelf, _onGroupInfo.getServerList());
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickClose(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_Login_Server_List);
        }

        #endregion
    }
}