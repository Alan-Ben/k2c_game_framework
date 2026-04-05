package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 商店错误
 ****/
 
public class ShopErr implements _IErrHolder
{
    public static final Result SHOP_ITEM_NOT_EXIST = Result.constInit(320001,"商品不存在");
    public static final Result SHOP_BUY_FAIL = Result.constInit(320002,"商品购买失败");
    public static final Result SHOP_REFRESH_LIMIT = Result.constInit(320003,"商店手动刷新次数限制");
}
