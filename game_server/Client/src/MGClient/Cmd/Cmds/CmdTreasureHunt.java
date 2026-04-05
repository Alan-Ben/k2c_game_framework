package MGClient.Cmd.Cmds;

import Common.TreasureHuntEnum.ETreasureHuntCaptureType;
import GC2GS.p036_TreasureHuntOp.GC2GS_036_001_ReqTreasureHuntOreCapture;
import GC2GS.p036_TreasureHuntOp.GC2GS_036_009_ReqTreasureHuntTransOre;
import GS2GC.p036_TreasureHuntOp.GS2GC_036_001_RetTreasureHuntOreCapture;
import GS2GC.p036_TreasureHuntOp.GS2GC_036_009_RetTreasureHuntTransOre;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

/**
 * @author Scott
 * @date 2016年7月8日
 */
@Commander(comment = "太空寻宝", name = "treasureHunt")
public class CmdTreasureHunt extends CmdBase
{
    @Command(comment = "捕获")
    public void capture(long _areaId, int _distance, ETreasureHuntCaptureType _type, boolean _isAdvance)
    {
        GC2GS_036_001_ReqTreasureHuntOreCapture proto = new GC2GS_036_001_ReqTreasureHuntOreCapture();
        proto.setAreaId(_areaId);
        proto.setDistance(_distance);
        proto.setType(_type);
        proto.setIsAdvance(_isAdvance);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_036_001_RetTreasureHuntOreCapture>(GS2GC_036_001_RetTreasureHuntOreCapture.class) {
            @Override
            public void handle(GS2GC_036_001_RetTreasureHuntOreCapture _response) {

            }
        });
    }

    @Command(comment = "转换矿石")
    public void transOre()
    {
        GC2GS_036_009_ReqTreasureHuntTransOre proto = new GC2GS_036_009_ReqTreasureHuntTransOre();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_036_009_RetTreasureHuntTransOre>(GS2GC_036_009_RetTreasureHuntTransOre.class) {
            @Override
            public void handle(GS2GC_036_009_RetTreasureHuntTransOre _response) {

            }
        });
    }


}
