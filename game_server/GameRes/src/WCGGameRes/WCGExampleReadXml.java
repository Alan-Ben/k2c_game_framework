package WCGGameRes;

import NPXML.ReadFileFunc._AWCGXMLReadingObj;
import NPXML.WCGResXMLFunc;
import NPXML.WCGXMLNodeInfo;
import org.w3c.dom.Node;

public class WCGExampleReadXml
{
    /********************
     * 从节点中读取相关信息
     *
     * @author alzq.z
     * @time Jun 27, 2013 12:29:59 AM
     */
    public static Integer readXML(WCGXMLNodeInfo _nodeInfo)
    {
        Integer test = 1;

        WCGResXMLFunc.readXML(
                _nodeInfo
                , test
                , new _AWCGXMLReadingObj<Integer>()
                {
                    @Override
                    public boolean readingNode(Integer _ref, Node _node, String _name)
                    {
                        return false;
                    }

                    @Override
                    public void readingNodeValue(Integer _test, String _name, String _value)
                    {
                        if (_name.equalsIgnoreCase("id"))
                        {
                            _test = Integer.parseInt(_value.trim());
                        }
                    }
                }
        );

        return test;
    }
}
