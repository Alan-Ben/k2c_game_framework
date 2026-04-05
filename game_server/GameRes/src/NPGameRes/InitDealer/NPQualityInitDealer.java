package NPGameRes.InitDealer;

import NPGameRes.Refs.Battle.RefNPQuality;

/*******************
 * 初始化mission中怪物实例信息
 * @author Administrator
 *
 */
public class NPQualityInitDealer extends _ABasicInitDealer
{
    //将mission instance初始化到mission中
    @Override
    public void dealInit()
    {
        RefNPQuality.getMgr().init();
    }

}
