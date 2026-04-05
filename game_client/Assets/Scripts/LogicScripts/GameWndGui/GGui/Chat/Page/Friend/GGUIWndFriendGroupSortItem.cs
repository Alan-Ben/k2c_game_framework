using System;
using ALPackage;
using DG.Tweening;
using GOE;
using UnityEngine;
using UnityEngine.EventSystems;

public class GGUIWndFriendGroupSortItem  : _ATALBasicUISubWnd<GGUIMonoFriendGroupSortItem>
{
    public event Action<GGUIWndFriendGroupSortItem> onDrag;
    private PlayerFriendGroup _m_info;
    private Tweener _m_tweener;
    private bool _m_isMoveing = false;

    public GGUIWndFriendGroupSortItem(GGUIMonoFriendGroupSortItem _wnd) : base(_wnd)
    {
        initWnd();
    }

    public PlayerFriendGroup info { get => _m_info; }

    protected override void _onShowWnd()
    {
        
    }

    protected override void _onHideWnd()
    {
        
    }

    protected override void _onReset()
    {
        
    }

    protected override void _onDiscard()
    {
        _m_tweener?.Kill(true);
    }

    protected override void _onWndInitDone()
    {
        if(null == wnd)
            return;
        ALUGUICommon.combineBtnClick(wnd.btnRemove, _clickRemoveGroup);
        ALUGUICommon.combineBeginDrag(wnd.dragGo, _onBeginDrag);
        ALUGUICommon.combineDrag(wnd.dragGo, _onDrag);
        ALUGUICommon.combineEndDrag(wnd.dragGo, _onEndDrag);
    }

    /// <summary>
    /// 爱开始拖拽
    /// </summary>
    /// <param name="obj"></param>
    private void _onBeginDrag(PointerEventData eventData)
    {
        ALUGUICommon.setGameObjEnable(wnd.onDragShow, true);
    }

    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="obj"></param>
    private void _onDrag(PointerEventData eventData)
    {
        if (null != wnd.dragFollowGo)
        {
            //跟随
            _setFollowTargetPos(eventData.position);
            onDrag?.Invoke(this);
        }
        
        
    }

    private void _setFollowTargetPos(Vector3 _screenPoint)
    {
        if (null == wnd.dragFollowGo || null == wnd.dragFollowGo.parent)
            return;
        //目标点的屏幕坐标
        RectTransform parentRect = wnd.dragFollowGo.parent as RectTransform;
        //目标点的UGUI坐标
        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            _screenPoint,
            Game.instance.mainCamera.uiCamera, 
            out uiPos);
            
