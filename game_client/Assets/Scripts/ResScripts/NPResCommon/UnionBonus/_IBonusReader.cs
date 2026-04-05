using CommonEnum;

namespace GOE
{
   /**
   * @description: 属性加成 读取接口，
   * 实现本接口的对象必须包含一个无参构造函数，详见{@link BonusReaderMgr#createNew(EPropBonusType)}
   */
    public interface _IBonusReader : _IParseFromStringable
    {
        /************
         * 获取加成类型
         * @return
         */
        EBonusFilterType getFilterType();

        /**
         * 获取对应的子加成id筛选数据，如无筛选则返回0
         * @return ENPPropBonusType
         */
        long getBonusId();

        /*********
         * 获取实际加成数据
         * @return
         */
        PlayerBonusPropertyModifier getBonusPropertyModifier();
    }
 
}