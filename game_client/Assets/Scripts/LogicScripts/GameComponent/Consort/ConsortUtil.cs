using System;
using System.Collections.Generic;
using Common.ConsortEnum;
using MJSDK_Package;
using NPEnum;

namespace GOE
{
    public class ConsortUtil
    {
        public static bool businessSkillAdvanceComprehendIsOn;

        public static bool consortCallDialogNotShowCG;//妃子邀约对话是否不显示CG
        
        /// <summary>
        /// 获取未解锁的妃子列表
        /// </summary>
        /// <param name="_dealItem"></param>
        /// <returns></returns>
        public static List<ConsortRefShowInfo> getLockConsortList(Action<ConsortRefShowInfo> _dealItem)
        {
            List<ConsortRefShowInfo> consortList = new List<ConsortRefShowInfo>();
            GRefdataCoreMgr.instance.consortRefCore.dealAllRef((_refObj) =>
            {
                if (null == _refObj)
                    return;
                if (NPPlayer.instance.consortComp.getConsortUnlockType(_refObj.id) == EGameCommonUnlockType.LOCK)
                {
                    ConsortRefShowInfo data = new ConsortRefShowInfo(_refObj);
                    _dealItem?.Invoke(data);
                    consortList.Add(data);
                }
            });
            
            // 排序规则: 初始亲密度+魅力值 -> 品质 -> ID
            consortList.Sort(sortConsortShowInfoDefaultWithoutUnlockCheck);
            return consortList;
        }

        /// <summary>
        /// 获取已解锁的妃子列表
        /// </summary>
        /// <param name="_newConsortInfoObj">是否是新的妃子信息对象, 若为false, 将会直接给component中的妃子数据</param>
        /// <param name="_itemCheckFunc"></param>
        /// <returns></returns>
        public static List<GGottenConsortInfo> getUnlockConsortList(bool _newConsortInfoObj, Func<GGottenConsortInfo, bool> _itemCheckFunc)
        {
            List<GGottenConsortInfo> consortList = new List<GGottenConsortInfo>();
            
            foreach (GGottenConsortInfo item in  NPPlayer.instance.consortComp.consortList)
            {
                if(item == null)
                    continue;

                GGottenConsortInfo consortInfo = item;
                if (_newConsortInfoObj)
                    consortInfo = new GGottenConsortInfo(item.serverConsortInfo);

                if (_itemCheckFunc == null || _itemCheckFunc(consortInfo))
                {
                    consortList.Add(consortInfo);   
                }
            }

            // 排序规则: 初始亲密度+魅力值 -> 品质 -> ID
            consortList.Sort(sortConsortShowInfoDefaultWithoutUnlockCheck);
            
            return consortList;
        }
        
        /// <summary>
        /// 遍历所有已解锁妃子列表
        /// </summary>
        /// <param name="_action"></param>
        public static void dealAllUnlockConsortList(Action<GGottenConsortInfo> _action)
        {
            if(_action == null)
                return;
            
            foreach (GGottenConsortInfo item in  NPPlayer.instance.consortComp.consortList)
            {
                if(item == null)
                    continue;
                
                _action.Invoke(item);
            }
        }

        /// <summary>
        /// 妃子展示数据默认排序规则 : 已解锁>未解锁 -> 亲密度 + 魅力值 -> 品质高到低 -> id
        /// </summary>
        /// <param name="_consortShowInfo1"></param>
        /// <param name="_consortShowInfo2"></param>
        /// <returns></returns>
        public static int sortConsortShowInfoDefault(_IConsortShowInfo _consortShowInfo1, _IConsortShowInfo _consortShowInfo2)
        {
            if (_consortShowInfo2 == null)
                return -1;
            if (_consortShowInfo1 == null)
                return 1;

            EGameCommonUnlockType consortUnlockType1 = _consortShowInfo1.unlockType;
            EGameCommonUnlockType consortUnlockType2 = _consortShowInfo2.unlockType;
            if (consortUnlockType1 != consortUnlockType2)
                return -consortUnlockType1.CompareTo(consortUnlockType2);//因为EGameCommonUnlockType中未解锁<已解锁, 所以这里已解锁要排在前面, 要加-
            
            long intimacyPlusCharmX = _consortShowInfo1.intimacy + _consortShowInfo1.charm;
            long intimacyPlusCharmY = _consortShowInfo2.intimacy + _consortShowInfo2.charm;
            if (intimacyPlusCharmX != intimacyPlusCharmY)
                return -intimacyPlusCharmX.CompareTo(intimacyPlusCharmY);
                
            EQuality qualityX = GCommon.getItemQuality(ENPItemType.CONSORT, _consortShowInfo1.consortId);
            EQuality qualityY = GCommon.getItemQuality(ENPItemType.CONSORT, _consortShowInfo2.consortId);
            if (qualityX != qualityY)
                return -qualityX.CompareTo(qualityY);
                
            if (_consortShowInfo1.consortId < _consortShowInfo2.consortId)
                return -1;
            if (_consortShowInfo1.consortId > _consortShowInfo2.consortId)
                return 1;
            return 0;
        }

