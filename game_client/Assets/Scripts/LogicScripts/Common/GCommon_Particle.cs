using System;
using System.Collections.Generic;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public static partial class GCommon
    {
        /// <summary>
        /// 展示道具粒子上漂效果
        /// </summary>
        /// <param name="_itemList"></param>
        /// <param name="_srcUIObj"></param>
        public static void showItemParticle(List<NPCommon_ItemInfo> _itemList, RectTransform _srcUIObj, long _specialParticleId = 0)
        {
            if (_itemList == null || _itemList.Count == 0 || _srcUIObj == null)
                return;

            for (int i = 0; i < _itemList.Count; i++)
            {
                if (_itemList[i] == null)
                    continue;

                showItemParticle(_itemList[i], _srcUIObj, _specialParticleId);
            }
        }

        /// <summary>
        /// 展示道具粒子上漂效果
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_srcUIObj"></param>
        public static void showItemParticle(NPCommon_ItemInfo _item, RectTransform _srcUIObj, long _specialParticleId = 0, Action _onAllParticleDone = null, bool _showRealCount = false)
        {
            if (_item == null || _srcUIObj == null)
                return;
            
            showItemParticle(_item, getUIRootPos(_srcUIObj), _specialParticleId, _onAllParticleDone, _showRealCount);
        }

        public static void showItemParticle(ENPItemType _itemType, long _subId, long _count, RectTransform _srcUIObj, long _specialParticleId = 0, Action _onAllParticleDone = null, bool _showRealCount = false)
        {
            showItemParticle(_itemType, _subId, _count, getUIRootPos(_srcUIObj), _specialParticleId, _onAllParticleDone, _showRealCount);
        }
        
        /// <summary>
        /// 展示道具粒子上漂效果
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_srcUIObj"></param>
        public static void showItemParticle(NPCommon_ItemInfo _item, Vector2 _uiPos, long _specialParticleId = 0, Action _onAllParticleDone = null, bool _showRealCount = false)
        {
            if (_item == null)
                return;

            ENPItemType itemType = (ENPItemType)_item.getItemType();
            long itemCount = _item.getCount();
            long itemId = _item.getSubId();

            showItemParticle(itemType, itemId, itemCount, _uiPos, _specialParticleId, _onAllParticleDone, _showRealCount);
        }
        
         /// <summary>
        /// 展示道具粒子上漂效果
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_srcUIObj"></param>
        public static void showItemParticle(ENPItemType _itemType, long _subId, long _count, Vector2 _uiPos, long _specialParticleId = 0, Action __onAllParticleDone = null, bool _showRealCount = false)
        {
            //需要展示的粒子数量
            int particleNum = 0;
            //需要展示的粒子ID
            long particleId = 0;
            //粒子背景图
            NPGSpriteIndex bgIndex = null;
            //是否需要展示道具数量
            bool needShowItemCount = true;

            //货币类型与普通道具类型区分开
            if (_itemType == ENPItemType.CURRENCY)
            {
                //货币类型只展示图标
                if (_showRealCount)
                    particleNum = (int)_count;
                else
                    particleNum = ParticleNumRangeInfo.getTargetParticleNum(_count);
                if (_specialParticleId != 0)
                    particleId = _specialParticleId;
                else
                    particleId = GRefdataCoreMgr.instance.npGeneral.currency_particle_id;

                bgIndex = null;
                needShowItemCount = false;
            }
            else if(_itemType == ENPItemType.BAG_ITEM)
            {
                //道具类型只展示1个，包含道具图标、背景图、数量
                particleNum = 1;
                if (_specialParticleId != 0)
                    particleId = _specialParticleId;
                else
                    particleId = GRefdataCoreMgr.instance.npGeneral.bag_item_particle_id;

                bgIndex = getItemQualityIcon(_itemType, _subId);
                needShowItemCount = true;
            }
            else
            {
                if (_showRealCount)
                    particleNum = (int)_count;
                else
                    particleNum = ParticleNumRangeInfo.getTargetParticleNum(_count);
                if (_specialParticleId != 0)
                    particleId = _specialParticleId;
                else
                    particleId = GRefdataCoreMgr.instance.npGeneral.currency_particle_id;

                bgIndex = null;
                needShowItemCount = false;
            }

            //播放粒子动画
            GGUIHarvestCore.instance.startHarvestCollection(GGUIHarvestUtil.getEHarvestType(_itemType, _subId), _uiPos, particleNum, particleId, 1,
            (_particleItem) =>
            {
                if (_particleItem != null)
                    _particleItem.setBagItemInfo(getItemTexIcon(_itemType, _subId), bgIndex, _count, needShowItemCount);
            }, __onAllParticleDone);
        }

        /// <summary>
        /// 根据屏幕坐标获取粒子UI坐标
        /// </summary>
        /// <param name="_screenPos"></param>
        /// <returns></returns>
        public static void getParticleUIPosByScreenPos(Vector2 _screenPos, Action<Vector2> _onGetUIPos)
        {
            if(!GGUIWndBurstParticle.instance.isLoaded)
                GGUIWndBurstParticle.instance.load();

            GGUIWndBurstParticle.instance.regLoadDoneDelegate(() =>
            {

                if (null == GGUIWndBurstParticle.instance.wnd?.sfxParent)
                {
 					_onGetUIPos?.Invoke(Vector2.zero);
                    return;
                }
            
                //屏幕坐标转换到UGUI坐标
                Vector2 uiPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)GGUIWndBurstParticle.instance.wnd?.sfxParent,
                    _screenPos,
                    Game.instance.mainCamera.uiCamera,
                    out uiPos);

                _onGetUIPos?.Invoke(uiPos);
            });
        }
    }
}