package Hotfix.V01.GS2GC.p201_TileMatchOp;

import java.nio.ByteBuffer;
/*********
 * 三消任务数据变更
 **/
public class GS2GC_201_052_OnTileMatchTaskChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 模式类型 */
private int modeType;
/** 任务数据 */
private Hotfix.V01.Common.TileMatchObj.TileMatch_TaskInfo taskInfo;


public GS2GC_201_052_OnTileMatchTaskChg() {
	modeType = 0;
	taskInfo = new Hotfix.V01.Common.TileMatchObj.TileMatch_TaskInfo();
}

public GS2GC_201_052_OnTileMatchTaskChg(
	 int _modeType
	, Hotfix.V01.Common.TileMatchObj.TileMatch_TaskInfo _taskInfo
) {	modeType = _modeType;
	taskInfo = _taskInfo;
}

public final byte getMainOrder() { return (byte)201; }

public final byte getSubOrder() { return (byte)52; }

/** 模式类型 */
public int getModeType() { return modeType; }
/** 模式类型 */
public void setModeType(int _modeType) { modeType = _modeType; }
/** 任务数据 */
public Hotfix.V01.Common.TileMatchObj.TileMatch_TaskInfo getTaskInfo() { return taskInfo; }
/** 任务数据 */
public void setTaskInfo(Hotfix.V01.Common.TileMatchObj.TileMatch_TaskInfo _taskInfo) { taskInfo = _taskInfo; }


public final int GetBufSize() {
	int _size = 4;
	_size += 4 + taskInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + taskInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) modeType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _taskInfoCustLen = _buf.getInt();
	int _taskInfoCurPos = _buf.position();
	taskInfo.ReadUnzipBuf(_buf, _taskInfoCurPos + _taskInfoCustLen);
	_buf.position(_taskInfoCurPos + _taskInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(modeType);
	_buf.putInt(taskInfo.GetBufSize());
	taskInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)52);
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

