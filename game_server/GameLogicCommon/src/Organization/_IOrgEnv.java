package Organization;

import NPCommon.DB.BM.BM;
import WCGBasicServer._AWCGBasicServer;

public interface _IOrgEnv
{
    _AWCGBasicServer getServerObj();

    BM getBM();
}
