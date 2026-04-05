using System.Collections.Generic;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        // /// <summary>
        // /// 构建事件实例配表数据列表
        // /// </summary>
        // private void _initCommonEventInstanceRefObj()
        // {
        //     // Func<Func<long, bool, _ICommonEventInstanceSubRefObj>, CommonEventRefObj, bool, bool> findEventSubRefObj =
        //     //     (_findFunc, _eventRefObj, _checkFail) =>
        //     //     {
        //     //         if (_findFunc == null || _eventRefObj == null)
        //     //             return false;
        //     //
        //     //         _ICommonEventInstanceSubRefObj eventSubRefObj = _findFunc(_eventRefObj.id, _checkFail);
        //     //         if (eventSubRefObj != null)
        //     //         {
        //     //             _eventRefObj.eventInstanceSubRefObj = eventSubRefObj;
        //     //             return true;
        //     //         }
        //     //
        //     //         return false;
        //     //     };
        //
        //     _ICommonEventInstanceSubRefObj eventInstanceSubRefObj = null;
        //     commonEventRefCore.dealAllRef((_commonEvent) =>
        //     {
        //         if(_commonEvent == null)
        //             return;
        //
        //         // if (findEventSubRefObj(commonEventAwardRefCore.getRef, _commonEvent, true))//从奖励事件实例子表中查找数据
        //         //     return;
        //         // if (findEventSubRefObj(commonEventBossRefCore.getRef, _commonEvent, true))//从奖励事件实例子表中查找数据
        //         //     return;
        //         // if (findEventSubRefObj(commonEventChoiceRefCore.getRef, _commonEvent, true))//从奖励事件实例子表中查找数据
        //         //     return;
        //         // if (findEventSubRefObj(commonEventDialogRefCore.getRef, _commonEvent, true))//从奖励事件实例子表中查找数据
        //         //     return;
        //         // if (findEventSubRefObj(commonEventDispatchRefCore.getRef, _commonEvent, true))//从奖励事件实例子表中查找数据
        //         //     return;
        //         // if (findEventSubRefObj(commonEventPveRefCore.getRef, _commonEvent, true))//从奖励事件实例子表中查找数据
        //         //     return;
        //
        //         long eventId = _commonEvent.id;
        //         
        //         eventInstanceSubRefObj = commonEventAwardRefCore.getRef(eventId);//从奖励事件实例子表中查找数据
        //         if (eventInstanceSubRefObj != null)
        //         {
        //             _commonEvent.eventInstanceSubRefObj = eventInstanceSubRefObj;
        //             return;
        //         }
        //         
        //         eventInstanceSubRefObj = commonEventChoiceRefCore.getRef(eventId);//从奖励事件实例子表中查找数据
        //         if (eventInstanceSubRefObj != null)
        //         {
        //             _commonEvent.eventInstanceSubRefObj = eventInstanceSubRefObj;
        //             return;
        //         }
        //         
        //         eventInstanceSubRefObj = commonEventDialogRefCore.getRef(eventId);//从奖励事件实例子表中查找数据
        //         if (eventInstanceSubRefObj != null)
        //         {
        //             _commonEvent.eventInstanceSubRefObj = eventInstanceSubRefObj;
        //             return;
        //         }
        //         
        //         eventInstanceSubRefObj = commonEventDispatchRefCore.getRef(eventId);//从奖励事件实例子表中查找数据
        //         if (eventInstanceSubRefObj != null)
        //         {
        //             _commonEvent.eventInstanceSubRefObj = eventInstanceSubRefObj;
        //             return;
        //         }
        //         
        //         Debug.LogError($"[initCommonEventInstanceRefObj] 通用事件配置错误 事件:{_commonEvent.id}找不到对应子表数据");
        //     });
        // }
        
        /// <summary>
        /// 检测条件是否达成
        /// </summary>
        /// <returns></returns>
        public static bool checkDispatchEventConditionIsEnable(CommonEventDispatchCondRefObj _conditionRefObj, List<long> _heroIdList)
        {
            int enableConditionHeroCount = 0;//达成条件的大臣数量
            
            // 没有条件, 代表条件通过
            if (_conditionRefObj == null || _conditionRefObj.condition == null || _conditionRefObj.condition.isEmpty)
                return true;

            if (_heroIdList == null || _heroIdList.Count <= 0)//若没有传入大臣id列表, 大臣数据直接代入空值判断条件是否达成
                return _conditionRefObj.condition.IsEnable(null, null);

            if (_heroIdList.Count < _conditionRefObj.num)//若选中的大臣数量小于达成条件需要的大臣数量
                return false;

            foreach (long heroId in _heroIdList)
            {
                HeroRefObj heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(heroId);
                if(heroRefObj == null)
                    continue;

                // 若该大臣满足条件, 满足条件大臣数量增加
                if (_conditionRefObj.condition.IsEnable(heroRefObj, null))
                    enableConditionHeroCount++;

                // 若满足条件的大臣数量达到要求数量, 返回true
                if (enableConditionHeroCount >= _conditionRefObj.num)
                    return true;
            }

            return enableConditionHeroCount >= _conditionRefObj.num;
        }
        
        /// <summary>
        /// 检测条件是否达成
        /// </summary>
        /// <returns></returns>
        public static bool checkDispatchEventConditionIsEnable(CommonEventDispatchCondRefObj _conditionRefObj, List<_IHeroCardShow> _heroShowInfoList)
        {
            int enableConditionHeroCount = 0;//达成条件的大臣数量
            
            // 没有条件, 代表条件通过
            if (_conditionRefObj == null || _conditionRefObj.condition == null || _conditionRefObj.condition.isEmpty)
                return true;

            if (_heroShowInfoList == null || _heroShowInfoList.Count <= 0)//若没有传入大臣id列表, 大臣数据直接代入空值判断条件是否达成
                return _conditionRefObj.condition.IsEnable(null, null);

            if (_heroShowInfoList.Count < _conditionRefObj.num)//若选中的大臣数量小于达成条件需要的大臣数量
                return false;

            foreach (_IHeroCardShow heroShowInfo in _heroShowInfoList)
            {
                if(heroShowInfo == null || heroShowInfo.heroRefObj == null)
                    continue;

                // 若该大臣满足条件, 满足条件大臣数量增加
                if (_conditionRefObj.condition.IsEnable(heroShowInfo.heroRefObj, null))
                    enableConditionHeroCount++;

                // 若满足条件的大臣数量达到要求数量, 返回true
                if (enableConditionHeroCount >= _conditionRefObj.num)
                    return true;
            }

            return enableConditionHeroCount >= _conditionRefObj.num;
        }
    }
}