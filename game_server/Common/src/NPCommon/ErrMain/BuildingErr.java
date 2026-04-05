package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 建筑系统错误
 ****/
 
public class BuildingErr implements _IErrHolder
{
    public static final Result BUILDING_NOT_EXIST = Result.constInit(80002,"建筑不存在");
    public static final Result BUILDING_EXISTED = Result.constInit(80003,"建筑已经存在");
    public static final Result BUILDING_BUILD_FAIL = Result.constInit(80004,"建筑创建失败");
    public static final Result BUILDING_NO_TYPE = Result.constInit(80005,"建筑类型错误");
    public static final Result BUILDING_LVL_FULL = Result.constInit(80006,"建筑满级");
    public static final Result BUSINESS_EMPLOYEE_FULL = Result.constInit(80007,"经营建筑雇佣人数上限");
    public static final Result BUSINESS_HERO_FULL = Result.constInit(80008,"经营建筑委派伙伴数量上限");
    public static final Result BUSINESS_PRODUCT_ALREADY_UNLOCK = Result.constInit(80009,"经营建筑产品已经解锁");
    public static final Result FARM_CLICK_GAP_LESS = Result.constInit(80010,"农田建筑点击间隔时间不足");
    public static final Result BUSINESS_PRODUCT_UNLOCK_REQUIRE_NOT_REACH = Result.constInit(80011,"经营建筑产品解锁条件未达成");
}
