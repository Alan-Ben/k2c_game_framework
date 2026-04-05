package NPGameRes;

import NPGameRes.GameObjs.NPActorProperty.NPPropertyContainer;

public class NPResFunc
{


    /*****
     * 读取怪物等级属性 等级带来的攻防血*(宠物配表的升级偏向）
     * @param _petId
     * @param _level
     * @return
     */
    public static void calcMobLvlProperty(long _petId, int _level, NPPropertyContainer _container)
    {

//        if (_level < 100000)
//        {
//            _level = 100000 + _level;
//        }
//        RefPetLvl refPetLvl = RefPetLvl.getMgr().get(_level);
//        if (null == refPetLvl)
//        {
//            return;
//        }
//        NPPropertyModifier modifier = refPetLvl.prop_add_list.duplicate();
//
//        RefPet refPet = RefPet.getMgr().get(_petId);
//        if (refPet != null)
//        {
//            //计算等级增加的攻防血
//            //乘以偏向系数
//            modifier = NPPropertyModifier.mulPercent(modifier, refPet.lvl_up_prop_bias);
//            //职业偏向
//            modifier = NPPropertyModifier.mulPercent(modifier, refPet.class_prop_bias);
//        }
//        _container.addModifier(modifier);
    }
}
