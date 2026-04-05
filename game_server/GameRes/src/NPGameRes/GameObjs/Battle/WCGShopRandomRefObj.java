package NPGameRes.GameObjs.Battle;

import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;

public class WCGShopRandomRefObj extends _IALBasicRefObj
{
    @Override
    public long _refId()
    {
        return id;
    }

    public long id; // 随机组ID
    public List<Long> storeIdList; // 随机商品库ID列表
    public List<Integer> storeWeightList; // 随机商品库权重列表

    public WCGShopRandomRefObj(long _id)
    {
        id = _id;
        storeIdList = new ArrayList<>();
        storeWeightList = new ArrayList<>();
    }

    public void addStoreId(long _storeId)
    {
        storeIdList.add(_storeId);
    }

    public void addStoreWeight(int _weight)
    {
        storeWeightList.add(_weight);
    }

    public List<Long> getShopStoreList(int count)
    {
        List<Integer> indexList = CommonFunc.getRandomIndexByRate(storeWeightList, count);
        // 处理结果
        List<Long> result = new ArrayList<>();
        for (Integer index : indexList)
        {
            result.add(storeIdList.get(index));
        }
        return result;
    }
}
