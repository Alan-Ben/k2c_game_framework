package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp;

import Common.TreasureHuntObj.TreasureHunt_CaptureResult;

import java.util.ArrayList;
import java.util.List;

public class TreasureHuntCaptureResult
{
    public List<TreasureHunt_CaptureResult> captureResultList;
    public int exp;

    public TreasureHuntCaptureResult()
    {
        this.captureResultList = new ArrayList<>();
    }
}
