using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家升级成功弹窗属性变化Container
    /// </summary>
    public class GGUIWndPlayerLvlUpSucContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoPlayerLvlUpSucContainerItem, GGUIMonoPlayerLvlUpSucContainer, GGUIWndPlayerLvlUpSucContainerItem>
    {
        //窗口容器
        protected List<GGUIWndPlayerLvlUpSucContainerItem> _m_lItemList;

        public GGUIWndPlayerLvlUpSucContainer(GGUIMonoPlayerLvlUpSucContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndPlayerLvlUpSucContainerItem>();
        }

        protected override GGUIWndPlayerLvlUpSucContainerItem _createItemWnd(GGUIMonoPlayerLvlUpSucContainerItem _itemMono)
        {
            // 创建对象
            GGUIWndPlayerLvlUpSucContainerItem item = new GGUIWndPlayerLvlUpSucContainerItem(_itemMono);
            return item;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_dataList"></param>
        public void showItemList(List<PlayerLvlUpSucItemData> _dataList)
        {
            if (wnd == null || _dataList == null)
                return;

            GGUIWndPlayerLvlUpSucContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _dataList.Count; i++)
            {
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[i];
                itemWnd.showWnd();
                itemWnd.setInfo(_dataList[i], wnd.openItemShowAnim ? (i * wnd.eachItemShowAnimInterval + wnd.firstItemStartShowAnimDelay) : 0);
                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }

            ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, _dataList.Count > 0);
            ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, _dataList.Count <= 0);
        }

        /// <summary>
        /// 获取当前有变化的item的动画展示总时间
        /// </summary>
        /// <returns></returns>
        public float getChangeItemAniTotalTime()
        {
            if (wnd == null || !wnd.openItemShowAnim || _m_lItemList == null || _m_lItemList.Count == 0)
                return 0f;

            //获取每个item动画时长
            GGUIWndPlayerLvlUpSucContainerItem item = _m_lItemList[0];
            float eachItemAniTime = 0f;
            if (item != null && item.wnd != null && item.wnd.wndAnimation != null && !string.IsNullOrEmpty(item.wnd.showAniName))
            {
                AnimationClip clip = item.wnd.wndAnimation.GetClip(item.wnd.showAniName);
                if (clip != null)
                {
                    eachItemAniTime = clip.length;
                }
            }

            //获取有变化的item总数
            float chgItemTotalCount = 0;
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                if (_m_lItemList[i] != null && _m_lItemList[i].itemData.curValue > _m_lItemList[i].itemData.lastValue)
                {
                    chgItemTotalCount++;
                }
            }

            if (chgItemTotalCount == 0)
                return 0;

            //第一个item延时时长 + 总间隔时长 + 最后一个item动画时长
            return wnd.firstItemStartShowAnimDelay + (chgItemTotalCount - 1) * wnd.eachItemShowAnimInterval + eachItemAniTime ;
        }
    }

    public struct PlayerLvlUpSucItemData
    {
        public bool isSpecial;
        public long lastValue;
        public long curValue;
        public string name;
        public NPGTextureIndex icon;
        public string specialValueStr;
    }
}
