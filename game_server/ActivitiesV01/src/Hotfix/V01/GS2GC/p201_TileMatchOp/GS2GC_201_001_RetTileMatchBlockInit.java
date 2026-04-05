package Hotfix.V01.GS2GC.p201_TileMatchOp;

import java.nio.ByteBuffer;
public class GS2GC_201_001_RetTileMatchBlockInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 方块数据 */
private java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo> blockList;
/** 任务数据 */
private Hotfix.V01.Common.TileMatchObj.TileMatch_TaskInfo taskInfo;


public GS2GC_201_001_RetTileMatchBlockInit() {
	blockList = new java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo>();
	taskInfo = new Hotfix.V01.Common.TileMatchObj.TileMatch_TaskInfo();
}

public GS2GC_201_001_RetTileMatchBlockInit(
	 java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo> _blockList
	, Hotfix.V01.Common.TileMatchObj.TileMatch_TaskInfo _taskInfo
) {	blockList = _blockList;
	taskInfo = _taskInfo;
}

public final byte getMainOrder() { return (byte)201; }

public final byte getSubOrder() { return (byte)1; }

/** 方块数据 */
public java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo> getBlockList() { return blockList; }
/** 方块数据 */
public void addBlockList(Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo _blockList) { blockList.add(_blockList); }
/** 任务数据 */
public Hotfix.V01.Common.TileMatchObj.TileMatch_TaskInfo getTaskInfo() { return taskInfo; }
/** 任务数据 */
public void setTaskInfo(Hotfix.V01.Common.TileMatchObj.TileMatch_TaskInfo _taskInfo) { taskInfo = _taskInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (blockList.size() * 12);
	_size += 4 + taskInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (blockList.size() * 12);
	_size += 4 + taskInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _blockListCount = _buf.getShort();
	for(int _i = 0; _i < _blockListCount; _i++) { 
		Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo _blockList = new Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo();
		if(_buf.remaining() <= 0) return;
	int __blockListCustLen = _buf.getInt();
	int __blockListCurPos = _buf.position();
	_blockList.ReadUnzipBuf(_buf, __blockListCurPos + __blockListCustLen);
	_buf.position(__blockListCurPos + __blockListCustLen);

		blockList.add(_blockList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _taskInfoCustLen = _buf.getInt();
	int _taskInfoCurPos = _buf.position();
	taskInfo.ReadUnzipBuf(_buf, _taskInfoCurPos + _taskInfoCustLen);
	_buf.position(_taskInfoCurPos + _taskInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)blockList.size());
	for(int _i = 0; _i < blockList.size(); _i++) { 
		_buf.putInt(blockList.get(_i).GetBufSize());
	blockList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(taskInfo.GetBufSize());
	taskInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)1);
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

