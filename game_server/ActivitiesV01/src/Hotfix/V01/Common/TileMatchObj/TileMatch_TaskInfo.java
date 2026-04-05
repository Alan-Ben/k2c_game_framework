package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-任务信息
 **/
public class TileMatch_TaskInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务序列号 */
private int serialId;
/** 任务id */
private long taskId;
/** 子任务列表 */
private java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_TaskSubInfo> subList;
/** 已经走的步数 */
private int hadGoStep;


public TileMatch_TaskInfo() {
	serialId = 0;
	taskId = (long)0;
	subList = new java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_TaskSubInfo>();
	hadGoStep = 0;
}

public TileMatch_TaskInfo(
	 int _serialId
	, long _taskId
	, java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_TaskSubInfo> _subList
	, int _hadGoStep
) {	serialId = _serialId;
	taskId = _taskId;
	subList = _subList;
	hadGoStep = _hadGoStep;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 任务序列号 */
public int getSerialId() { return serialId; }
/** 任务序列号 */
public void setSerialId(int _serialId) { serialId = _serialId; }
/** 任务id */
public long getTaskId() { return taskId; }
/** 任务id */
public void setTaskId(long _taskId) { taskId = _taskId; }
/** 子任务列表 */
public java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_TaskSubInfo> getSubList() { return subList; }
/** 子任务列表 */
public void addSubList(Hotfix.V01.Common.TileMatchObj.TileMatch_TaskSubInfo _subList) { subList.add(_subList); }
/** 已经走的步数 */
public int getHadGoStep() { return hadGoStep; }
/** 已经走的步数 */
public void setHadGoStep(int _hadGoStep) { hadGoStep = _hadGoStep; }


public final int GetBufSize() {
	int _size = 16;
	_size += 2 + (subList.size() * 12);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (subList.size() * 12);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serialId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) taskId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _subListCount = _buf.getShort();
	for(int _i = 0; _i < _subListCount; _i++) { 
		Hotfix.V01.Common.TileMatchObj.TileMatch_TaskSubInfo _subList = new Hotfix.V01.Common.TileMatchObj.TileMatch_TaskSubInfo();
		if(_buf.remaining() <= 0) return;
	int __subListCustLen = _buf.getInt();
	int __subListCurPos = _buf.position();
	_subList.ReadUnzipBuf(_buf, __subListCurPos + __subListCustLen);
	_buf.position(__subListCurPos + __subListCustLen);

		subList.add(_subList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadGoStep = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serialId);
	_buf.putLong(taskId);
	_buf.putShort((short)subList.size());
	for(int _i = 0; _i < subList.size(); _i++) { 
		_buf.putInt(subList.get(_i).GetBufSize());
	subList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(hadGoStep);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

