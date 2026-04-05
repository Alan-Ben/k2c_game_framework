package NPCommon.CommonObj.ShowItemCollector.Sub;

import Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType;
import Common.BagItemUseObj.BagItemUse_ConsortShowInfo;
import NPCommon.CommonObj.ShowItemCollector._AShowItemCollector;

public class ConsortShowItemCollector extends _AShowItemCollector<EBagItemUse_ConsortDrawShowType, BagItemUse_ConsortShowInfo>
{
    public ConsortShowItemCollector()
    {
        super(EBagItemUse_ConsortDrawShowType.class, false);
    }

    @Override
    public BagItemUse_ConsortShowInfo makeSub(long _instanceId, EBagItemUse_ConsortDrawShowType _type, int _num)
    {
        return new BagItemUse_ConsortShowInfo(_instanceId, _type, _num);
    }
}
