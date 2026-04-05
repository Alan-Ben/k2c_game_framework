package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_071_OnStageGoalChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 阶段任务数据 */
private Common.StageGoalObj.StageGoal_Info stageGoal;


public GS2GC_007_071_OnStageGoalChg() {
	stageGoal = new Common.StageGoalObj.StageGoal_Info();
}

public GS2GC_007_071_OnStageGoalChg(
	 Common.StageGoalObj.StageGoal_Info _stageGoal
) {	stageGoal = _stageGoal;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)71; }

/** 阶段任务数据 */
public Common.StageGoalObj.StageGoal_Info getStageGoal() { return stageGoal; }
/** 阶段任务数据 */
public void setStageGoal(Common.StageGoalObj.StageGoal_Info _stageGoal) { stageGoal = _stageGoal; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + stageGoal.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + stageGoal.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _stageGoalCustLen = _buf.getInt();
	int _stageGoalCurPos = _buf.position();
	stageGoal.ReadUnzipBuf(_buf, _stageGoalCurPos + _stageGoalCustLen);
	_buf.position(_stageGoalCurPos + _stageGoalCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(stageGoal.GetBufSize());
	stageGoal.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)71);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)71);
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

