using System.Collections.Generic;
using JetBrains.Annotations;

/// <summary>
/// 热更脚本版本号（SC:SourceCode）
/// </summary>
public class HotfixSCVersion
{
    public int main = 0;
    public int sub = 6;
    public int patch = 26;
    public int build = 1;
    public int date = 326;
    
    [NotNull] public static HotfixSCVersion instance = new HotfixSCVersion();

    public static List<int> version = new List<int>()
    {
        instance.main,
        instance.sub,
        instance.patch,
        instance.build,
        instance.date
    };
    
    public string clientShowId { get { return $"{main}.{sub}.{patch}.{build}.{date}"; } }

}
