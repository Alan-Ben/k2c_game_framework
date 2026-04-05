package NPUSServer.GMCommand.Cmds;

import CommonEnum.EBonusFilterType;
import CommonEnum.EBonusPropertyType;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPGameRes.Refs.Building.RefBuilding;
import NPGameRes.Refs.Consort.RefConsort;
import NPGameRes.Refs.Hero.RefHero;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "bonus命令", name = "bonus")
public class CmdBonus extends UsCmdBase
{
    @ACommand(comment = "获取加成值[加成属性类型][加成过滤类型][过滤id]")
    public String getValue(EBonusPropertyType _propertyType, EBonusFilterType _filterType, long _id)
    {
        return String.valueOf(getOwner().getBonusMgr().getTotalAndFilterPropertyBonus(_propertyType, _filterType, _id));
    }

    /**
     * 打印玩家加成信息
     * @return
     */
    @ACommand(comment = "打印加成信息[加成过滤类型][过滤id]")
    public String printBonus(EBonusFilterType _filterType, long _id)
    {
        long[] bonusArray = getOwner().getBonusMgr().getBonusListByFilter(_filterType, _id);

        return _toString(bonusArray);
    }

    /**
     * 打印指定大臣加成信息
     * @param _heroId 大臣ID
     * @return
     */
    @ACommand(comment = "打印指定大臣加成信息[大臣ID]")
    public String hero(long _heroId)
    {
        // 获取大臣配置
        RefHero heroRef = RefHero.getMgr().get(_heroId);
        if (heroRef == null)
        {
            return "未找到大臣ID[" + _heroId + "]的配置信息";
        }

        StringBuilder sb = new StringBuilder();
        sb.append("大臣ID: ").append(_heroId).append("\n");
        sb.append("特长属性: ").append(heroRef.spec_attr_type).append("\n");
        sb.append("品质: ").append(heroRef.quality).append("\n");
        sb.append("==========================================\n");

        // 1. NONE类型（全局加成）
        long[] noneBonus = getOwner().getBonusMgr().getBonusListByFilter(EBonusFilterType.NONE, 0);
        sb.append("【全局加成 NONE】\n");
        sb.append(_toStringWithoutTitle(noneBonus));
        sb.append("==========================================\n");

        // 2. HERO_ATTR类型（特长伙伴加成）
        long[] heroAttrBonus = getOwner().getBonusMgr().getBonusListByFilter(EBonusFilterType.HERO_ATTR, heroRef.spec_attr_type.ordinal());
        sb.append("【特长加成 HERO_ATTR: ").append(heroRef.spec_attr_type).append("】\n");
        sb.append(_toStringWithoutTitle(heroAttrBonus));
        sb.append("==========================================\n");

        // 3. HERO_ID类型（指定伙伴加成）
        long[] heroIdBonus = getOwner().getBonusMgr().getBonusListByFilter(EBonusFilterType.HERO_ID, _heroId);
        sb.append("【指定大臣加成 HERO_ID: ").append(_heroId).append("】\n");
        sb.append(_toStringWithoutTitle(heroIdBonus));
        sb.append("==========================================\n");

        // 4. QUALITY类型（品质加成）
        long[] qualityBonus = getOwner().getBonusMgr().getBonusListByFilter(EBonusFilterType.QUALITY, heroRef.quality.ordinal());
        sb.append("【品质加成 QUALITY: ").append(heroRef.quality).append("】\n");
        sb.append(_toStringWithoutTitle(qualityBonus));
        sb.append("==========================================\n");

        // 5. 汇总所有加成
        long[] totalBonus = new long[EBonusPropertyType.EBonusPropertyType_Length];
        for (int i = 0; i < totalBonus.length; i++)
        {
            totalBonus[i] = noneBonus[i] + heroAttrBonus[i] + heroIdBonus[i] + qualityBonus[i];
        }
        sb.append("【总加成汇总】\n");
        sb.append(_toStringWithoutTitle(totalBonus));

        return sb.toString();
    }

