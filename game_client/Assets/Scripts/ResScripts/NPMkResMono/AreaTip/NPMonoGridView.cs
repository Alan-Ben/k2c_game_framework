using UnityEngine;
using System.Collections.Generic;
using System;

using ALPackage;

public enum ENPAreaGridState
{
    Empty,
    Occupied,
    OperationAccess,
    OperationDeny,
}
/// <summary>
/// 区域格子的颜色设置
/// </summary>
[Serializable]
public class NPAreaGridViewStateInfo
{
    // 需要修改的 sprite
    public SpriteRenderer chgColorSprite;

    [ALHeader("空闲颜色")]
    public Color emptyColor;
    [ALHeader("被占用颜色")]
    public Color occupiedColor;
    [ALHeader("当前操作对象的合法区域颜色")]
    public Color operationAccessColor;
    [ALHeader("当前操作对象的合法区域不可用的颜色")]
    public Color operationDenyColor;

    /// <summary>
    /// 根据带入参数设置对应的颜色
    /// </summary>
    public void changeViewColor(ENPAreaGridState _gridState)
    {
        switch (_gridState)
        {
            case ENPAreaGridState.Empty:
                _chgColor(emptyColor);
                break;
            case ENPAreaGridState.Occupied:
                _chgColor(occupiedColor);
                break;
            case ENPAreaGridState.OperationAccess:
                _chgColor(operationAccessColor);
                break;
            case ENPAreaGridState.OperationDeny:
                _chgColor(operationDenyColor);
                break;
        }
    }

    /// <summary>
    /// 设置对应颜色
    /// </summary>
    protected void _chgColor(Color _color)
    {
        ALUGUICommon.setUIObjFullColor(chgColorSprite, _color);
    }
}

public class NPMonoGridView : MonoBehaviour {

    //需要修改的sprite列表
    public List<NPAreaGridViewStateInfo> chgColorSpriteList;

    public void setGridState(ENPAreaGridState _gridState)
    {
        for(int i = 0; i < chgColorSpriteList.Count; i++)
        {
            if (null == chgColorSpriteList[i])
                continue;

            chgColorSpriteList[i].changeViewColor(_gridState);
        }
    }

    /// <summary>
    /// 设置显示层级
    /// </summary>
    /// <param name="_layer"></param>
    public void setLayer(int _layer)
    {
        for (int i = 0; i < chgColorSpriteList.Count; i++)
        {
            if (chgColorSpriteList[i].chgColorSprite != null)
                chgColorSpriteList[i].chgColorSprite.gameObject.layer = _layer;
        }
    }
}
