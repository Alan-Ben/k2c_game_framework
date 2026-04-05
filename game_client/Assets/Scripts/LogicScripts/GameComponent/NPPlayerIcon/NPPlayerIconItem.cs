
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NPCommon;
using ALPackage;
using System.Text;


namespace GOE
{
    //头像数据
    public class NPPlayerIconItem
    {
        //配表数据
        private PlayerIconRefObj _m_irPlayerIconRef;
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
        public NPPlayerIconItem(Common.NpPlayerInfoObj.PlayerInfo_Icon _info)
        {
            if (null == _info)
                return;

            _m_expireTimeTagS = _info.getExpireTimeTagS();
            _m_isViewed = _info.getViewed();

            _initWithId(_info.getId());
        }
        public NPPlayerIconItem(GS2GC.p021_PlayerInfo.GS2GC_021_006_PlayerIconAdd _info)
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
        public NPPlayerIconItem(UniformItemObj _baseData)
        {
            if (null == _baseData)
                return;

            //配表数据
            _m_irPlayerIconRef = GRefdataCoreMgr.instance.playerIconCore.getRef(_baseData.sub_id);
            if (null == _m_irPlayerIconRef)
            {
                Debug.LogError("错误：player_icon 表查找不到此id:" + _baseData.sub_id);
                return;
            }
            //赋值基础数据
            _m_baseData = _baseData;
        }

        public long refId { get { return null == _m_irPlayerIconRef ? 0 : _m_irPlayerIconRef.id; } }
        //配表数据
        public PlayerIconRefObj playerIconRef { get { return _m_irPlayerIconRef; } }
        //基础数据
        public UniformItemObj baseData { get { return _m_baseData; } }
        //是否为新物品
        public bool isNew { get { return !_m_isViewed; } }
        //是否过期
        public bool isExpired { get { return _getIsExpired(); } }

        public long expiredTimeS { get { return _m_expireTimeTagS; } }

        public EPlayerInfoIconType showType { get { return null == _m_irPlayerIconRef ? EPlayerInfoIconType.NONE : _m_irPlayerIconRef.show_type; } }
        
        /// <summary>
        /// 更新相关属性配置
        /// </summary>
        /// <param name="_expiredTimeTagS"></param>
        public void setExpiredTimeTagS(long _expiredTimeTagS)
        {
            _m_expireTimeTagS = _expiredTimeTagS;
        }
        public void setIsViewed(bool _isViewed)
        {
            _m_isViewed = _isViewed;
            NPPlayer.instance.iconComp.redTipReadIcon(refId);
        }

        /// <summary>
        /// 判断是否超时
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
            _m_irPlayerIconRef = GRefdataCoreMgr.instance.playerIconCore.getRef(_id);
            if (null == _m_irPlayerIconRef)
            {
                Debug.LogError("错误：player_icon表查找不到此id:" + _id);
                return;
            }
            _m_baseData = UniformItemSqliteAssistant.getUnifromItem(NPEnum.ENPItemType.ICON, _id);

        }
    }
}

