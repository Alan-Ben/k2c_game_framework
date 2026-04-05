using System.Collections.Generic;
using ALPackage;using GOE;
using UnityEngine;
using UnityEngine.UI;


//周卡选形象的item的container
public class GGUIWndWeekCardAssignTDShowItemContainer : _ATNPGGUIWndSingleChoiceContainer<GGUIMonoWeekCardAssignTDShowItem,GGUIMonoWeekCardAssignTDShowItemContainer,GGUIWndWeekCardAssignTDShowItem>
{
    private List<GGUIWndWeekCardAssignTDShowItem> _m_lItemGroupList;
    public GGUIWndWeekCardAssignTDShowItemContainer(GGUIMonoWeekCardAssignTDShowItemContainer _containerMono) : base(_containerMono)
    {
        initWnd();
    }
    
    protected override void _onShowWndEx()
    {
        
    }

    protected override void _onHideWndEx()
    {
        foreach (GGUIWndWeekCardAssignTDShowItem iconItem in _m_lItemGroupList)
        {
            iconItem?.discard();
        }
        _m_lItemGroupList?.Clear();
    }

    protected override void _onResetEx()
    {
        _m_lItemGroupList?.Clear();
    }

    protected override void _onDiscardEx()
    {
        _m_lItemGroupList?.Clear();
        _m_lItemGroupList = null;
    }

    protected override void _onWndInitDoneEx()
    {
        _m_lItemGroupList = new List<GGUIWndWeekCardAssignTDShowItem>();
    }


    protected override GGUIWndWeekCardAssignTDShowItem _createItemWnd(GGUIMonoWeekCardAssignTDShowItem _itemMono)
    {
        GGUIWndWeekCardAssignTDShowItem itemWnd = new GGUIWndWeekCardAssignTDShowItem(_itemMono);
        return itemWnd;
    }

    public void showItemList(List<_ICommonIconShowData> _itemList)
    {
        GGUIWndWeekCardAssignTDShowItem tempItemWnd = null;
        _ICommonIconShowData itemData = null;
        int count = 0;
        for (int i = 0; i < _itemList.Count; i++)
        {
            itemData = _itemList[i];
            if (null == itemData)
                continue;
            
            if (count >= _m_lItemGroupList.Count)
            {
                tempItemWnd = addItemWnd();
                if (tempItemWnd == null)
                    return;
                //放入数据队列
                _m_lItemGroupList.Add(tempItemWnd);
            }
            else
            {
                tempItemWnd = _m_lItemGroupList[count];
            }

            tempItemWnd.setInfo(itemData);
            count++;
        }

        for (int i = _m_lItemGroupList.Count; i > count; i--)
        {
            removeItemWnd(_m_lItemGroupList[i - 1]);
            _m_lItemGroupList.RemoveAt(i - 1);
        }

        _refreshContentLayout();
    }

    /// <summary>
    /// 刷新容器布局
    /// </summary>
    private void _refreshContentLayout()
    {
        ALCommonActionMonoTask.addNextFrameTask(() =>
        {
            if (wnd == null || wnd.itemContainer == null)
                return;

            LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
        });
    }

}
