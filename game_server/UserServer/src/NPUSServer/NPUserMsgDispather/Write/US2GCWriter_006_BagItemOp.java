package NPUSServer.NPUserMsgDispather.Write;

import Common.BagItemUseObj.BagItemUse_ConsortShowInfo;
import Common.BagItemUseObj.BagItemUse_HeroShowInfo;
import GS2GC.p006_BagItemOp.*;
import NPUSServer.NPUSUserMgr.UserComp.BagItemComp.BagItemInfo;

import java.util.ArrayList;
import java.util.List;

/**
 * 006 背包道具 writer
 */
public class US2GCWriter_006_BagItemOp
{
	public static GS2GC_006_001_RetSellBagItem make_001_RetSellBagItemSucc()
    {
    	GS2GC_006_001_RetSellBagItem proto = new GS2GC_006_001_RetSellBagItem();
        return proto;
    }

    public static GS2GC_006_002_RetBagUseItem make_002_RetBagUseItemSucc(long _itemId, long _count, NPCommon.CommonObj.NPItemCollector _collector)
    {
    	GS2GC_006_002_RetBagUseItem proto = new GS2GC_006_002_RetBagUseItem();
        proto.setItemId(_itemId);
        proto.setCount(_count);
        _collector.fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_006_007_RetConvert make_007_RetConvert()
    {
        GS2GC_006_007_RetConvert proto = new GS2GC_006_007_RetConvert();

        return proto;
    }

    public static GS2GC_006_008_RetAKeyConvert make_008_RetAKeyConvert()
    {
        GS2GC_006_008_RetAKeyConvert proto = new GS2GC_006_008_RetAKeyConvert();

        return proto;
    }
    
    public static GS2GC_006_009_RetBagUseItemForSelectHero make_009_RetBagUseItemForSelectHero(List<BagItemUse_HeroShowInfo> _showList)
    {
        return new GS2GC_006_009_RetBagUseItemForSelectHero((ArrayList<BagItemUse_HeroShowInfo>) _showList);
    }

    public static GS2GC_006_010_RetBagUseItemForSelectConsort make_010_RetBagUseItemForSelectConsort(List<BagItemUse_ConsortShowInfo> _showList)
    {
        return new GS2GC_006_010_RetBagUseItemForSelectConsort((ArrayList<BagItemUse_ConsortShowInfo>) _showList);
    }

    public static GS2GC_006_050_PushBagItemInfo make_050_PushBagItemInfo(BagItemInfo _info)
    {
        GS2GC_006_050_PushBagItemInfo proto = new GS2GC_006_050_PushBagItemInfo();
        proto.setBagItemInfo(_info.toProto());
        return proto;
    }

    public static GS2GC_006_051_PushRemoveBagItem make_051_PushRemoveBagItem(long _itemId)
    {
        GS2GC_006_051_PushRemoveBagItem proto = new GS2GC_006_051_PushRemoveBagItem();
        proto.setItemId(_itemId);
        return proto;
    }
}
