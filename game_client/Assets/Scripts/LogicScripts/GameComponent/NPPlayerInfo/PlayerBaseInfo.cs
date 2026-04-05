using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ALPackage;

using NPEnum;

namespace GOE
{
    public class PlayerBaseInfo
    {
        public static readonly int g_PlayerParamTypeCount = Enum.GetValues(typeof(NPEnum.ENPPlayerParam)).Length;

        //成员变量
        protected long _m_lCid;//玩家的id名称不用uid了，避免和国林那边的uid冲突
        protected string _m_sPlayerName;//玩家名字

        protected long[] _m_arrPlayerParam;//玩家参数

        // 默认头像框
        private PlayerIconBgkItem _m_dDefaultIconBgk;

        // 默认头像
        private NPPlayerIconItem _m_dDefaultIcon;

        // 默认气泡框
        private NPPlayerBubbleItem _m_dDefaultBubble;
        
        private CuteActorInfo _m_dCuteActor;

        //构造函数
        public PlayerBaseInfo()
        {
            _m_arrPlayerParam = new long[g_PlayerParamTypeCount];
            //初始化所有参数的值
            for(int i = 0; i < g_PlayerParamTypeCount; i++)
            {
                _m_arrPlayerParam[i] = 0;
            }
        }
        //属性
        public long CID { get { return _m_lCid; } }
        public string PlayerName { get { return _m_sPlayerName; } }

        /// <summary>
        /// 默认头像
        /// </summary>
        public NPPlayerIconItem defaultIcon
        {
            get
            {
                if (null == _m_dDefaultIcon)
                {
                    UniformItemObj obj = UniformItemSqliteAssistant.getUnifromItem(ENPItemType.ICON, GRefdataCoreMgr.instance.npGeneral.default_player_icon);
                    if (null == obj)
                    {
                        Debug.LogError(" General 表中 默认头像id 不存在");
                        return null;
                    }
                    _m_dDefaultIcon = new NPPlayerIconItem(obj);
                }

                return _m_dDefaultIcon;
            }
        }
        /// <summary>
        /// 默认头像框
        /// </summary>
        public PlayerIconBgkItem defaultIconBgk
        {
            get
            {
                if(null == _m_dDefaultIconBgk)
                {
                    UniformItemObj obj = UniformItemSqliteAssistant.getUnifromItem(ENPItemType.ICON_BGK, GRefdataCoreMgr.instance.npGeneral.default_player_icon_bgk);
                    if (null == obj)
                    {
                        Debug.LogError(" General 表中 默认头像框id 不存在");
                        return null;
                    }
                    _m_dDefaultIconBgk = new PlayerIconBgkItem(obj);
                }
               

                return _m_dDefaultIconBgk;
            }
        }
        /// <summary>
        /// 默认Q版形象
        /// </summary>
        public CuteActorInfo defaultCuteActor
        {
            get
            {
                if(null == _m_dCuteActor)
                {
                    UniformItemObj obj = UniformItemSqliteAssistant.getUnifromItem(ENPItemType.CUTE_ACTOR, GRefdataCoreMgr.instance.npGeneral.player_default_cute_actor_id);
                    if (null == obj || GRefdataCoreMgr.instance.npGeneral.player_default_cute_actor_id <= 0)
                    {
                        obj = null;
                        Debug.LogError($" General 表中 玩家默认Q版形象id:{GRefdataCoreMgr.instance.npGeneral.player_default_cute_actor_id} 不存在, 会从cute_actor表中取第一个数据作为默认值");
                        foreach (CuteActorRefObj cuteActorRefObj in GRefdataCoreMgr.instance.cuteActorRefCore.makeNewAllRefList())
                        {
                            if (cuteActorRefObj != null && cuteActorRefObj.id > 0)
                            {
                                obj = UniformItemSqliteAssistant.getUnifromItem(ENPItemType.CUTE_ACTOR, cuteActorRefObj.id);
                                if(obj != null)
                                    break;
                            }
                        }

                        if (obj == null)
                        {
                            Debug.LogError("cute_actor表中没有数据，无法设置默认Q版形象");
                        }
                    }
                    _m_dCuteActor = new CuteActorInfo(obj);
                }
               

                return _m_dCuteActor;
            }
        }
        /// <summary>
        /// 默认气泡框
        /// </summary>
        public NPPlayerBubbleItem defaultBubble
        {
            get
            {
                if (null == _m_dDefaultBubble)
                {
                    UniformItemObj obj = UniformItemSqliteAssistant.getUnifromItem(ENPItemType.BUBBLE, GRefdataCoreMgr.instance.npGeneral.chat_default_bubble_id);
                    if (null == obj)
                    {
                        Debug.LogError(" General 表中 默认头像框id 不存在");
                        return null;
                    }
                    _m_dDefaultBubble = new NPPlayerBubbleItem(obj);
                }


                return _m_dDefaultBubble;
            }
        }
        
        

