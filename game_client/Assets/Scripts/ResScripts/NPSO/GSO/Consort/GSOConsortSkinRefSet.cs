using System;
using ALPackage;
using NPEnum;
using SQLite4Unity3d;
using System.Collections.Generic;
using GOE;


/**************
 * 情人皮肤配表
 **/

[System.Serializable]
public class GConsortSkinRefObj : _IALBasicRefObj
#if NP_GAME
    , _IConsortSkinShowInfo
#endif
{
    public long _refId { get { return id; } }

    public long id;//皮肤id
    public long consort_id;//妃子id
    public int sort_v;//排序id(小的排前面)
    public NPGTextureIndex consort_head;//妃子头像
    public NPGTextureIndex consort_card_image;//妃子卡牌半身像
    public NPGTextureIndex skin_card_img;//皮肤卡牌图片
    public NPGGoIndex td_show;//全身形象
    public NPGGoIndex td_bg_index;//形象背景GO
    public NPCommonCostItem unlock_cost_item;//解锁消耗
    public string unlock_condition_desc;//解锁条件描述
    public List<string> unlock_condition_desc_args;//解锁条件描述参数

    #region 皮肤等级列表

    [NonSerialized]
    private List<GConsortSkinLvlRefObj> _m_lSkinLvlList;
    
    public List<GConsortSkinLvlRefObj> skinLvlList
    {
        get
        {
            if (null == _m_lSkinLvlList)
            {
                _m_lSkinLvlList = new List<GConsortSkinLvlRefObj>();

#if NP_GAME
                GRefdataCoreMgr.instance.consortSkinLvlRefCore.dealAllRef((_refObj) =>
                {
                    if (_refObj != null && _refObj.consort_skin_id == id)
                        _m_lSkinLvlList.Add(_refObj);
                });
                
                _m_lSkinLvlList.Sort((_a, _b) =>
                {
                    if (_b == null)
                        return -1;
                    if (_a == null)
                        return 1;

                    return _a.lvl.CompareTo(_b.lvl);
                });
#endif
            }
            return _m_lSkinLvlList;
        }
    }

    #endregion

    /// <summary>
    /// 检查解锁消耗物品是否足够
    /// </summary>
    /// <returns></returns>
    public bool checkUnlockCostItemEnough(bool _showTip)
    {
#if NP_GAME
        return unlock_cost_item == null || !unlock_cost_item.IsValid || GCommon.isItemEnough(unlock_cost_item, _showTip);
#else
        return false;
#endif
    }
    
#if NP_GAME
    public long skinId { get { return id; } }
    public GConsortSkinRefObj skinRefObj { get { return this; } }
    public NPGTextureIndex consortHeadIcon { get { return consort_head; } }
    public NPGTextureIndex consortCardImage { get { return consort_card_image; } }
    public NPGTextureIndex skinIcon { get { return GCommon.getItemTexIcon(ENPItemType.CONSORT_SKIN, skinId); } }
    public NPGTextureIndex skinCardImg { get { return skin_card_img; } }
    public NPGGoIndex tdShow { get { return td_show; } }
    public NPGGoIndex tdBgIndex { get { return td_bg_index; } }
#endif
}

public class GSOConsortSkinRefSet : _TALSOBasicRefSet<GConsortSkinRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_skin"; } }
}
