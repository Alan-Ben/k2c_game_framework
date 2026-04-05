using GOE;
namespace Common
{
/****
 商店错误
 ****/
    class ShopErr
    {
        static ShopErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(320001, "商品不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(320002, "商品购买失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(320003, "商店手动刷新次数限制"));
        }
    }
}
