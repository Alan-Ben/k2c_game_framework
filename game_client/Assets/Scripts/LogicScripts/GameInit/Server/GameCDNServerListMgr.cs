using ALPackage;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Common.NpPlayerInfoObj;

namespace GOE
{
    /// <summary>
    /// 玩家服务器列表数据，从cdn里面获取数据，一般只在打开选服界面的时候再去cdn拉，减少消耗
    /// </summary>
    public class GameCDNServerListMgr
    {
        private static GameCDNServerListMgr _g_instance = new GameCDNServerListMgr();
        [NotNull]
        public static GameCDNServerListMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GameCDNServerListMgr();
                return _g_instance;
            }
        }

        //每个大区下的服务器列表，key代表大区tag
        [NotNull] private Dictionary<string, List<ServerDataInfo>> _m_lServerDictList = new Dictionary<string, List<ServerDataInfo>>();
        //玩家登录过的服务及服务器上的角色信息
        [NotNull] private Dictionary<string, List<NP_SYS_PlayerJoinedUSInfo>> _m_joinedUSDic = new Dictionary<string, List<NP_SYS_PlayerJoinedUSInfo>>();
        //获取角色列表回调
        private Action<List<NP_SYS_PlayerJoinedUSInfo>> _m_aOnGetCharacterList;
        //是否正在获取角色列表
        private bool _m_bIsGettingCharacter = false;
        private string _m_sGetCharAreaTag;

        /// <summary>
        /// 获取玩家已经创角的服务器列表
        /// </summary>
        public void getPlayerJoinedUSList(string _selectedAreaTag, Action<List<NP_SYS_PlayerJoinedUSInfo>> _resultAction)
        {
            //如果已经有数据，直接返回数据
            if (_m_joinedUSDic.TryGetValue(_selectedAreaTag, out List<NP_SYS_PlayerJoinedUSInfo> joinedUSList))
            {
                if (_resultAction != null)
                    _resultAction(joinedUSList);
                return;
            }

            //如果正在获取数据中，注册回调
            if (_m_bIsGettingCharacter)
            {
                if(_m_sGetCharAreaTag == _selectedAreaTag)
                    _m_aOnGetCharacterList += _resultAction;
                else
                {
                    //TODO 不同大区获取角色列表
                }
                return;
            }

            _m_sGetCharAreaTag = _selectedAreaTag;
            _m_aOnGetCharacterList = _resultAction;
            _m_bIsGettingCharacter = true;

            //请求推荐服信息，这边要等GS连上才行
            GameInit_LoginProcess.instance.regDoneDelegate(() =>
            {
                NPGSClientListener.directSendMsg(NPGSWriter_001_BasicOp.make_011_ReqPlayerJoinedUSList());
            });
        }

        public void retPlayerJoinedUSList(NPGS2GC.p001_BasicOp.NPGS2GC_001_011_RetPlayerJoinedUSList _msg)
        {
            if (null == _msg)
                return;

            _m_bIsGettingCharacter = false;
            _m_joinedUSDic[_m_sGetCharAreaTag] = _msg.getJoinedUSList();
            if (_m_aOnGetCharacterList != null)
                _m_aOnGetCharacterList(_msg.getJoinedUSList());
        }

        /// <summary>
        /// 获取对应大区下内的服务器列表
        /// </summary>
        /// <param name="_areaId"></param>
        /// <param name="_resultAction"></param>
        public void getServerList(int _areaId, Action<List<ServerDataInfo>> _resultAction)
        {
            getServerList(CDNSetting_AreaInfo.instance.getAreaTagById(_areaId.ToString()), _resultAction);
        }

        /// <summary>
        /// 获取对应大区下内的服务器列表
        /// </summary>
        /// <param name="_areaTag"></param>
        /// <param name="_resultAction"></param>
        public void getServerList(string _areaTag, Action<List<ServerDataInfo>> _resultAction)
        {
            if (null == _resultAction || string.IsNullOrEmpty(_areaTag))
                return;

            //如果是当前大区，直接返回数据
            if (CDNSetting_AreaInfo.instance.getAreaIdByTag(_areaTag) == CDNSetting_AreaInfo.instance.areaId)
            {
                CDNSetting_ServerListInfo.instance.requestData(_resultAction);
                return;
            }

            //不是当前大区才需要额外查询一下
            //找得到直接返回
            if (_m_lServerDictList.TryGetValue(_areaTag, out List<ServerDataInfo> _serverList))
            {
                _resultAction(_serverList);
            }
            else
            {
                //找不到去cdn拉一下数据
                string filePath = string.Format("/server_list/{0}/{1}", CDNSetting_ClientConfigInfo.instance.platformId, CDNSetting_AreaInfo.instance.getAreaIdByTag(_areaTag));
                ALCDNCommonDownloadMgr<List<ServerDataInfo>>.instance.reqCommonDownload(null, (_result) =>
                {
                    if (null != _result)
                    {
                        _m_lServerDictList.Add(_areaTag, _result.config);
                        _resultAction(_result.config);
                    }
                    else
                    {
                        _resultAction(null);
                    }

                }, () =>
                {
                    _resultAction(null);
                }, new ALURLDownloader(Game.instance.mainCamera.platInfo.phpUrlForLoginList), filePath, "latest");
            }
        }

        /// <summary>
        /// 获取当前所在服务器信息
        /// </summary>
        /// <param name="_resultAction"></param>
        public void getCurServerInfo(Action<ServerDataInfo> _resultAction)
        {
            getServerList(CDNSetting_AreaInfo.instance.areaTag, list =>
            {
                GameInit_SelectServer.instance.getCurSelectServerItem(serverId =>
                {
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i] != null && list[i].serverId == serverId)
                            {
                                _resultAction?.Invoke(list[i]);
                                return;
                            }
                        }
                    }
                });
            });
        }

        /// <summary>
        /// 清空缓存的角色列表
        /// </summary>
        public void clearCharacterList()
        {
            if(_m_joinedUSDic != null)
                _m_joinedUSDic.Clear();
        }
    }
}