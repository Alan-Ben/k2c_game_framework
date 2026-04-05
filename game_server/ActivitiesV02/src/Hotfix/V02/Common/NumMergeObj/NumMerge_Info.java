package Hotfix.V02.Common.NumMergeObj;

import java.nio.ByteBuffer;
/*********
 * 数字合并-完整状态信息
 **/
public class NumMerge_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 棋盘数据 */
private Hotfix.V02.Common.NumMergeObj.NumMerge_BoardData boardData;
/** 宝箱信息 */
private Hotfix.V02.Common.NumMergeObj.NumMerge_BoxInfo boxInfo;


public NumMerge_Info() {
	boardData = new Hotfix.V02.Common.NumMergeObj.NumMerge_BoardData();
	boxInfo = new Hotfix.V02.Common.NumMergeObj.NumMerge_BoxInfo();
}

public NumMerge_Info(
	 Hotfix.V02.Common.NumMergeObj.NumMerge_BoardData _boardData
	, Hotfix.V02.Common.NumMergeObj.NumMerge_BoxInfo _boxInfo
) {	boardData = _boardData;
	boxInfo = _boxInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 棋盘数据 */
public Hotfix.V02.Common.NumMergeObj.NumMerge_BoardData getBoardData() { return boardData; }
/** 棋盘数据 */
public void setBoardData(Hotfix.V02.Common.NumMergeObj.NumMerge_BoardData _boardData) { boardData = _boardData; }
/** 宝箱信息 */
public Hotfix.V02.Common.NumMergeObj.NumMerge_BoxInfo getBoxInfo() { return boxInfo; }
/** 宝箱信息 */
public void setBoxInfo(Hotfix.V02.Common.NumMergeObj.NumMerge_BoxInfo _boxInfo) { boxInfo = _boxInfo; }


public final int GetBufSize() {
	int _size = 16;
	_size += 4 + boardData.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
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
	if(_buf.remaining() <= 0) return;
	int _boxInfoCustLen = _buf.getInt();
	int _boxInfoCurPos = _buf.position();
	boxInfo.ReadUnzipBuf(_buf, _boxInfoCurPos + _boxInfoCustLen);
	_buf.position(_boxInfoCurPos + _boxInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(boardData.GetBufSize());
	boardData.PutUnzipBuf(_buf);
	_buf.putInt(boxInfo.GetBufSize());
	boxInfo.PutUnzipBuf(_buf);
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

