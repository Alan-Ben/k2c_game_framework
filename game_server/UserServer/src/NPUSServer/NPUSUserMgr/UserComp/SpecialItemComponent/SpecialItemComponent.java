package NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent;

import CommonEnum.ESpecialItemType;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.*;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerSpecialItemBO;

import java.util.Arrays;
import java.util.List;
import java.util.Objects;

/**
 * 特殊物品管理器
 */
public class SpecialItemComponent extends _ANPUserComponent
{
    //处理器列表
    private _ASpecialItemDealer[] _m_dealerList;

    public SpecialItemComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.SPECIAL_ITEM);

        //注册组件
        _m_dealerList = new _ASpecialItemDealer[ESpecialItemType.ESpecialItemType_Length];
        regDealer(new SpecialItemDealer_Gold(this));
        regDealer(new SpecialItemDealer_Station(this));
        regDealer(new SpecialItemDealer_FarmMutiple(this));
        regDealer(new SpecialItemDealer_MarsEnergy(this));
        regDealer(new SpecialItemDealer_PaidGem(this));
        regDealer(new SpecialItemDealer_PaidVoucher(this));
    }

    /**
     * 注册管理器
     * @param _dealer
     */
    private void regDealer(_ASpecialItemDealer _dealer)
    {
        _m_dealerList[_dealer.getSpecialItemType().ordinal()] = _dealer;
    }

    /**
     * 获取处理器
     * @param _type
     * @return
     */
    public _ASpecialItemDealer getDealer(int _type)
    {
        return _m_dealerList[_type];
    }

    /**
     * 获取处理器
     * @param _type
     * @return
     */
    public _ASpecialItemDealer getDealer(ESpecialItemType _type)
    {
        return getDealer(_type.ordinal());
    }

    /**
     * 获取处理器
     * @param _type
     * @return
     */
    public <T extends _ASpecialItemDealer> T getDealer(ESpecialItemType _type, Class<T> _clazz)
    {
        _ASpecialItemDealer dealer = getDealer(_type.ordinal());
        if (dealer == null) {
            USLog.error(getUSServer(), "SpecialItemComponent getDealer dealer is null for type={}", _clazz.getSimpleName());
            return null;
        }

        if (!_clazz.isInstance(dealer)) {
            USLog.error(getUSServer(), "SpecialItemComponent getDealer dealer is not instance of class type={} dealer={} e:{}",
                    _clazz.getSimpleName(), dealer.getClass().getSimpleName(), new Exception());
            return null;
        }

        return _clazz.cast(dealer);
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerSpecialItemBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerSpecialItemBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerSpecialItemBO> _boList)
                    {
                        for (PlayerSpecialItemBO bo : _boList)
                        {
                            _ASpecialItemDealer dealer = getDealer(bo.getType());
                            if (dealer == null)
                            {
                                USLog.error(getUSServer(), "SpecialItemComponent _init dealer is null type=" + bo.getType());
                                continue;
                            }

                            dealer.initBo(bo);
                        }

                        setInited();
                    }

                    @Override
                    public void dealFail()
                    {
                        getUserData().setDataLoadFail();
                    }
                });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        Arrays.stream(_m_dealerList).filter(Objects::nonNull).forEach(_ASpecialItemDealer::onInited);
    }

    @Override
    public void dispose()
    {
    	Arrays.stream(_m_dealerList).filter(Objects::nonNull).forEach(_ASpecialItemDealer::dispose);
    }
}
