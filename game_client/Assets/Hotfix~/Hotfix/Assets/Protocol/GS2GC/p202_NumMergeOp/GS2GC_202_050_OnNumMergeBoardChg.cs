using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GS2GC.p202_NumMergeOp
{

/// <summary>
/// 棋盘变更推送
/// </summary>
public class GS2GC_202_050_OnNumMergeBoardChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 棋盘数据
/// </summary>
private Hotfix.Common.NumMergeObj.NumMerge_BoardData boardData;
/// <summary>
/// 是否触发buff合并
/// </summary>
private bool isBuffTriggered;


public GS2GC_202_050_OnNumMergeBoardChg() {
	boardData = new Hotfix.Common.NumMergeObj.NumMerge_BoardData();
	isBuffTriggered = false;
}

public GS2GC_202_050_OnNumMergeBoardChg(
	Hotfix.Common.NumMergeObj.NumMerge_BoardData _boardData
	, bool _isBuffTriggered
) {	boardData = _boardData;
	isBuffTriggered = _isBuffTriggered;
}

public byte getMainOrder() { return (byte)202; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 棋盘数据
/// </summary>
public Hotfix.Common.NumMergeObj.NumMerge_BoardData getBoardData() { return boardData; }
/// <summary>
/// 棋盘数据
/// </summary>
public void setBoardData(Hotfix.Common.NumMergeObj.NumMerge_BoardData _boardData) { boardData = _boardData; }
/// <summary>
/// 是否触发buff合并
/// </summary>
public bool getIsBuffTriggered() { return isBuffTriggered; }
/// <summary>
/// 是否触发buff合并
/// </summary>
public void setIsBuffTriggered(bool _isBuffTriggered) { isBuffTriggered = _isBuffTriggered; }


public int GetBufSize() {
	int _size = 1;
	_size += 4 + boardData.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;
	_size += 4 + boardData.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _boardDataCustLen = _buf.getInt();
	int _boardDataCurPos = _buf.getCurPos();
	boardData.ReadUnzipBuf(_buf, _boardDataCurPos + _boardDataCustLen);
	_buf.setPosition(_boardDataCurPos + _boardDataCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isBuffTriggered = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(boardData.GetBufSize());
	boardData.PutUnzipBuf(_buf);
	_buf.put(isBuffTriggered?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)202);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)202);
	_recBuf.put((byte)50);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("boardData").Append(":").Append(boardData == null ? "null" : boardData.ToString()).Append(", ");
	builder.Append("isBuffTriggered").Append(":").Append(isBuffTriggered.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

