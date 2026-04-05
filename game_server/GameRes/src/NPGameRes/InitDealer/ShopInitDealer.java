package NPGameRes.InitDealer;

import NPCommon.Game.WeightValueList;
import NPCommon.Log.CommLog;
import NPCommon.Util.Pair.WCGTripleLong;
import NPGameRes.Refs.Shop.*;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/*******************
 * 初始化空间对象
 * @author Administrator
 *
 */
public class ShopInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        //初始化商店物品折扣随机池
        Map<Long, WeightValueList<RefShopItemDiscount>> weightMap = new HashMap<>();
        for (RefShopItemDiscount refShopItemDiscount : RefShopItemDiscount.getMgr().getList())
        {
            if (refShopItemDiscount == null)
                continue;

            weightMap.computeIfAbsent(refShopItemDiscount.group_id, k -> new WeightValueList<>()).add(refShopItemDiscount, refShopItemDiscount.wei);
        }
        //初始化商店物品折扣
        for (RefShopItem refShopItem : RefShopItem.getMgr().getList())
        {
            if (refShopItem == null)
                continue;

            //该折扣包含在此商品中
            WeightValueList<RefShopItemDiscount> weightList = weightMap.get(refShopItem.discount_group_id);
            if (weightList != null)
            {
                refShopItem.setDiscountWeightList(weightList);
            }else
            {
                refShopItem.setDiscountWeightList(new WeightValueList<>());
            }
        }

        //初始化商店物品组掉落
        for (RefShop refShop : RefShop.getMgr().getList())
        {
            if (refShop == null)
                continue;

            List<ShopItemGroupDrop> list = new ArrayList<>();
            //商品组:商品基础数量:折扣商品基础数量
            for (WCGTripleLong triplePair : refShop.shop_item_group_refresh_list)
            {
                //判断商品组是否存在
                RefShopItemGroup refShopItemGroup = RefShopItemGroup.getMgr().get(triplePair.getFirst());
                if (refShopItemGroup == null)
                {
                    CommLog.error("ShopInitDealer dealInit refShopItemGroup is null groupId:{} ", triplePair.getFirst());
                    continue;
                }

                //初始化商品组掉落对象
                ShopItemGroupDrop dropGroup = new ShopItemGroupDrop(refShopItemGroup, triplePair.getSecond(), triplePair.getThree());
                boolean initSuc = dropGroup.init();
                if (!initSuc)
                    continue;

                list.add(dropGroup);
            }

            refShop.setShopItemGroupDropList(list);
        }
    }
}
