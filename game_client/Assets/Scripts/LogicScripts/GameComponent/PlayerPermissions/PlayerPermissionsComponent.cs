using ALPackage;
using GS2GC.p002_InitOp;
using JetBrains.Annotations;
using System.Collections.Generic;
using GS2GC.p021_PlayerInfo;

namespace GOE
{
    /// <summary>
    /// 玩家权限组件
    /// </summary>
    public class PlayerPermissionsComponent : _ANPBasicPlayerComponent
    {
        //权限id列表
        [NotNull] private HashSet<long> _m_playerPermissionsList = new HashSet<long>();
        //构造函数
        public PlayerPermissionsComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        protected static ENPPlayerCompType[] _g_DependComp = { };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.PLAYER_PERMISSIONS; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqPlayerPermissionsInit();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerPermissionComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _clear();
        }

        public override void onAllCompInited()
        {
        }

        //析构函数
        private void _clear()
        {
            _m_playerPermissionsList.Clear();
        }

        /// <summary>
        /// 检查是否拥有权限
        /// </summary>
        /// <returns></returns>
        public bool checkHavePermissions(long _id)
        {
            return _m_playerPermissionsList.Contains(_id);
        }

        /// <summary>
        /// 获取所有权限列表
        /// </summary>
        /// <returns></returns>
        public List<long> getAllPermissions()
        {
            return new List<long>(_m_playerPermissionsList);
        }

        /// <summary>
        /// 设置权限列表
        /// </summary>
        /// <param name="_idList"></param>
        private void _setPermissions(List<long> _idList)
        {
            if (_idList == null)
                return;

            _m_playerPermissionsList.Clear();
            for (int i = 0; i < _idList.Count; i++)
            {
                _m_playerPermissionsList.Add(_idList[i]);
            }
        }

        #region S2C

        /// <summary>
        /// 玩家权限初始化
        /// </summary>
        /// <param name="_msg"></param>
        public void retPlayerPermissionsInit(GS2GC_002_059_RetPlayerPermissionsInit _msg)
        {
            if (_msg == null)
                return;

            _setPermissions(_msg.getEffectIdList());

            setInitDone();
        }

        /// <summary>
        /// 玩家权限变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onPlayerPermissionsChg(GS2GC_021_062_OnPlayerPermissionsChg _msg)
        {
            if (_msg == null)
                return;

            _setPermissions(_msg.getEffectIdList());

            WinMsg.SendMsg(WinMsgType.ON_PLAYER_PERMISSION_CHG);
            GCommon.reloadCustomLoadPrefab();
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求玩家权限初始化
        /// </summary>
        public void reqPlayerPermissionsInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_059_ReqPlayerPermissionsInit());
        }

        #endregion
    }
}
