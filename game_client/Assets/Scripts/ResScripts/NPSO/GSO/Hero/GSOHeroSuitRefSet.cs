using ALPackage;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

/// <summary>
/// 伙伴套系表
/// </summary>
[Serializable]
public class HeroSuitRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//阶段
    public string halo_name;//套系名称
    public string desc;//套系描述
    public List<string> desc_args;//套系描述参数
    public List<long> halo_suit_skill_id_list;//套系技能列表



    [NonSerialized]
    private List<HeroRefObj> _m_lHeroRefList;//伙伴列表
    [NotNull]
    public List<HeroRefObj> heroRefList
    {
        get
        {
            if (_m_lHeroRefList == null)
                _m_lHeroRefList = new List<HeroRefObj>();
            return _m_lHeroRefList;
        }
    }
    public void addHeroRef(HeroRefObj _refObj)
    {
        if (heroRefList.Contains(_refObj))
            return;

        heroRefList.Add(_refObj);
    }
}

public class GSOHeroSuitRefSet : _TALSOBasicRefSet<HeroSuitRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_halo_suit"; } }
}