        Vector2 tempPos = wnd.dragFollowGo.anchoredPosition;
        tempPos.y = uiPos.y + parentRect.rect.height / 2;
        ALUGUICommon.setUIPos(wnd.dragFollowGo, tempPos);
        
    }

    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="obj"></param>
    private void _onEndDrag(PointerEventData eventData)
    {
        _moveToTargetTrans();
        ALUGUICommon.setGameObjEnable(wnd.onDragShow, false);
    }

    /// <summary>
    /// 点击移除分组
    /// </summary>
    /// <param name="obj"></param>
    private void _clickRemoveGroup(GameObject obj)
    {
        if (!_m_info.getCanRemove())
        {
            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friends_default_group_cannot_remove));
            return;
        }
        
        //二次确认弹窗，点击确认移除
        NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.friends_group_del_tip_desc)
            ,TextTranslate.instance.getLanguage(TransKeyConst.cancel)
            ,null
            ,TextTranslate.instance.getLanguage(TransKeyConst.confirm)
            , () =>
            {
                NPPlayer.instance.friendsComp.reqDeleteFriendGroup(_m_info.dbId, null);
            });
    }

    /// <summary>
    /// 设置显示信息
    /// </summary>
    /// <param name="_info"></param>
    public void setInfo(PlayerFriendGroup _info)
    {
        _m_info = _info;
        _refreshWnd();
    }

    /// <summary>
    /// 刷新窗口
    /// </summary>
    private void _refreshWnd()
    {
        if(null == wnd)
            return;
        ALUGUICommon.setLabelTxt(wnd.txtGroupName, _m_info.getGroupName());
    }

    /// <summary>
    /// 判断某个item是不是在可切换位置的范围内
    /// </summary>
    /// <param name="_itemWnd"></param>
    /// <returns></returns>
    public bool isInSwitchRect(GGUIWndFriendGroupSortItem _itemWnd)
    {
        //切换移动中，bu
        if (_m_isMoveing)
            return false;
        if (null == _itemWnd || null == _itemWnd.wnd || null == _itemWnd.wnd.dragFollowGo)
            return false;
        //在差值范围内
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
            Game.instance.mainCamera.uiCamera,
            _itemWnd.wnd.dragFollowGo.position);
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
            screenPoint,
            Game.instance.mainCamera.uiCamera,
            out localPosition);
            //= wnd.dragFollowGo.rectPositionToLocalPosition(_itemWnd.wnd.dragFollowGo.anchoredPosition);
        if (Mathf.Abs(localPosition.y + rectTransform.rect.height / 2) < wnd.checkInRectOffSetY)
            return true;
        
        return false;
    }

    /// <summary>
    /// 和某个item切换位置
    /// </summary>
    /// <param name="_itemWnd"></param>
    public void switchWitchItemWnd(GGUIWndFriendGroupSortItem _itemWnd)
    {
        if (null == wnd)
            return;
        int transformIndex = rectTransform.GetSiblingIndex();
        int targetTransformIndex = _itemWnd.rectTransform.GetSiblingIndex();
        //先记录当前位置，用于待会移动的起始位置
        // Vector3 moveStartPos = wnd.dragEndTargetGo.position;
        Vector3 curPosition = _itemWnd.wnd.dragFollowGo.position;
        //先设置小的，再设置大的
        if (transformIndex < targetTransformIndex)
        {
            _itemWnd.rectTransform.SetSiblingIndex(transformIndex);
            rectTransform.SetSiblingIndex(targetTransformIndex);
        }
        else
        {
            rectTransform.SetSiblingIndex(targetTransformIndex);
            _itemWnd.rectTransform.SetSiblingIndex(transformIndex);
        }
        //设置一下正在拖拽的item位置
        ALCommonActionMonoTask.addNextFrameTask(() =>
        {
            if(_itemWnd != null && _itemWnd.wnd != null && _itemWnd.wnd.dragFollowGo != null)
                _itemWnd.wnd.dragFollowGo.position = curPosition;
        });
        // //刷新位置
        // _itemWnd._moveToTargetTrans();
        // _moveToTargetTrans(moveStartPos);
    }

    /// <summary>
    /// 移动到终点定位位置
    /// </summary>
    private void _moveToTargetTrans()
    {
        if (null == wnd || null == wnd.dragFollowGo)
            return;
        _moveToTargetTrans(wnd.dragFollowGo.position);
    }
    
    private void _moveToTargetTrans(Vector3  _startPos)
    {
        if (null == wnd || null == wnd.dragEndTargetGo || null == wnd.dragFollowGo)
            return;
        ALCommonTaskController.CommonActionAddNextFrameTask(() =>
        {
            _m_tweener?.Kill(true);
            _m_isMoveing = true;
            Vector3 curPos = _startPos;
            wnd.dragFollowGo.position = curPos;
            _m_tweener = DOTween.To(()=> curPos.y, x => curPos.y = x, wnd.dragEndTargetGo.position.y, 0.1f)
                .OnUpdate(() =>
                {
                    wnd.dragFollowGo.position = curPos;
                })
                .OnComplete(() =>
            {
                wnd.dragFollowGo.position = wnd.dragEndTargetGo.position;
                _m_isMoveing = false;
            }).SetAutoKill(true);
        });
    }
}