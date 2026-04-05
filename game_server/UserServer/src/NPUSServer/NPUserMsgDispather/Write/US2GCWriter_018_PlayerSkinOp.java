package NPUSServer.NPUserMsgDispather.Write;

import GS2GC.p018_PlayerSkinOp.*;
import NPCommon.PlayerInfo_CurTitle;
import NPUSServer.NPUSUserMgr.UserComp.PlayerSkinComp.PlayerSkinInfo;
import NPUSServer.NPUSUserMgr.UserComp.TitleComp.ComboTitleUnitBgInfo;
import NPUSServer.NPUSUserMgr.UserComp.TitleComp.ComboTitleUnitPreInfo;
import NPUSServer.NPUSUserMgr.UserComp.TitleComp.ComboTitleUnitSfxInfo;
import NPUSServer.NPUSUserMgr.UserComp.TitleComp.PlayerTitleInfo;

public class US2GCWriter_018_PlayerSkinOp
{
    public static GS2GC_018_001_RetSetCommTitle make_001_RetSetCommTitle()
    {
    	GS2GC_018_001_RetSetCommTitle proto = new GS2GC_018_001_RetSetCommTitle();
        
        return proto;
    }
    
    public static GS2GC_018_002_RetSetComboTitle make_002_RetSetComboTitle()
    {
    	GS2GC_018_002_RetSetComboTitle proto = new GS2GC_018_002_RetSetComboTitle();
        
        return proto;
    }
    
    public static GS2GC_018_003_RetSetTitleShow make_003_RetSetTitleShow()
    {
    	GS2GC_018_003_RetSetTitleShow proto = new GS2GC_018_003_RetSetTitleShow();
        
        return proto;
    }
    
    public static GS2GC_018_004_RetUnlockComboTitlePre make_004_RetUnlockComboTitlePre()
    {
    	GS2GC_018_004_RetUnlockComboTitlePre proto = new GS2GC_018_004_RetUnlockComboTitlePre();
        
        return proto;
    }
    
    public static GS2GC_018_005_RetUnlockComboTitleSfx make_005_RetUnlockComboTitleSfx()
    {
    	GS2GC_018_005_RetUnlockComboTitleSfx proto = new GS2GC_018_005_RetUnlockComboTitleSfx();
        
        return proto;
    }
    
    public static GS2GC_018_006_RetUnlockComboTitleBg make_006_RetUnlockComboTitleBg()
    {
    	GS2GC_018_006_RetUnlockComboTitleBg proto = new GS2GC_018_006_RetUnlockComboTitleBg();
        
        return proto;
    }
    
    public static GS2GC_018_007_RetviewComboTitlePre make_007_RetviewComboTitlePre()
    {
    	GS2GC_018_007_RetviewComboTitlePre proto = new GS2GC_018_007_RetviewComboTitlePre();
        
        return proto;
    }
    
    public static GS2GC_018_008_RetViewComboTitleSfx make_008_RetViewComboTitleSfx()
    {
    	GS2GC_018_008_RetViewComboTitleSfx proto = new GS2GC_018_008_RetViewComboTitleSfx();
        
        return proto;
    }
    
    public static GS2GC_018_009_RetViewComboTitleBg make_009_RetViewComboTitleBg()
    {
    	GS2GC_018_009_RetViewComboTitleBg proto = new GS2GC_018_009_RetViewComboTitleBg();
        
        return proto;
    }
    
    public static GS2GC_018_010_RetUnlockPlayerSkin make_010_RetUnlockPlayerSkin()
    {
    	GS2GC_018_010_RetUnlockPlayerSkin proto = new GS2GC_018_010_RetUnlockPlayerSkin();
        
        return proto;
    }
    
    public static GS2GC_018_011_RetSetCurPlayerSkin make_011_RetSetCurPlayerSkin()
    {
    	GS2GC_018_011_RetSetCurPlayerSkin proto = new GS2GC_018_011_RetSetCurPlayerSkin();
        
        return proto;
    }
    
    public static GS2GC_018_012_RetUnsetCurPlayerSkin make_012_RetUnsetCurPlayerSkin()
    {
    	GS2GC_018_012_RetUnsetCurPlayerSkin proto = new GS2GC_018_012_RetUnsetCurPlayerSkin();
        
        return proto;
    }
    
    public static GS2GC_018_013_RetUpgradePlayerSkin make_013_RetUpgradePlayerSkin()
    {
    	GS2GC_018_013_RetUpgradePlayerSkin proto = new GS2GC_018_013_RetUpgradePlayerSkin();
        
        return proto;
    }
    
    public static GS2GC_018_015_RetviewTitle make_015_RetviewTitle()
    {
    	GS2GC_018_015_RetviewTitle proto = new GS2GC_018_015_RetviewTitle();
        
        return proto;
    }

    public static GS2GC_018_050_OnCommTitleChg make_050_OnCommTitleChg(PlayerTitleInfo _title)
    {
    	GS2GC_018_050_OnCommTitleChg proto = new GS2GC_018_050_OnCommTitleChg();
    	proto.setInfo(_title.makeProto());
        
        return proto;
    }

    public static GS2GC_018_051_OnComboTitlePreChg make_051_OnComboTitlePreChg(ComboTitleUnitPreInfo _unit)
    {
    	GS2GC_018_051_OnComboTitlePreChg proto = new GS2GC_018_051_OnComboTitlePreChg();
    	proto.setPre(_unit.toProto());
        
        return proto;
    }

    public static GS2GC_018_052_OnComboTitleSfxChg make_052_OnComboTitleSfxChg(ComboTitleUnitSfxInfo _unit)
    {
    	GS2GC_018_052_OnComboTitleSfxChg proto = new GS2GC_018_052_OnComboTitleSfxChg();
    	proto.setSfx(_unit.toProto());
        
        return proto;
    }

    public static GS2GC_018_053_OnComboTitleBgChg make_053_OnComboTitleBgChg(ComboTitleUnitBgInfo _unit)
    {
    	GS2GC_018_053_OnComboTitleBgChg proto = new GS2GC_018_053_OnComboTitleBgChg();
    	proto.setBg(_unit.toProto());
        
        return proto;
    }

    public static GS2GC_018_054_OnCurTitleChg make_054_OnCurTitleChg(PlayerInfo_CurTitle _info)
    {
    	GS2GC_018_054_OnCurTitleChg proto = new GS2GC_018_054_OnCurTitleChg();
    	proto.setCurInfo(_info);
        
        return proto;
    }

    public static GS2GC_018_060_OnPlayerSkinChg make_060_OnPlayerSkinChg(PlayerSkinInfo _info)
    {
    	GS2GC_018_060_OnPlayerSkinChg proto = new GS2GC_018_060_OnPlayerSkinChg();
    	proto.setInfo(_info.toProto());
        
        return proto;
    }
}
