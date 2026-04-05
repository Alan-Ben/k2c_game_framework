
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NPCommon;
using ALPackage;
using System.Text;


namespace GOE
{
    public class PlayerIconBgkItem
    {
        //配表数据
        private PlayerIconBgkRefObj _m_iconBgkRef;
        //基础数据
        private UniformItemObj _m_baseData;
        //超时时间
        private long _m_expireTimeTagS;
        //是否查看
        private bool _m_isViewed;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_info"></param>
        public PlayerIconBgkItem(Common.NpPlayerInfoObj.PlayerInfo_IconBgk _info)
        {
            if (null == _info)
                return;

            _m_expireTimeTagS = _info.getExpireTimeTagS();
            _m_isViewed = _info.getViewed();

            _initWithId(_info.getId());

        }
        public PlayerIconBgkItem(GS2GC.p021_PlayerInfo.GS2GC_021_011_PlayerIconBgkAdd _info)
        {
            if (null == _info)
                return;

            _m_expireTimeTagS = _info.getExpireTimeTagS();
            _m_isViewed = _info.getViewed();

            _initWithId(_info.getId());
            
        }
        /// <summary>
        /// 根据uniform创建数据
        /// </summary>
        /// <param name="_baseData"></param>
        public PlayerIconBgkItem(UniformItemObj _baseData)
        {
            if (null == _baseData)
                return;

            //配表数据
            _m_iconBgkRef = GRefdataCoreMgr.instance.iconBgkCore.getRef(_baseData.sub_id);
            if (null == _m_iconBgkRef)
            {
                Debug.LogError("错误：player_icon_bgk 表查找不到此id:" + _baseData.sub_id);
                return;
            }
            //赋值基础数据
            _m_baseData = _baseData;
        }

        public long refId { get { return null == _m_iconBgkRef ? 0 : _m_iconBgkRef.id; } }
        //配表数据
        public PlayerIconBgkRefObj iconBgkRef { get { return _m_iconBgkRef; } }
        //基础数据
        public UniformItemObj baseData { get { return _m_baseData; } }
        public bool isNew { get { return !_m_isViewed; } }
        public bool isExpired { get { return _getIsExpired(); } }

        public long expiredTimeS { get { return _m_expireTimeTagS; } }

        /// <summary>
        /// 更新相关配置
        /// </summary>
        /// <param name="_expiredTimeTagS"></param>
        public void setExpiredTimeTagS(long _expiredTimeTagS)
        {
            _m_expireTimeTagS = _expiredTimeTagS;
        }
        public void setIsViewed(bool _isViewed)
        {
            _m_isViewed = _isViewed;
            NPPlayer.instance.iconBgkComp.redTipReadIconBgk(refId);

        }
        /// <summary>
        /// 获取是否过期
        /// </summary>
        /// <returns></returns>
        private bool _getIsExpired()
        {
            if (_m_expireTimeTagS == -1 || _m_expireTimeTagS == 0)
                return false;

            if (FpsAndPingMgr.instance.serverTimeTagS >= _m_expireTimeTagS)
                return true;

            return false;
        }
        /// <summary>
        /// 获取距离过期的剩余时间
        /// </summary>
        public long getExpiredTimeS()
        {
            if (_getIsExpired())
            {
                return -1;
            }
            return _m_expireTimeTagS - FpsAndPingMgr.instance.serverTimeTagS;
        }
        /// <summary>
        /// 统一初始化
        /// </summary>
        /// <param name="_id"></param>
        private void _initWithId(long _id)
        {
            _m_iconBgkRef = GRefdataCoreMgr.instance.iconBgkCore.getRef(_id);
            if (null == _m_iconBgkRef)
            {
                Debug.LogError("错误：player_icon_bgk表查找不到此id:" + _id);
                return;
            }
            _m_baseData = UniformItemSqliteAssistant.getUnifromItem(NPEnum.ENPItemType.ICON_BGK, _id);

        }
    }
}

