package NPUSServer.NPUSUserMgr.UserComp.HeroComp;

import CommonEnum.EBasicAttrType;
import CommonEnum.EBonusPropertyType;
import NPGameRes.Refs.Consort.RefConsort;
import NPGameRes.Refs.Hero.RefHeroLevel;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;

/**
 * 大臣属性计算器
 */
public class HeroCalculator
{

    /************
     * 计算大臣的资质值
     */
    public static long calTalent(HeroInfo _heroInfo, StringBuilder _sb)
    {
        if (null == _heroInfo)
            return 0;

        //计算资质
        long talentPoint = _heroInfo.getPropertyContainer().getValue(EBasicAttrType.TALENT);
        if (_sb != null)
            _sb.append("大臣资质值: ").append(talentPoint).append("\n");

        //获取玩家身上加成
        long playerTalentAddition = _heroInfo.getPlayerBonusAddValue(EBonusPropertyType.TALENT);
        if (_sb != null)
            _sb.append("玩家bonus资质值: ").append(playerTalentAddition).append("\n");

        //获取套系加成
        long suitTalentAddition = 0;
        if (_heroInfo.getSuitInfo() != null)
            suitTalentAddition = _heroInfo.getSuitInfo().getAttrContainer().getValue(EBasicAttrType.TALENT);
        if (_sb != null)
            _sb.append("套系资质值: ").append(suitTalentAddition).append("\n");

        //获取装备加成
        long equipTalentAddition = _heroInfo.getEquipTalent();
        if (_sb != null)
            _sb.append("藏品资质值: ").append(equipTalentAddition).append("\n");

        //返回累加值
        long talent = talentPoint + playerTalentAddition + suitTalentAddition + equipTalentAddition;
        if (_sb != null)
            _sb.append("总资质值: ").append(talent).append("\n");
        return talent;
    }

    /************
     * 计算大臣的实力值
     */
    public static long calPower(HeroInfo _heroInfo, StringBuilder _sb)
    {
        if (null == _heroInfo)
            return 0;
        if (_sb != null)
            _sb.append("大臣id: ").append(_heroInfo.getRef().id).append("\n");

        //1.计算基础实力
        RefHeroLevel heroLevelRef = _heroInfo.getHeroLevelRef();
        long basicPower = calTalent(_heroInfo, _sb) * (heroLevelRef == null ? 0 : heroLevelRef.level_ratio);
        if (_sb != null)
            _sb.append("实力基础值: ").append(basicPower).append("\n");

        //2.计算万分比加成
        //获取大臣自身加成
        long heroPowerPerAddition = _heroInfo.getPropertyContainer().getValue(EBasicAttrType.POWER_PER);
        if (_sb != null)
            _sb.append("大臣自身万分比加成: ").append(heroPowerPerAddition).append("\n");
        //获取套系加成
        long suitPowerPerAddition = 0;
        if (_heroInfo.getSuitInfo() != null)
            suitPowerPerAddition = _heroInfo.getSuitInfo().getAttrContainer().getValue(EBasicAttrType.POWER_PER);
        if (_sb != null)
            _sb.append("套系万分比加成: ").append(suitPowerPerAddition).append("\n");
        //获取玩家身上加成
        long playerPowerPerAddition = _heroInfo.getPlayerBonusAddValue(EBonusPropertyType.POWER_PER);
        if (_sb != null)
            _sb.append("玩家bonus万分比加成:").append(playerPowerPerAddition).append("\n");
        //获取对应家人的加成
        long consortPowerPerAddition = calConsortAdd(_heroInfo, EBasicAttrType.POWER_PER);
        if (_sb != null)
            _sb.append("家人万分比加成: ").append(consortPowerPerAddition).append("\n");
        //获取装备加成
        long equipPowerPerAddition = _heroInfo.getEquipPowerPer();
        if (_sb != null)
            _sb.append("藏品万分比加成: ").append(equipPowerPerAddition).append("\n");
        //万分比加成累加值
        long powerPerAddition = heroPowerPerAddition + suitPowerPerAddition + playerPowerPerAddition + consortPowerPerAddition + equipPowerPerAddition;
        if (_sb != null)
            _sb.append("万分比加成累加值: ").append(powerPerAddition).append("\n");

        //3.获取绝对值加成
        //获取大臣自身加成
        long heroPowerAddition = _heroInfo.getPropertyContainer().getValue(EBasicAttrType.POWER);
        if (_sb != null)
            _sb.append("大臣自身绝对值加成: ").append(heroPowerAddition).append("\n");
        //获取套系加成
        long suitPowerAddition = 0;
        if (_heroInfo.getSuitInfo() != null)
            suitPowerAddition = _heroInfo.getSuitInfo().getAttrContainer().getValue(EBasicAttrType.POWER);
        if (_sb != null)
            _sb.append("套系绝对值加成: ").append(suitPowerAddition).append("\n");
        //获取玩家身上加成
        long playerPowerAddition = _heroInfo.getPlayerBonusAddValue(EBonusPropertyType.POWER);
        if (_sb != null)
            _sb.append("玩家bonus绝对值加成: ").append(playerPowerAddition).append("\n");
        //获取对应家人的加成
        long consortPowerAddition = calConsortAdd(_heroInfo, EBasicAttrType.POWER);
        if (_sb != null)
            _sb.append("家人绝对值加成: ").append(consortPowerAddition).append("\n");
        //获取道具额外加成
        long itemPowerAddition = _heroInfo.getItemAddPower();
        if (_sb != null)
            _sb.append("道具绝对值加成: ").append(itemPowerAddition).append("\n");
        //获取竞技场额外加成
        long arenaPowerAddition = _heroInfo.getArenaAddPower();
        if (_sb != null)
            _sb.append("竞技场绝对值加成: ").append(arenaPowerAddition).append("\n");
        //获取游历额外加成
        long travelPowerAddition = _heroInfo.getTravelAddPower();
        if (_sb != null)
            _sb.append("游历绝对值加成: ").append(travelPowerAddition).append("\n");
        //绝对值加成累加值
        long powerAddition = heroPowerAddition + suitPowerAddition + playerPowerAddition + consortPowerAddition + itemPowerAddition + arenaPowerAddition + travelPowerAddition;
        if (_sb != null)
            _sb.append("绝对值加成累加值: ").append(powerAddition).append("\n");

        //4.返回累加值
        long totalPower = (long) Math.ceil((double) basicPower * (10000 + powerPerAddition) / 10000 + powerAddition);
        if (_sb != null)
            _sb.append("总实力值: ").append(totalPower).append("\n");
        return totalPower;
    }

    /**
     * 计算家人带来的加成
     * @param _heroInfo
     * @param _attr
     * @return
     */
    public static long calConsortAdd(HeroInfo _heroInfo, EBasicAttrType _attr)
    {
        if (null == _heroInfo || null == _heroInfo.getRef())
            return 0;

        long value = 0;
        for (int i = 0; i < _heroInfo.getRef().getRelationConsortList().size(); i++)
        {
            RefConsort consortRef = _heroInfo.getRef().getRelationConsortList().get(i);
            if (null == consortRef)
                continue;

            ConsortInfo consort = _heroInfo.getUserdata().getConsortComponent().lookup(consortRef.id);
            if (null == consort)
                continue;

            value += consort.getPropertyContainer().getValue(_attr);
        }

        return value;
    }
}
