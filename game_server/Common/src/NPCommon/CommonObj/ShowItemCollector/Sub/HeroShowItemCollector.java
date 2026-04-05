package NPCommon.CommonObj.ShowItemCollector.Sub;

import Common.BagItemUseEnum.EBagItemUse_HeroDrawShowType;
import Common.BagItemUseObj.BagItemUse_HeroShowInfo;
import NPCommon.CommonObj.ShowItemCollector._AShowItemCollector;

public class HeroShowItemCollector extends _AShowItemCollector<EBagItemUse_HeroDrawShowType, BagItemUse_HeroShowInfo>
{
    public HeroShowItemCollector()
    {
        super(EBagItemUse_HeroDrawShowType.class, false);
    }

    @Override
    public BagItemUse_HeroShowInfo makeSub(long _instanceId, EBagItemUse_HeroDrawShowType _type, int _num)
    {
        return new BagItemUse_HeroShowInfo(_instanceId, _type, _num);
    }
}
