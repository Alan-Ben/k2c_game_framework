package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 爬塔相关错误
 ****/
 
public class TowerErr implements _IErrHolder
{
    public static final Result TOWER_FLOOR_HAD_BEEN_OCCUPIED = Result.constInit(450001,"爬塔楼层已被占领");
    public static final Result TOWER_NOT_ATTACK_FORWARD = Result.constInit(450002,"爬塔不能向前进攻关卡");
    public static final Result TOWER_RESEARCH_ALREADY_ACTIVE = Result.constInit(450003,"爬塔研究已激活");
    public static final Result TOWER_CHAPTER_RESEARCH_NOT_DONE = Result.constInit(450004,"爬塔章节研究未完成");
    public static final Result TOWER_RESEARCH_NOT_FOUND = Result.constInit(450005,"爬塔研究未找到");
}
