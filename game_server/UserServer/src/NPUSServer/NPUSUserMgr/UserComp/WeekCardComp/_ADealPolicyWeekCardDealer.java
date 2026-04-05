package NPUSServer.NPUSUserMgr.UserComp.WeekCardComp;

import Common.WeekCardObj.WeekCard_ExtraInfo_DealPolicy;

import java.nio.ByteBuffer;

public abstract class _ADealPolicyWeekCardDealer extends _AWeekCardDealer
{
    private WeekCard_ExtraInfo_DealPolicy _m_dealPolicy = new WeekCard_ExtraInfo_DealPolicy();

    public _ADealPolicyWeekCardDealer(WeekCardComponent _comp)
    {
        super(_comp);
    }

    @Override
    protected boolean _checkSettingParamLegal(byte[] _extraInfo)
    {
        WeekCard_ExtraInfo_DealPolicy extraData = new WeekCard_ExtraInfo_DealPolicy();
        try
        {
            extraData.readPackage(ByteBuffer.wrap(_extraInfo));
        } catch (Exception e)
        {
            return false;
        }
        return true;
    }

    @Override
    protected void _onSettingChg(byte[] _extraInfo)
    {
        //解析额外数据
        if (_extraInfo != null && _extraInfo.length > 0)
        {
            WeekCard_ExtraInfo_DealPolicy extraData = new WeekCard_ExtraInfo_DealPolicy();
            extraData.readPackage(ByteBuffer.wrap(_extraInfo));

            _m_dealPolicy = extraData;
        }
    }

    /**
     * 是否懒汉 即接近上限再处理
     * @return true:懒汉
     */
    public boolean isLazy()
    {
        WeekCard_ExtraInfo_DealPolicy dealPolicy = _m_dealPolicy;

        if (dealPolicy == null)
            return false;

        return dealPolicy.getIsLazy();
    }
}
