package NPGameRes.Refs;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;
import NPGameRes.GameObjs.Battle.DecodeInt;

@RefTable(tableName = "unicode_length_check")
public class RefUnicodeLengthCheck extends RefBase
{
    private static RefUnicodeLengthCheckMgr _g_mgr = new RefUnicodeLengthCheckMgr();

    public static RefUnicodeLengthCheckMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefUnicodeLengthCheckMgr extends RefListContainer<RefUnicodeLengthCheck>
    {
        /**
         * 根据配置去字符串长度
         * @param _str
         * @return
         */
        public int getCharLength(String _str)
        {
            int strLength = 0;
            if(_str.isEmpty())
                return strLength;

            for(int i = 0; i < _str.length(); i++)
            {
                int chr = (char) _str.charAt(i);
                boolean inArea = false;
                for(RefUnicodeLengthCheck ref : getList()){
                    if(chr >= ref.min_range_include.value && chr <= ref.max_range_include.value)
                    {
                        strLength += ref.length;
                        inArea = true;
                        break;
                    }
                }

                //不在配置区间,默认长度为1
                if(!inArea)
                {
                    strLength += 1;
                }
            }

            return strLength;
        }
    }

    //////////////////////////////
    @Override
    public RefUnicodeLengthCheckMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefUnicodeLengthCheckMgr) _mgr;
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefUnicodeLengthCheck newRef = (RefUnicodeLengthCheck) _newRef;
        id = newRef.id;
        min_range_include = newRef.min_range_include;
        max_range_include = newRef.max_range_include;
        length = newRef.length;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////
    public long id;
    public DecodeInt min_range_include;//左范围，最小值0x0000
    public DecodeInt max_range_include;//右范围，最大值0xffff
    public int length;//字符长度

}
