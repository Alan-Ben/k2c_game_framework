package NPUSServer.NPUserMsgDispather.Write;

import Common.HeroObj.Equip_BaseInfo;
import Common.HeroObj.Equip_Info;
import Common.HeroObj.Equip_SkillInfo;
import GS2GC.p013_HeroOp.*;
import NPUSServer.NPUSUserMgr.UserComp.EquipComp.EquipInfo;

import java.util.List;

/**
 * 013 骑士协议 writer
 */
public class US2GCWriter_013_HeroOp
{
	public static GS2GC_013_001_RetHeroUpgrade make_001_RetHeroUpgrade()
    {
        return new GS2GC_013_001_RetHeroUpgrade();
    }

    public static GS2GC_013_002_RetHeroSetSkin make_002_RetHeroSetSkin()
    {
        return new GS2GC_013_002_RetHeroSetSkin();
    }

	public static GS2GC_013_004_RetHeroStepUpgrade make_004_RetHeroStepUpgrade()
    {
        return new GS2GC_013_004_RetHeroStepUpgrade();
    }
	
    public static GS2GC_013_005_RetHeroSkinUpgrade make_005_RetHeroSkinUpgrade()
    {
        return new GS2GC_013_005_RetHeroSkinUpgrade();
    }

    public static GS2GC_013_006_RetHeroStarUpgrade make_006_RetHeroStarUpgrade()
    {
        return new GS2GC_013_006_RetHeroStarUpgrade();
    }

    public static GS2GC_013_007_RetHeroTalentSkillUpgrade make_007_RetHeroTalentSkillUpgrade()
    {
        return new GS2GC_013_007_RetHeroTalentSkillUpgrade();
    }

    public static GS2GC_013_008_RetHeroBusinessSkillUpgrade make_008_RetHeroBusinessSkillUpgrade()
    {
        return new GS2GC_013_008_RetHeroBusinessSkillUpgrade();
    }

    public static GS2GC_013_009_RetHeroSkinUnlock make_009_RetHeroSkinUnlock()
    {
        return new GS2GC_013_009_RetHeroSkinUnlock();
    }

    public static GS2GC_013_010_RetBuildingPlaceHero make_010_RetBuildingPlaceHero()
    {
        return new GS2GC_013_010_RetBuildingPlaceHero();
    }

    public static GS2GC_013_011_RetBuildingRemoveHero make_011_RetBuildingRemoveHero()
    {
        return new GS2GC_013_011_RetBuildingRemoveHero();
    }

    public static GS2GC_013_015_RetHeroHaloUnlock make_015_RetHeroHaloUnlock()
    {
        return new GS2GC_013_015_RetHeroHaloUnlock();
    }

    public static GS2GC_013_016_RetHeroHaloUpgrade make_016_RetHeroHaloUpgrade()
    {
        return new GS2GC_013_016_RetHeroHaloUpgrade();
    }

    public static GS2GC_013_021_RetEquipUpgrade make_021_RetEquipUpgrade()
    {
        return new GS2GC_013_021_RetEquipUpgrade();
    }

    public static GS2GC_013_022_RetEquipSkillRebuild make_022_RetEquipSkillRebuild()
    {
        return new GS2GC_013_022_RetEquipSkillRebuild();
    }

    public static GS2GC_013_023_RetEquipDisassemble make_023_RetEquipDisassemble()
    {
        return new GS2GC_013_023_RetEquipDisassemble();
    }

    public static GS2GC_013_024_RetEquipAwaken make_024_RetEquipAwaken()
    {
        return new GS2GC_013_024_RetEquipAwaken();
    }

    public static GS2GC_013_027_RetEquipSkillList make_027_RetEquipSkillList(EquipInfo _equipInfo)
    {
        GS2GC_013_027_RetEquipSkillList proto = new GS2GC_013_027_RetEquipSkillList();
        _equipInfo.makeSkillList(proto.getSkillList());
        return proto;
    }

    public static GS2GC_013_028_RetHeroWearEquip make_028_RetHeroWearEquip()
    {
        return new GS2GC_013_028_RetHeroWearEquip();
    }

    public static GS2GC_013_029_RetHeroUnWearEquip make_029_RetHeroUnWearEquip()
    {
        return new GS2GC_013_029_RetHeroUnWearEquip();
    }

    public static GS2GC_013_030_RetEquipLockStateChg make_030_RetEquipLockStateChg()
    {
        return new GS2GC_013_030_RetEquipLockStateChg();
    }

    public static GS2GC_013_065_OnEquipAdd make_065_OnEquipAdd(Equip_Info _equipInfo)
    {
        return new GS2GC_013_065_OnEquipAdd(_equipInfo);
    }

    public static GS2GC_013_066_OnEquipBaseChg make_066_OnEquipBaseChg(Equip_BaseInfo _equipInfo)
    {
        return new GS2GC_013_066_OnEquipBaseChg(_equipInfo);
    }

    public static GS2GC_013_067_OnEquipSkillChg make_067_OnEquipSkillChg(long _dbId, Equip_SkillInfo _skillInfo)
    {
        return new GS2GC_013_067_OnEquipSkillChg(_dbId, _skillInfo);
    }

    public static GS2GC_013_068_OnEquipRemove make_068_OnEquipRemove(List<Long> dbIdList)
    {
        GS2GC_013_068_OnEquipRemove proto = new GS2GC_013_068_OnEquipRemove();
        proto.getDbIdList().addAll(dbIdList);
        return proto;
    }

}
