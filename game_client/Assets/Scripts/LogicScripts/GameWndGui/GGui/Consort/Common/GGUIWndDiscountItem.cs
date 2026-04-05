using ALPackage;
using GOE;

/// <summary>
/// 折扣显示item
/// </summary>
public class GGUIWndDiscountItem:_ATALBasicUISubWnd<GGUIMonoDiscountItem>
{
    private NPGGUIWndCommonItem _m_costItemWnd;
    private NPCommonCostItem _m_item;
    private long _m_discount;


    public GGUIWndDiscountItem(GGUIMonoDiscountItem _wnd) : base(_wnd)
    {
        initWnd();
    }

    protected override void _onShowWnd()
    {
    }

    protected override void _onHideWnd()
    {
    }

    protected override void _onReset()
    {
        _m_costItemWnd?.resetWnd();
    }

    protected override void _onDiscard()
    {
        _m_costItemWnd?.discard();
        _m_costItemWnd = null;
    }

    protected override void _onWndInitDone()
    {
        if (null == wnd)
            return;
        if (null != wnd.costItem)
        {
            _m_costItemWnd = new NPGGUIWndCommonItem(wnd.costItem);
        }
    }

    /// <summary>
    /// 设置显示信息
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="_discount"></param>
    public void setInfo(NPCommonCostItem _item, long _discount)
    {
        _m_item = _item;
        _m_discount = _discount;
        _refreshWnd();
    }
    

    /// <summary>
    /// 刷新显示信息
    /// </summary>
    private void _refreshWnd()
    {
        if(null == wnd)
            return;
        
        _m_costItemWnd?.showWnd();
        _m_costItemWnd?.setItem(_m_item);

        long discountItemCount = _m_item.count * _m_discount / 10000;
        ALUGUICommon.setLabelTxt(wnd.txtDiscountItemCount, wnd.showLargeNum ? discountItemCount.ToLargeString(_m_item.getLargeStringType()) : discountItemCount.ToString());
        
        //已有数量，默认取背包数量，有外部传参取外部传参
        long hasNum = GCommon.getItemCount(_m_item.getItemType(), _m_item.subId);
        //已有数量文本
        string strHasNum = wnd.showLargeNum ? hasNum.ToLargeString(_m_item.getLargeStringType()) : hasNum.ToString();
        if(hasNum < discountItemCount)
            strHasNum = GCommon.addColorForRichText(strHasNum, wnd.notEnoughTxtColor);

        bool hasDiscount = _m_discount < 10000;//是否有折扣
        if (hasDiscount)
        {
            ALUGUICommon.setLabelTxt(wnd.txtSourceItemCount, TextTranslate.instance.getLanguage(wnd.hasDiscountShowKey ,
                strHasNum, 
                wnd.showLargeNum ? _m_item.count.ToLargeString(_m_item.getLargeStringType()) : _m_item.count.ToString()));
        }
        else
        {
            ALUGUICommon.setLabelTxt(wnd.txtSourceItemCount,  TextTranslate.instance.getLanguage(wnd.noDiscountShowKey ,
                strHasNum, 
                wnd.showLargeNum ? _m_item.count.ToLargeString(_m_item.getLargeStringType()) : _m_item.count.ToString()));
        }
        ALUGUICommon.setLabelTxt(wnd.txtDiscount, TextTranslate.instance.getLanguage(TransKeyConst.consort_gem_call_discount,100 - (_m_discount / 100)));
        ALUGUICommon.setGameObjEnable(wnd.goDiscountList, hasDiscount);
    }
}