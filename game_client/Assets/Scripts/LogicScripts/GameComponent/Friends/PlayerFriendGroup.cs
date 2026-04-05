using System.Collections.Generic;
using Common.FriendObj;

namespace GOE
{
    /// <summary>
    /// 好友分组
    /// </summary>
    public class PlayerFriendGroup
    {
        private List<long> _m_playerList;
        private int _m_groupIndex;
        private long _m_dbId;
        private string _m_name;
        
        public PlayerFriendGroup()
        {
            _m_dbId = 0;
            _m_groupIndex = 0;
            _m_playerList = new List<long>();
            _m_name = TransKeyConst.friends_group_default_name;
        }
        public PlayerFriendGroup(Friend_CustomGroupInfo serverData)
        {
            _m_dbId = serverData.getDbId();
            _m_groupIndex = 0;
            _m_playerList = new List<long>();
            _m_playerList.AddRange(serverData.getCidList());
            _m_name = serverData.getName();
        }

        public long dbId { get => _m_dbId; }

        public int groupIndex { get => _m_groupIndex; }
        public List<long> playerList { get => _m_playerList; }
        public long count { get => _m_playerList.Count; }
        public bool isDefault { get => _m_dbId == 0; }//是否默认分组

        /// <summary>
        /// 设置排序位置
        /// </summary>
        /// <param name="_name"></param>
        public void setIndex(int _name)
        {
            _m_groupIndex = _name;
        }
        
        /// <summary>
        /// 设置排序位置
        /// </summary>
        /// <param name="_index"></param>
        public void setDBId(long _dbId)
        {
            _m_dbId = _dbId;
        }
        
        /// <summary>
        /// 设置排序位置
        /// </summary>
        /// <param name="_name"></param>
        public void setName(string _name)
        {
            _m_name = _name;
        }
        
        /// <summary>
        /// 添加玩家
        /// </summary>
        /// <param name="_cid"></param>
        public void addPlayer(long _cid)
        {
            if(_m_playerList.Contains(_cid))
                return;
            _m_playerList.Add(_cid);
        }
        
        /// <summary>
        /// 添加玩家
        /// </summary>
        /// <param name="_cidList"></param>
        public void addPlayer(List<long> _cidList)
        {
            foreach (long cid in _cidList)
            {
                addPlayer(cid);
            }
        }

        /// <summary>
        /// 移除玩家
        /// </summary>
        /// <param name="_cid"></param>
        public void removePlayer(long _cid)
        {
            if(!_m_playerList.Contains(_cid))
                return;
            _m_playerList.Remove(_cid);
        }

        /// <summary>
        /// 移除玩家
        /// </summary>
        /// <param name="_cidList"></param>
        public void removePlayer(List<long> _cidList)
        {
            foreach (long cid in _cidList)
            {
                removePlayer(cid);
            }
        }

        /// <summary>
        /// 分组名字
        /// </summary>
        /// <returns></returns>
        public string getGroupName()
        {
            return TextTranslate.instance.getLanguage(_m_name);
        }

        /// <summary>
        /// 是否可以移除
        /// </summary>
        /// <returns></returns>
        public bool getCanRemove()
        {
            //默认分组不移除，其它可以移除
            return !isDefault;
        }

        /// <summary>
        /// 是否可以改名
        /// </summary>
        /// <returns></returns>
        public bool getCanChgName()
        {
            //默认分组不改名，其它可以改名
            return !isDefault;
        }
    }
}