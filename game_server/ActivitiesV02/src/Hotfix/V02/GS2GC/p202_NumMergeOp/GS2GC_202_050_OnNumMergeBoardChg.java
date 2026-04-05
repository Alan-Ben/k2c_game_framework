package Hotfix.V02.GS2GC.p202_NumMergeOp;

import java.nio.ByteBuffer;
/*********
 * 棋盘变更推送
 **/
public class GS2GC_202_050_OnNumMergeBoardChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 棋盘数据 */
private Hotfix.V02.Common.NumMergeObj.NumMerge_BoardData boardData;
/** 是否触发buff合并 */
private boolean isBuffTriggered;


public GS2GC_202_050_OnNumMergeBoardChg() {
	boardData = new Hotfix.V02.Common.NumMergeObj.NumMerge_BoardData();
	isBuffTriggered = false;
}

public GS2GC_202_050_OnNumMergeBoardChg(
	 Hotfix.V02.Common.NumMergeObj.NumMerge_BoardData _boardData
	, boolean _isBuffTriggered
) {	boardData = _boardData;
	isBuffTriggered = _isBuffTriggered;
}

public final byte getMainOrder() { return (byte)202; }

public final byte getSubOrder() { return (byte)50; }

/** 棋盘数据 */
public Hotfix.V02.Common.NumMergeObj.NumMerge_BoardData getBoardData() { return boardData; }
/** 棋盘数据 */
public void setBoardData(Hotfix.V02.Common.NumMergeObj.NumMerge_BoardData _boardData) { boardData = _boardData; }
/** 是否触发buff合并 */
public boolean getIsBuffTriggered() { return isBuffTriggered; }
/** 是否触发buff合并 */
public void setIsBuffTriggered(boolean _isBuffTriggered) { isBuffTriggered = _isBuffTriggered; }


public final int GetBufSize() {
	int _size = 1;
	_size += 4 + boardData.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += 4 + boardData.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _boardDataCustLen = _buf.getInt();
	int _boardDataCurPos = _buf.position();
	boardData.ReadUnzipBuf(_buf, _boardDataCurPos + _boardDataCustLen);
	_buf.position(_boardDataCurPos + _boardDataCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isBuffTriggered = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(boardData.GetBufSize());
	boardData.PutUnzipBuf(_buf);
	_buf.put(isBuffTriggered?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)202);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)202);
	_recBuf.put((byte)50);
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

