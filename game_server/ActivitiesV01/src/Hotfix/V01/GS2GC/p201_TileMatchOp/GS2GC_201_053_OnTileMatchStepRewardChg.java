package Hotfix.V01.GS2GC.p201_TileMatchOp;

import java.nio.ByteBuffer;
/*********
 * 三消阶段奖励数据变更
 **/
public class GS2GC_201_053_OnTileMatchStepRewardChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 阶段奖励数据 */
private Hotfix.V01.Common.TileMatchObj.TileMatch_StepRewardInfo stepRewardInfo;


public GS2GC_201_053_OnTileMatchStepRewardChg() {
	stepRewardInfo = new Hotfix.V01.Common.TileMatchObj.TileMatch_StepRewardInfo();
}

public GS2GC_201_053_OnTileMatchStepRewardChg(
	 Hotfix.V01.Common.TileMatchObj.TileMatch_StepRewardInfo _stepRewardInfo
) {	stepRewardInfo = _stepRewardInfo;
}

public final byte getMainOrder() { return (byte)201; }

public final byte getSubOrder() { return (byte)53; }

/** 阶段奖励数据 */
public Hotfix.V01.Common.TileMatchObj.TileMatch_StepRewardInfo getStepRewardInfo() { return stepRewardInfo; }
/** 阶段奖励数据 */
public void setStepRewardInfo(Hotfix.V01.Common.TileMatchObj.TileMatch_StepRewardInfo _stepRewardInfo) { stepRewardInfo = _stepRewardInfo; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _stepRewardInfoCustLen = _buf.getInt();
	int _stepRewardInfoCurPos = _buf.position();
	stepRewardInfo.ReadUnzipBuf(_buf, _stepRewardInfoCurPos + _stepRewardInfoCustLen);
	_buf.position(_stepRewardInfoCurPos + _stepRewardInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(stepRewardInfo.GetBufSize());
	stepRewardInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)53);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

