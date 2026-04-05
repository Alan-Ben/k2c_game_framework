package NPUSServer.NPUserMsgDispather.Write;

import Common.MuseumObj.Museum_ItemInfo;
import GS2GC.p035_MuseumOp.GS2GC_035_001_RetMuseumItemUpgrade;
import GS2GC.p035_MuseumOp.GS2GC_035_002_RetMuseumItemActive;
import GS2GC.p035_MuseumOp.GS2GC_035_050_OnMuseumItemAdd;
import GS2GC.p035_MuseumOp.GS2GC_035_051_OnMuseumItemChg;

/**
 * p035 博物馆操作协议 writer
 */
public class US2GCWriter_035_MuseumOp
{
    public static GS2GC_035_001_RetMuseumItemUpgrade make_001_RetMuseumItemUpgrade()
    {
        return new GS2GC_035_001_RetMuseumItemUpgrade();
    }

    public static GS2GC_035_002_RetMuseumItemActive make_002_RetMuseumItemActive()
    {
        return new GS2GC_035_002_RetMuseumItemActive();
    }

    public static GS2GC_035_050_OnMuseumItemAdd make_050_OnMuseumItemAdd(Museum_ItemInfo itemInfo)
    {
        return new GS2GC_035_050_OnMuseumItemAdd(itemInfo);
    }


    public static GS2GC_035_051_OnMuseumItemChg make_051_OnMuseumItemChg(Museum_ItemInfo itemInfo)
    {
        return new GS2GC_035_051_OnMuseumItemChg(itemInfo);
    }

}