package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 大臣系统错误
 ****/
 
public class HeroErr implements _IErrHolder
{
    public static final Result HERO_REACH_CURRENT_STEP_LIMIT = Result.constInit(20001,"等级达到当前阶段上限");
    public static final Result HERO_UPGRADE_FAIL = Result.constInit(20002,"骑士升级失败");
    public static final Result HERO_LEVEL_NOT_ENOUGH = Result.constInit(20003,"骑士等级不够");
    public static final Result HERO_STEP_IS_MAX = Result.constInit(20004,"骑士已达到最大阶段");
    public static final Result HERO_SKILL_NOT_EXIST = Result.constInit(20005,"骑士技能不存在");
    public static final Result HERO_STAR_REACH_MAX = Result.constInit(20006,"骑士星级达到上限");
    public static final Result HERO_BUSINESS_SKILL_LEVEL_REACH_MAX = Result.constInit(20007,"骑士经验技能等级达到上限");
    public static final Result HERO_DONT_HAVE_HALO = Result.constInit(20008,"大臣没有光环");
    public static final Result HERO_TALENT_SKILL_NOT_EXIST = Result.constInit(20009,"骑士资质技能不存在");
    public static final Result HERO_SKIN_NOT_EXIST = Result.constInit(20010,"骑士皮肤不存在");
    public static final Result HERO_NOT_FOUND = Result.constInit(20011,"骑士不存在");
    public static final Result HERO_SKIN_ALREADY_EXIST = Result.constInit(20012,"已拥有骑士皮肤");
    public static final Result HERO_A_KEY_UPGRADE_FUNC_NOT_UNLOCK = Result.constInit(20013,"大臣一键升级功能未解锁");
    public static final Result HERO_HALO_UNLOCK_REPEAT = Result.constInit(20014,"大臣重复解锁光环");
    public static final Result HERO_NOT_PLACE = Result.constInit(20015,"大臣未驻扎");
    public static final Result HERO_HALO_NOT_UNLOCK = Result.constInit(20016,"大臣光环尚未解锁");
    public static final Result HERO_ALREADY_EXISTED = Result.constInit(20017,"大臣已经存在");
    public static final Result HERO_TALENT_SKILL_LEVEL_REACH_MAX = Result.constInit(20018,"大臣资质技能等级达到上限");
    public static final Result HERO_CANT_PLACE_TO_THIS_ATTR_BUILDING = Result.constInit(20019,"大臣无法驻扎到该相性建筑");
    public static final Result HERO_TALENT_SKILL_CANT_MANUAL_UPGREADE = Result.constInit(20020,"大臣资质技能不支持手动升级");
    public static final Result EQUIP_LEVEL_MAX = Result.constInit(20021,"藏品等级已达到最大");
    public static final Result EQUIP_IS_NOT_BEEN_WEAR = Result.constInit(20022,"藏品没有被穿戴");
    public static final Result EQUIP_NUM_REACH_LIMIT = Result.constInit(20023,"该藏品数量达到上限");
    public static final Result EQUIP_NOT_FOUND = Result.constInit(20024,"藏品不存在");
    public static final Result EQUIP_SKILL_NOT_FOUND = Result.constInit(20025,"藏品技能不存在");
    public static final Result EQUIP_RESHAPE_FAIL = Result.constInit(20026,"藏品重塑失败");
    public static final Result EQUIP_IS_LOCKED = Result.constInit(20027,"藏品已锁定");
    public static final Result EQUIP_IS_WEARED = Result.constInit(20028,"藏品已穿戴");
}