        /// <summary>
        /// 亲密度 + 魅力值 -> 品质高到低 -> id
        /// </summary>
        /// <param name="_consortShowInfo1"></param>
        /// <param name="_consortShowInfo2"></param>
        /// <returns></returns>
        public static int sortConsortShowInfoDefaultWithoutUnlockCheck(_IConsortShowInfo _consortShowInfo1, _IConsortShowInfo _consortShowInfo2)
        {
            if (_consortShowInfo2 == null)
                return -1;
            if (_consortShowInfo1 == null)
                return 1;

            long intimacyPlusCharmX = _consortShowInfo1.intimacy + _consortShowInfo1.charm;
            long intimacyPlusCharmY = _consortShowInfo2.intimacy + _consortShowInfo2.charm;
            if (intimacyPlusCharmX != intimacyPlusCharmY)
                return -intimacyPlusCharmX.CompareTo(intimacyPlusCharmY);
                
            EQuality qualityX = GCommon.getItemQuality(ENPItemType.CONSORT, _consortShowInfo1.consortId);
            EQuality qualityY = GCommon.getItemQuality(ENPItemType.CONSORT, _consortShowInfo2.consortId);
            if (qualityX != qualityY)
                return -qualityX.CompareTo(qualityY);
                
            if (_consortShowInfo1.consortId < _consortShowInfo2.consortId)
                return -1;
            if (_consortShowInfo1.consortId > _consortShowInfo2.consortId)
                return 1;
            return 0;
        }

        /// <summary>
        /// 获取经营技能状态
        /// </summary>
        /// <returns></returns>
        public static void getBusinessSkillState(ConsortBusinessSkillRefObj _businessSKillRefObj, GGottenConsortInfo _gottenConsort, Action<EConsortBusinessSkillItemState> _onGetSkillState)
        {
            if (_businessSKillRefObj == null || _gottenConsort == null ||
                _businessSKillRefObj.unlock_need_intimacy > _gottenConsort.intimacy)
            {
                _onGetSkillState?.Invoke(EConsortBusinessSkillItemState.LOCK);
                return;
            }

            _gottenConsort.getBusinessSkillInfo(_businessSKillRefObj.id, (_businessSkillInfo) =>
            {
                if (_businessSkillInfo == null ||
                    ((_businessSkillInfo.advanceOpCount + _businessSkillInfo.normalOpCount) <= 0))
                {
                    _onGetSkillState?.Invoke(EConsortBusinessSkillItemState.UNLOCK_NO_OP);
                    return;
                }
                
                _onGetSkillState?.Invoke(_businessSkillInfo.isReachAddMax ? EConsortBusinessSkillItemState.UNLOCK_ADD_MAX : EConsortBusinessSkillItemState.UNLOCK_ADD_NOT_MAX);
            });
        }

        /// <summary>
        /// 获取经营技能状态
        /// </summary>
        /// <param name="_businessSKillRefObj"></param>
        /// <param name="_businessSkillInfo"></param>
        /// <param name="_intimacy"></param>
        /// <returns></returns>
        public static EConsortBusinessSkillItemState getBusinessSkillState(ConsortBusinessSkillRefObj _businessSKillRefObj, ConsortBusinessSkillInfo _businessSkillInfo, long _intimacy)
        {
            if (_businessSKillRefObj == null || _businessSKillRefObj.unlock_need_intimacy > _intimacy)
            {
                return EConsortBusinessSkillItemState.LOCK;
            }

            // 若_businessSKillRefObj的id和_businessSkillInfo的id不同, _businessSkillInfo无效
            if (_businessSkillInfo == null || _businessSkillInfo.skillId != _businessSKillRefObj.id
                || ((_businessSkillInfo.advanceOpCount + _businessSkillInfo.normalOpCount) <= 0))
            {
                return EConsortBusinessSkillItemState.UNLOCK_NO_OP;
            }
            
            return _businessSkillInfo.isReachAddMax ? EConsortBusinessSkillItemState.UNLOCK_ADD_MAX : EConsortBusinessSkillItemState.UNLOCK_ADD_NOT_MAX;
        }
        
        /// <summary>
        /// 根据类型显示顺序 获取处于该顺序的类型
        /// 故事类型显示顺序 首次邀约FIRST_CALL ＞ 二次邀约SECOND_CALL ＞ 邀约CALL ＞ 结婚MARRY ＞ 游玩PLAY
        /// </summary>
        /// <param name="_showOrder">从0开始, 最大为 （EConsortStoryType类型数量 - 1）</param>
        /// <returns></returns>
        public static EConsortStoryType consortStoryShowOrderToType(int _showOrder)
        {
            switch (_showOrder)
            {
                case 0:
                    return EConsortStoryType.FIRST_CALL;
                case 1:
                    return EConsortStoryType.SECOND_CALL;
                case 2:
                    return EConsortStoryType.CALL;
                case 3:
                    return EConsortStoryType.MARRY;
                case 4:
                    return EConsortStoryType.PLAY;
                default:
                    return EConsortStoryType.NONE;
            }
        }

        /// <summary>
        /// 获取妃子皮肤展示信息
        /// </summary>
        /// <returns></returns>
        public static _IConsortSkinShowInfo getConsortSkinShowInfo(long _skinId)
        {
            GConsortSkinRefObj skinRefObj = GRefdataCoreMgr.instance.consortSkinRefCore.getRef(_skinId);
            if (skinRefObj == null)
                return null;

            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(skinRefObj.consort_id);
            if (consortInfo != null)
            {
                GConsortSkinInfo skinInfo = consortInfo.getSkinInfo(_skinId);
                if(skinInfo != null)
                {
                    return skinInfo;
                }
            }

            return skinRefObj;
        }
    }
}