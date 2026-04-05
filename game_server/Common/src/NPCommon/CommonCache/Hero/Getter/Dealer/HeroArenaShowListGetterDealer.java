package NPCommon.CommonCache.Hero.Getter.Dealer;

import Common.HeroObj.Hero_ArenaShowList;
import NPCommon.CommonCache.GetterBase._ACacheGetter;
import NPCommon.CommonCache.GetterBase._ACacheGetterDealer;
import NPCommon.CommonCache.Hero.Getter._IPlayerHeroCacheGetterEnv;
import NPCommon.CommonCache.Hero._ACachedPlayerHeroList;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import WCGCommon.Enum.NPEnum.EServerType;

public class HeroArenaShowListGetterDealer extends _ACacheGetterDealer<_ACachedPlayerHeroList, _IPlayerHeroCacheGetterEnv, Hero_ArenaShowList>
{
    public HeroArenaShowListGetterDealer(_ACacheGetter<_ACachedPlayerHeroList, _IPlayerHeroCacheGetterEnv> _getter)
    {
        super(_getter);
    }

    @Override
    public void getInfo(long _cid, HandlerTwo<Boolean, Hero_ArenaShowList> _handler)
    {
        //获取目标玩家所在UsId
        int typeId = CommonFunc.parseServerTypeIdFromCid(_cid);
        //如果是本地服务器，直接获取数据
        if (getServer().getServerType() == EServerType.USER.ordinal() && getServer().getServerTypeId() == typeId)
        {
            getEnv().getData(_cid, new HandlerTwo<Boolean, _ACachedPlayerHeroList>()
            {
                @Override
                public void handle(Boolean _suc, _ACachedPlayerHeroList _data)
                {
                    if (_suc && _data != null)
                    {
                        _handler.handle(true, _data.makeArenaShowList());
                    } else
                    {
                        _handler.handle(false, null);
                    }
                }
            });
        } else
        {
            //TODO 从远程服务器获取数据
            _handler.handle(false, null);
        }
    }
}
