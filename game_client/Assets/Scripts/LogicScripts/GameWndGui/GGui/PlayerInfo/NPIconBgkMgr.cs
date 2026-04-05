using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ALPackage;

namespace GOE
{
    // 刷新头像框使用剩余时间
    public class UpdateIconBgkRemainTime : _IALBaseMonoTask
    {
        private NPIconBgk _m_iCurIconBgk = null;

        public void deal()
        {
            //todo
            //// 当前头像框
            //_m_iCurIconBgk = NPPlayer.instance.playerInfo.curIconBgk;

            //if(_m_iCurIconBgk == null)
            //    return;

            //// 刷新头像框使用剩余时间
            //_m_iCurIconBgk.updateRemainTime();

            //// 每5秒刷新一次
            //ALMonoTaskMgr.instance.addMonoTask(this, 5);
        }
    }

    /// <summary>
    /// 玩家头像框管理器
    /// </summary>
    public class NPIconBgkMgr : WCGSingleton<NPIconBgkMgr>
    {
        // 解锁头像事件
        public Action<NPIconBgk> unlockDelegate = null;

        // 所有头像框数据
        private List<NPIconBgk> _m_lIconBgkList = new List<NPIconBgk>();

        public NPIconBgkMgr()
        {
            // 初始化头像框列表
            _resetIconBgkList();

            ALMonoTaskMgr.instance.addMonoTask(new UpdateIconBgkRemainTime(), 5);
        }

        // 重置头像框列表
        private void _resetIconBgkList()
        {
            _m_lIconBgkList.Clear();

            // 全部头像框
            List<PlayerIconBgkRefObj> allIconBgk = GRefdataCoreMgr.instance.iconBgkCore.refList;

            if(allIconBgk == null)
                return;

            // 头像框
            NPIconBgk temp = null;
            for(int i = 0; i < allIconBgk.Count; i++)
            {
                temp = new NPIconBgk(allIconBgk[i].id);
                _insertIconBgk(temp, _m_lIconBgkList);
            }
        }

        #region 服务端回包

        // 获取已解锁头像回包
        public void onRetUnlockedIconBgks(List<Common.Common_IconBgk> _unlockedList)
        {
            if(_unlockedList == null)
                return;

            // 头像框数据
            Common.Common_IconBgk data = null;
            // 头像框
            NPIconBgk temp = null;

            for(int i = 0; i < _unlockedList.Count; i++)
            {
                if(_unlockedList[i] == null)
                    continue;

                data = _unlockedList[i];
                temp = _m_lIconBgkList.Find(x => x.id == data.getIconBgk());

                if(temp != null)
                {
                    temp.isUnlock = true;
                }

                // 头像期限
                if(data.getLeftTimeS() > 0)
                {
                    // 延长1秒头像使用时间
                    temp.lockedTimeSinceStart = Time.realtimeSinceStartup + data.getLeftTimeS() + 1;
                }
            }
        }

        // 头像框排序(sortId越小越靠前)
        private void _insertIconBgk(NPIconBgk _iconBgk, List<NPIconBgk> _iconBgkList)
        {
            if(_iconBgk.refObj == null)
                return;

            // 头像框插入索引
            int insertIdx = _iconBgkList.Count;

            for(int i = 0; i < _iconBgkList.Count; i++)
            {
                if(_iconBgk.refObj.sort_id < _iconBgkList[i].refObj.sort_id)
                {
                    insertIdx = i;
                    break;
                }
            }

            _iconBgkList.Insert(insertIdx, _iconBgk);
        }

        // 获得头像框
        public void onGainIconBgk(Common.Common_IconBgk _iconBgk)
        {
            if(_iconBgk == null)
                return;

            NPIconBgk temp = null;

            for(int i = 0; i < _m_lIconBgkList.Count; i++)
            {
                if(_m_lIconBgkList[i].id == _iconBgk.getIconBgk())
                {
                    temp = _m_lIconBgkList[i];
                    temp.isUnlock = true;

                    if(_iconBgk.getLeftTimeS() > 0)
                    {
                        // 延长一秒头像使用时间
                        temp.lockedTimeSinceStart = _iconBgk.getLeftTimeS() + Time.realtimeSinceStartup + 1;
                    }

                    break;
                }
            }

            if(temp == null)
                return;

            if(unlockDelegate != null)
                unlockDelegate(temp);
        }
        #endregion

        #region 向服务端请求

        #endregion

        // 根据id获取头像框
        public NPIconBgk getIconBgk(long _id, bool _getDefaultWhenUnfind = false)
        {
            for(int i = 0; i < _m_lIconBgkList.Count; i++)
            {
                if(_m_lIconBgkList[i].id == _id)
                    return _m_lIconBgkList[i];
            }

            return  null;
        }

        // 获取所有头像框
        public List<NPIconBgk> getAllIconBgk()
        {
            List<NPIconBgk> list = new List<NPIconBgk>();

            for(int i = 0; i < _m_lIconBgkList.Count; i++)
            {
                if(_m_lIconBgkList[i].canSee)
                    list.Add(_m_lIconBgkList[i]);
            }

            return list;
        }

        // 抹掉所有数据
        public void clearAll()
        {
            _resetIconBgkList();
            unlockDelegate = null;
        }
    }

    /// <summary>
    /// 玩家头像框数据结构
    /// </summary>
    public class NPIconBgk
    {
        // 头像框id
        private long _m_lId;
        public long id { get { return _m_lId; } }

        // 头像框名字
        private string _m_sName;
        public string name { get { return _m_sName; } }

        // 头像框描述
        private string _m_sDes;
        public string des { get { return _m_sDes; } }

        // 是否解锁(免费也视为已解锁)
        private bool _m_bIsUnlock = false;
        public bool isUnlock
        {
            get
            {
                if(_m_rRefObj == null)
                    return false;

                return _m_bIsUnlock;
            }
            set { _m_bIsUnlock = value; }
        }
        public bool canSee
        {
            get
            {
                if(_m_rRefObj == null)
                    return false;

                if(_m_bIsUnlock)
                    return true;

                return !_m_rRefObj.disable_cannot_see;
            }
            set { _m_bIsUnlock = value; }
        }

        // 头像框分表配置
        private PlayerIconBgkRefObj _m_rRefObj = null;
        public PlayerIconBgkRefObj refObj { get { return _m_rRefObj; } }

        // 头像框资源索引
        private NPGTextureIndex _m_tTexIdx;
        public NPGTextureIndex texIdx { get { return _m_tTexIdx; ; } }

        // 头像框被锁定的时间
        public float lockedTimeSinceStart { get; set; }

        public NPIconBgk(long _id)
        {
            // 分表配置
            _m_rRefObj = GRefdataCoreMgr.instance.iconBgkCore.getRef(_id);
            if(_m_rRefObj == null)
            {
                Debug.LogError("错误：icon_bgk表没有配此id:" + _id);
                return;
            }

            _m_lId = _id;
 
        }

        // 刷新头像框是否到期
        public void updateRemainTime()
        {
            if(isUnlock && lockedTimeSinceStart > 0)
            {
                // 若当前使用的头像到期了
                if(lockedTimeSinceStart <= Time.realtimeSinceStartup)
                {
                    // 设为未解锁状态
                    isUnlock = false;

                    // 请求服务端锁定当前头像框,并将头像框切回默认头像框
                    //NPIconBgkMgr.instance.reqLockedUsingIconBgk();
                }
            }
        }
    }


}

