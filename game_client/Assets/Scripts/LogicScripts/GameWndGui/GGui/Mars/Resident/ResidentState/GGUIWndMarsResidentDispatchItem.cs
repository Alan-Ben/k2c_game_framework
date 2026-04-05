using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 居民派遣列表Item窗口
    /// </summary>
    public class GGUIWndMarsResidentDispatchItem : _ANPGGUIBasicGridItemWnd<GGUIMonoMarsResidentDispatchItem>
    {
        private MarsBuildingInfo _m_buildingInfo; // 绑定的建筑信息
     
        private NPGGuiWndTexture _m_buildingIconWnd; // 建筑图标窗口包装

        public GGUIWndMarsResidentDispatchItem(GGUIMonoMarsResidentDispatchItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 绑定派遣按钮
            ALUGUICommon.combineBtnClick(wnd.btnDispatch, _onClickDispatch);

            // 初始化建筑图标包装
            if (wnd.imgBuild != null)
            {
                _m_buildingIconWnd = new NPGGuiWndTexture(wnd.imgBuild);
            }
        }

        protected override void _onDiscard()
        {
            _m_buildingInfo = null;

            if (_m_buildingIconWnd != null)
            {
                _m_buildingIconWnd.discard();
                _m_buildingIconWnd = null;
            }

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnDispatch, _onClickDispatch);
            }

            _m_buildingInfo = null;
        }

        protected override void _onShowWnd()
        {
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_buildingIconWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_buildingIconWnd?.discardTexture();
        }

        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 设置建筑数据
        /// </summary>
        public void setData(MarsBuildingInfo _info)
        {
            _m_buildingInfo = _info;
            refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow || _m_buildingInfo == null)
                return;

            // 建筑图标显示
            if (_m_buildingIconWnd != null)
            {
                NPGTextureIndex iconIndex = _m_buildingInfo.refObj.building_icon;
                if (iconIndex != null && iconIndex.enable())
                {
                    _m_buildingIconWnd.setTexture(iconIndex);
                }
            }
            
            // 建筑名称
            if (wnd.txtBuildName != null)
                ALUGUICommon.setLabelTxt(wnd.txtBuildName, _m_buildingInfo.nameTranslated);

            // 建筑当前已派遣人数 / 可派遣上限 (slot_num)
            int peopleCount = 0;// 当前已派遣人数
            long slotPeopleLimit = 0;// 可派遣上限
            if (_m_buildingInfo.settleSlotData.isValid())
            {
                peopleCount = _m_buildingInfo.settleSlotData.peopleCount;
                slotPeopleLimit = _m_buildingInfo.settleSlotData.peopleLimit;
            }

            if (wnd.txtBuildPeopleNum != null)
                ALUGUICommon.setLabelTxt(wnd.txtBuildPeopleNum, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, peopleCount, slotPeopleLimit));

            long canDispatchNum = _getCanDispatchNum();// 当前可派遣人数

            if (wnd.txtDispatchPeopleNum != null)
                ALUGUICommon.setLabelTxt(wnd.txtDispatchPeopleNum, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, canDispatchNum));

            // 状态显示: 根据是否可派遣展示互斥对象组
            ALUGUICommon.setGameObjEnable(wnd.canDispatchShowObjList, canDispatchNum > 0);
            ALUGUICommon.setGameObjEnable(wnd.cannotDispatchShowObjList, canDispatchNum <= 0);
        }

        /// <summary>
        /// 获取当前建筑可派遣人数
        /// </summary>
        /// <returns></returns>
        private long _getCanDispatchNum()
        {
            // 可派遣人数 = min(休息中居民数, 建筑剩余空位, 默认预设值 > 0)
            
            long peopleCount = 0;// 当前已派遣人数
            long slotPeopleLimit = 0;//// 可派遣上限
            if (_m_buildingInfo != null && _m_buildingInfo.settleSlotData.isValid())
            {
                peopleCount = _m_buildingInfo.settleSlotData.peopleCount;
                slotPeopleLimit = _m_buildingInfo.settleSlotData.peopleLimit;
            }
            long remainSlot = slotPeopleLimit - peopleCount;//建筑剩余空位
            
            long idleNum = NPPlayer.instance.marsComp.peopleSubComponent.idlePeopleNum;//休息中居民数
            int defaultDispatchNum = GRefdataCoreMgr.instance.npGeneral.mars_resident_dispatch_default_num;// 默认派遣数量

            long canDispatchNum = Math.Min(idleNum, remainSlot);//先去掉休息中居民数和剩余空位的最小值
            if (defaultDispatchNum > 0)//在默认派遣数量大于0时，才和默认派遣数量比较
                canDispatchNum = Math.Min(canDispatchNum, defaultDispatchNum);

            return canDispatchNum;
        }
        
        private void _onClickDispatch(GameObject _go)
        {
            if (_m_buildingInfo == null)
                return;

            long canDispatchNum = _getCanDispatchNum();
            if (canDispatchNum <= 0)
            {
                // 不可派遣，直接返回（可加提示）
                return;
            }

            NPPlayer.instance.marsComp.buildingSubComponent.ReqDispatchPeople(_m_buildingInfo.buildRefId, (int)canDispatchNum,
                () =>
                {
                    // 这里不刷新，在外层窗口监听刀消息变化时刷新
                    // //派遣后本地刷新
                    // refreshWnd();
                });
        }
    }
}
