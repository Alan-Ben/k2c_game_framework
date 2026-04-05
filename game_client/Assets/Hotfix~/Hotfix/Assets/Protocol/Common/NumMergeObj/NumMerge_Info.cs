using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.NumMergeObj
{

/// <summary>
/// 数字合并-完整状态信息
/// </summary>
public class NumMerge_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 棋盘数据
/// </summary>
private Hotfix.Common.NumMergeObj.NumMerge_BoardData boardData;
/// <summary>
/// 宝箱信息
/// </summary>
private Hotfix.Common.NumMergeObj.NumMerge_BoxInfo boxInfo;


public NumMerge_Info() {
	boardData = new Hotfix.Common.NumMergeObj.NumMerge_BoardData();
	boxInfo = new Hotfix.Common.NumMergeObj.NumMerge_BoxInfo();
}

public NumMerge_Info(
	Hotfix.Common.NumMergeObj.NumMerge_BoardData _boardData
	, Hotfix.Common.NumMergeObj.NumMerge_BoxInfo _boxInfo
) {	boardData = _boardData;
	boxInfo = _boxInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 棋盘数据
/// </summary>
public Hotfix.Common.NumMergeObj.NumMerge_BoardData getBoardData() { return boardData; }
/// <summary>
/// 棋盘数据
/// </summary>
public void setBoardData(Hotfix.Common.NumMergeObj.NumMerge_BoardData _boardData) { boardData = _boardData; }
/// <summary>
/// 宝箱信息
/// </summary>
public Hotfix.Common.NumMergeObj.NumMerge_BoxInfo getBoxInfo() { return boxInfo; }
/// <summary>
/// 宝箱信息
/// </summary>
public void setBoxInfo(Hotfix.Common.NumMergeObj.NumMerge_BoxInfo _boxInfo) { boxInfo = _boxInfo; }


public int GetBufSize() {
	int _size = 16;
	_size += 4 + boardData.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
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
	int _boxInfoCustLen = _buf.getInt();
	int _boxInfoCurPos = _buf.getCurPos();
	boxInfo.ReadUnzipBuf(_buf, _boxInfoCurPos + _boxInfoCustLen);
	_buf.setPosition(_boxInfoCurPos + _boxInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(boardData.GetBufSize());
	boardData.PutUnzipBuf(_buf);
	_buf.putInt(boxInfo.GetBufSize());
	boxInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("boxInfo").Append(":").Append(boxInfo == null ? "null" : boxInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

