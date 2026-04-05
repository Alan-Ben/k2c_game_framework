
package NPCommon;

public class NPVersion
{
    public static int majorVersion() {return 0;}
    public static int minorVersion() {return 6;}
    public static int buildVersion() {return 26;}
    public static int fixVersion() {return 0;}

    public static String getString()
    {
        return String.format("%d.%d.%d.%d", majorVersion(), minorVersion(), buildVersion(), fixVersion());
    }
}
