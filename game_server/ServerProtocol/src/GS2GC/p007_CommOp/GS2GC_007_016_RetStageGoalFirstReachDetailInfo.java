package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_016_RetStageGoalFirstReachDetailInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 大阶段首达信息列表 */
private java.util.ArrayList<Common.StageGoalObj.StageGoal_BigStepFirstReachInfo> infoList;


public GS2GC_007_016_RetStageGoalFirstReachDetailInfo() {
	infoList = new java.util.ArrayList<Common.StageGoalObj.StageGoal_BigStepFirstReachInfo>();
}

public GS2GC_007_016_RetStageGoalFirstReachDetailInfo(
	 java.util.ArrayList<Common.StageGoalObj.StageGoal_BigStepFirstReachInfo> _infoList
) {	infoList = _infoList;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)16; }

/** 大阶段首达信息列表 */
public java.util.ArrayList<Common.StageGoalObj.StageGoal_BigStepFirstReachInfo> getInfoList() { return infoList; }
/** 大阶段首达信息列表 */
public void addInfoList(Common.StageGoalObj.StageGoal_BigStepFirstReachInfo _infoList) { infoList.add(_infoList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (infoList.size() * 28);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (infoList.size() * 28);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _infoListCount = _buf.getShort();
	for(int _i = 0; _i < _infoListCount; _i++) { 
		Common.StageGoalObj.StageGoal_BigStepFirstReachInfo _infoList = new Common.StageGoalObj.StageGoal_BigStepFirstReachInfo();
		if(_buf.remaining() <= 0) return;
	int __infoListCustLen = _buf.getInt();
	int __infoListCurPos = _buf.position();
	_infoList.ReadUnzipBuf(_buf, __infoListCurPos + __infoListCustLen);
	_buf.position(__infoListCurPos + __infoListCustLen);

		infoList.add(_infoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)infoList.size());
	for(int _i = 0; _i < infoList.size(); _i++) { 
		_buf.putInt(infoList.get(_i).GetBufSize());
	infoList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)16);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)16);
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