    /**
     * 打印指定家人加成信息
     * @param _consortId 家人ID
     * @return
     */
    @ACommand(comment = "打印指定家人加成信息[家人ID]")
    public String consort(long _consortId)
    {
        // 获取家人配置
        RefConsort consortRef = RefConsort.getMgr().get(_consortId);
        if (consortRef == null)
        {
            return "未找到家人ID[" + _consortId + "]的配置信息";
        }

        StringBuilder sb = new StringBuilder();
        sb.append("家人ID: ").append(_consortId).append("\n");
        sb.append("==========================================\n");

        // 1. NONE类型（全局加成）
        long[] noneBonus = getOwner().getBonusMgr().getBonusListByFilter(EBonusFilterType.NONE, 0);
        sb.append("【全局加成 NONE】\n");
        sb.append(_toStringWithoutTitle(noneBonus));
        sb.append("==========================================\n");

        // 2. CONSORT_ID类型（指定家人加成）
        long[] consortIdBonus = getOwner().getBonusMgr().getBonusListByFilter(EBonusFilterType.CONSORT_ID, _consortId);
        sb.append("【指定家人加成 CONSORT_ID: ").append(_consortId).append("】\n");
        sb.append(_toStringWithoutTitle(consortIdBonus));
        sb.append("==========================================\n");

        // 3. 汇总所有加成
        long[] totalBonus = new long[EBonusPropertyType.EBonusPropertyType_Length];
        for (int i = 0; i < totalBonus.length; i++)
        {
            totalBonus[i] = noneBonus[i] + consortIdBonus[i];
        }
        sb.append("【总加成汇总】\n");
        sb.append(_toStringWithoutTitle(totalBonus));

        return sb.toString();
    }

    /**
     * 打印指定建筑加成信息
     * @param _buildingId 建筑ID
     * @return
     */
    @ACommand(comment = "打印指定建筑加成信息[建筑ID]")
    public String building(long _buildingId)
    {
        // 获取建筑配置
        RefBuilding buildingRef = RefBuilding.getMgr().get(_buildingId);
        if (buildingRef == null)
        {
            return "未找到建筑ID[" + _buildingId + "]的配置信息";
        }

        // 注意：建筑的attr需要通过实际建筑实例获取，这里只能显示基础的加成信息
        StringBuilder sb = new StringBuilder();
        sb.append("建筑ID: ").append(_buildingId).append("\n");
        sb.append("注意：建筑特长属性需要通过实际建筑实例获取，此处仅显示基础加成\n");
        sb.append("==========================================\n");

        // 1. NONE类型（全局加成）
        long[] noneBonus = getOwner().getBonusMgr().getBonusListByFilter(EBonusFilterType.NONE, 0);
        sb.append("【全局加成 NONE】\n");
        sb.append(_toStringWithoutTitle(noneBonus));
        sb.append("==========================================\n");

        // 2. BUILDING_ID类型（指定建筑加成）
        long[] buildingIdBonus = getOwner().getBonusMgr().getBonusListByFilter(EBonusFilterType.BUILDING_ID, _buildingId);
        sb.append("【指定建筑加成 BUILDING_ID: ").append(_buildingId).append("】\n");
        sb.append(_toStringWithoutTitle(buildingIdBonus));
        sb.append("==========================================\n");

        // 注意：BUILDING_ATTR需要具体的建筑实例才能获取，这里无法显示

        // 3. 汇总所有加成（不包含BUILDING_ATTR）
        long[] totalBonus = new long[EBonusPropertyType.EBonusPropertyType_Length];
        for (int i = 0; i < totalBonus.length; i++)
        {
            totalBonus[i] = noneBonus[i] + buildingIdBonus[i];
        }
        sb.append("【总加成汇总（不含特长属性加成）】\n");
        sb.append(_toStringWithoutTitle(totalBonus));

        return sb.toString();
    }

    /**
     * 格式化输出加成信息
     * @param bonusArray 加成数组
     * @param withTitle 是否包含标题
     * @return 格式化后的字符串
     */
    private static String _toString(long[] bonusArray, boolean withTitle)
    {
        StringBuilder sb = new StringBuilder();
        String indent = withTitle ? "" : "  ";
        int startLength = sb.length();

        // 添加标题（如果需要）
        if (withTitle)
        {
            sb.append("加成列表:\n");
            startLength = sb.length();
        }

        // 遍历所有属性类型，显示非零的加成值
        boolean hasBonus = false;
        for (int i = 0; i < bonusArray.length; i++)
        {
            if (bonusArray[i] != 0)
            {
                EBonusPropertyType propertyType = EBonusPropertyType.EBonusPropertyType_FromInt(i);
                if (propertyType != null)
                {
                    sb.append(indent).append(propertyType).append(": ").append(bonusArray[i]).append("\n");
                    hasBonus = true;
                }
            }
        }

        // 如果没有任何加成，显示提示信息
        if (!hasBonus)
        {
            sb.append(indent).append("无加成数据");
            if (!withTitle)
            {
                sb.append("\n");
            }
        }

        return sb.toString();
    }

    /**
     * 格式化输出加成信息（带标题）
     * @return
     */
    private static String _toString(long[] bonusArray)
    {
        return _toString(bonusArray, true);
    }

    /**
     * 格式化输出加成信息（不带标题）
     * @return
     */
    private static String _toStringWithoutTitle(long[] bonusArray)
    {
        return _toString(bonusArray, false);
    }


}
