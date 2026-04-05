package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 阶段任务数据组件初始化
 **/
public class GS2GC_002_062_RetStageGoalInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 当前阶段任务数据 */
private Common.StageGoalObj.StageGoal_Info curStageGoal;
/** 已领取的大阶段奖励列表 */
private java.util.ArrayList<Long> hadDrawBigStepList;
/** 已领取的大阶段首达奖励列表 */
private java.util.ArrayList<Long> hadDrawBigStepFirstReachList;
/** 可领取的大阶段首达奖励列表 */
private java.util.ArrayList<Long> canDrawBigStepFirstReachList;


public GS2GC_002_062_RetStageGoalInit() {
	curStageGoal = new Common.StageGoalObj.StageGoal_Info();
	hadDrawBigStepList = new java.util.ArrayList<Long>();
	hadDrawBigStepFirstReachList = new java.util.ArrayList<Long>();
	canDrawBigStepFirstReachList = new java.util.ArrayList<Long>();
}

public GS2GC_002_062_RetStageGoalInit(
	 Common.StageGoalObj.StageGoal_Info _curStageGoal
	, java.util.ArrayList<Long> _hadDrawBigStepList
	, java.util.ArrayList<Long> _hadDrawBigStepFirstReachList
	, java.util.ArrayList<Long> _canDrawBigStepFirstReachList
) {	curStageGoal = _curStageGoal;
	hadDrawBigStepList = _hadDrawBigStepList;
	hadDrawBigStepFirstReachList = _hadDrawBigStepFirstReachList;
	canDrawBigStepFirstReachList = _canDrawBigStepFirstReachList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)62; }

/** 当前阶段任务数据 */
public Common.StageGoalObj.StageGoal_Info getCurStageGoal() { return curStageGoal; }
/** 当前阶段任务数据 */
public void setCurStageGoal(Common.StageGoalObj.StageGoal_Info _curStageGoal) { curStageGoal = _curStageGoal; }
/** 已领取的大阶段奖励列表 */
public java.util.ArrayList<Long> getHadDrawBigStepList() { return hadDrawBigStepList; }
/** 已领取的大阶段奖励列表 */
public void addHadDrawBigStepList(long _hadDrawBigStepList) { hadDrawBigStepList.add(_hadDrawBigStepList); }
/** 已领取的大阶段首达奖励列表 */
public java.util.ArrayList<Long> getHadDrawBigStepFirstReachList() { return hadDrawBigStepFirstReachList; }
/** 已领取的大阶段首达奖励列表 */
public void addHadDrawBigStepFirstReachList(long _hadDrawBigStepFirstReachList) { hadDrawBigStepFirstReachList.add(_hadDrawBigStepFirstReachList); }
/** 可领取的大阶段首达奖励列表 */
public java.util.ArrayList<Long> getCanDrawBigStepFirstReachList() { return canDrawBigStepFirstReachList; }
/** 可领取的大阶段首达奖励列表 */
public void addCanDrawBigStepFirstReachList(long _canDrawBigStepFirstReachList) { canDrawBigStepFirstReachList.add(_canDrawBigStepFirstReachList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + curStageGoal.GetBufSize();
	_size += 2 + (hadDrawBigStepList.size() * 8);
	_size += 2 + (hadDrawBigStepFirstReachList.size() * 8);
	_size += 2 + (canDrawBigStepFirstReachList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + curStageGoal.GetBufSize();
	_size += 2 + (hadDrawBigStepList.size() * 8);
	_size += 2 + (hadDrawBigStepFirstReachList.size() * 8);
	_size += 2 + (canDrawBigStepFirstReachList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _curStageGoalCustLen = _buf.getInt();
	int _curStageGoalCurPos = _buf.position();
	curStageGoal.ReadUnzipBuf(_buf, _curStageGoalCurPos + _curStageGoalCustLen);
	_buf.position(_curStageGoalCurPos + _curStageGoalCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawBigStepListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawBigStepListCount; _i++) { 
		long _hadDrawBigStepList = (long)0;
		if(_buf.remaining() > 0) _hadDrawBigStepList = _buf.getLong();
		hadDrawBigStepList.add(_hadDrawBigStepList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawBigStepFirstReachListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawBigStepFirstReachListCount; _i++) { 
		long _hadDrawBigStepFirstReachList = (long)0;
		if(_buf.remaining() > 0) _hadDrawBigStepFirstReachList = _buf.getLong();
		hadDrawBigStepFirstReachList.add(_hadDrawBigStepFirstReachList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _canDrawBigStepFirstReachListCount = _buf.getShort();
	for(int _i = 0; _i < _canDrawBigStepFirstReachListCount; _i++) { 
		long _canDrawBigStepFirstReachList = (long)0;
		if(_buf.remaining() > 0) _canDrawBigStepFirstReachList = _buf.getLong();
		canDrawBigStepFirstReachList.add(_canDrawBigStepFirstReachList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(curStageGoal.GetBufSize());
	curStageGoal.PutUnzipBuf(_buf);
	_buf.putShort((short)hadDrawBigStepList.size());
	for(int _i = 0; _i < hadDrawBigStepList.size(); _i++) { 
		_buf.putLong(hadDrawBigStepList.get(_i));
	}
	_buf.putShort((short)hadDrawBigStepFirstReachList.size());
	for(int _i = 0; _i < hadDrawBigStepFirstReachList.size(); _i++) { 
		_buf.putLong(hadDrawBigStepFirstReachList.get(_i));
	}
	_buf.putShort((short)canDrawBigStepFirstReachList.size());
	for(int _i = 0; _i < canDrawBigStepFirstReachList.size(); _i++) { 
		_buf.putLong(canDrawBigStepFirstReachList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)62);
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