        /// <summary>
        /// 当前头像
        /// </summary>
        public NPPlayerIconItem curIcon
        {
            get
            {
                NPPlayerIconItem item = NPPlayer.instance.iconComp.getIcon(this[ENPPlayerParam.ICON]);
                // 头像为空 或者 超时，则返回默认头像
                if (null == item || item.isExpired)
                    return defaultIcon;

                return item;
            }
        }
        /// <summary>
        /// 玩家当前头像框
        /// </summary>
        public PlayerIconBgkItem curIconBgk
        {
            get
            {
                PlayerIconBgkItem bgkItem = NPPlayer.instance.iconBgkComp.getIconBgk(this[ENPPlayerParam.ICON_BGK]);
                // 头像框为空 或者 超时，则返回默认头像
                if (null == bgkItem || bgkItem.isExpired)
                    return defaultIconBgk;

                return bgkItem;
            }
        }
        /// <summary>
        /// 玩家当前气泡框
        /// </summary>
        public NPPlayerBubbleItem curBubble
        {
            get
            {
                NPPlayerBubbleItem bubbleItem = NPPlayer.instance.bubbleComp.getBubble(this[ENPPlayerParam.BUBBLE]);
                // 头像框为空 或者 超时，则返回默认头像
                if (null == bubbleItem || bubbleItem.isExpired)
                    return defaultBubble;

                return bubbleItem;
            }
        }
        /// <summary>
        /// 玩家当前Q版形象
        /// </summary>
        public CuteActorInfo curCuteActor
        {
            get
            {
                CuteActorInfo cuteActorInfo = NPPlayer.instance.cuteActorComp.getCuteActorInfo(this[ENPPlayerParam.CUTE_ACTOR]);
                // 头像框为空 或者 过期，则返回默认头像
                if (null == cuteActorInfo || cuteActorInfo.isExpired)
                    return defaultCuteActor;

                return cuteActorInfo;
            }
        }

        public _IPlayerCuteActorShowInfo curCuteActorShowInfo
        {
            get
            {
                // Q版形象
                NPGGoIndex cuteActorGoIndex = NPPlayer.instance.playerInfo.curCuteActor?.cuteActorRefObj?.go_index;
                if (cuteActorGoIndex == null || !cuteActorGoIndex.isValid())
                    cuteActorGoIndex = NPPlayer.instance.playerInfo.defaultCuteActor?.cuteActorRefObj?.go_index;
                
                return new CommonPlayerCuteActorShowInfo(cuteActorGoIndex, Color.clear);
            }
        }
        
        //返回玩家的CS同步参数，部分参数服务器不做初始化，为0时取默认值
        public long this[NPEnum.ENPPlayerParam _type] 
        { 
            get 
            { 
                if(_m_arrPlayerParam[(int)_type] == 0)
                {
                    switch (_type) 
                    {
                        //头像
                        case ENPPlayerParam.ICON:
                            return GRefdataCoreMgr.instance.npGeneral.default_player_icon;
                        //头像框
                        case ENPPlayerParam.ICON_BGK:
                            return GRefdataCoreMgr.instance.npGeneral.default_player_icon_bgk;
                        //聊天气泡
                        case ENPPlayerParam.BUBBLE:
                            return GRefdataCoreMgr.instance.npGeneral.chat_default_bubble_id;
                        case ENPPlayerParam.CUTE_ACTOR:
                            return GRefdataCoreMgr.instance.npGeneral.player_default_cute_actor_id;
                    }
                }
                return _m_arrPlayerParam[(int)_type]; 
            } 
        }

        /*************
         * 获取玩家对应的值
         **/
        public long getValue(NPEnum.ENPPlayerParam _valueType)
        {
            return this[_valueType];
        }

        public virtual void clear()
        {
            for(int i = 0; i < g_PlayerParamTypeCount; i++)
            {
                _m_arrPlayerParam[i] = 0;
            }
            _m_lCid = 0;
            _m_sPlayerName = "";
        }
    }
}
