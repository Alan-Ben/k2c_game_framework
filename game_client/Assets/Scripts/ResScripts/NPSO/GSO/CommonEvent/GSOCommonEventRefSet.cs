using System;
using ALPackage;
using NPEnum;
using SQLite4Unity3d;

namespace GOE
{
    /// <summary>
    /// 通用事件主表
    /// </summary>
    [Serializable]
    public class CommonEventRefObj
    {
        public long id;//唯一id
        public long Id { get { return id; } set { id = value; } }
        

        public long before_event_dialog_id;//事件前置剧情对话id
        public long beforeEventDialogId { get { return before_event_dialog_id; } set { before_event_dialog_id = value; } }
        
        
        public long after_event_dialog_id;//事件后置剧情对话id
        public long afterEventDialogId { get { return after_event_dialog_id; } set { after_event_dialog_id = value; } }

        
        public string reward_show_type;//奖励显示类型
        public string rewardShowType { get { return reward_show_type; } set { reward_show_type = value; } }
        private bool _m_isInitRewardShowType = false;
        private ENpRewardShowType _m_eRewardShowType;
        [Ignore]
        public ENpRewardShowType eRewardShowType
        {
            get
            {
                if (!_m_isInitRewardShowType)
                {
                    ALCommon.TryEnumParse(typeof(ENpRewardShowType), reward_show_type, out _m_eRewardShowType);
                    _m_isInitRewardShowType = true;
                }

                return _m_eRewardShowType;
            }
        }

        
        [NonSerialized]
        private _ICommonEventInstanceSubRefObj _m_iEventInstanceSubRefObj;//通用事件-事件子表配表数据
        [NonSerialized]
        private bool _m_bEventInstanceSubRefObjHasInit;
        [Ignore]
        public _ICommonEventInstanceSubRefObj eventInstanceSubRefObj
        {
            get
            {
                if (_m_iEventInstanceSubRefObj == null && !_m_bEventInstanceSubRefObjHasInit)
                {
                    _initEventInstanceSubRefObj();
                }

                if (_m_iEventInstanceSubRefObj == null)
                    Debug.LogError($"[initCommonEventInstanceRefObj] 通用事件配置错误 事件:{id}找不到对应子表数据");    

                return _m_iEventInstanceSubRefObj;
            }
        }

        private void _initEventInstanceSubRefObj()
        {
#if NP_GAME
            _m_bEventInstanceSubRefObjHasInit = true;
            
            _m_iEventInstanceSubRefObj = GRefdataCoreMgr.instance.commonEventAwardRefCore.getRef(id);//从奖励事件实例子表中查找数据
            if (_m_iEventInstanceSubRefObj != null)
                return;
                
            _m_iEventInstanceSubRefObj = GRefdataCoreMgr.instance.commonEventChoiceRefCore.getRef(id);//从奖励事件实例子表中查找数据
            if (_m_iEventInstanceSubRefObj != null)
                return;
                
            _m_iEventInstanceSubRefObj = GRefdataCoreMgr.instance.commonEventDialogRefCore.getRef(id);//从奖励事件实例子表中查找数据
            if (_m_iEventInstanceSubRefObj != null)
                return;
                
            _m_iEventInstanceSubRefObj = GRefdataCoreMgr.instance.commonEventDispatchRefCore.getRef(id);//从奖励事件实例子表中查找数据
            if (_m_iEventInstanceSubRefObj != null)
                return;   
                
            _m_iEventInstanceSubRefObj = GRefdataCoreMgr.instance.commonEventPlotDialogRefCore.getRef(id);//从剧情对话事件实例表中查找数据
            if (_m_iEventInstanceSubRefObj != null)
                return;
            
            _m_iEventInstanceSubRefObj = GRefdataCoreMgr.instance.commonEventMiniGameRefCore.getRef(id);//从小游戏事件实例表中查找数据
            if (_m_iEventInstanceSubRefObj != null)
                return;
#endif
        }
        
        public static string assetPath { get { return "refdata_db/common_event.unity3d"; } }
        public static string objName { get { return "refdata_db/common_event.txt"; } }
        public static string tableName { get { return "common_event"; } }
    }
}