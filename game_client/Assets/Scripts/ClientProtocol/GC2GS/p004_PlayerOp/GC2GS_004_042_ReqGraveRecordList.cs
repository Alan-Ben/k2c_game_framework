using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

/// <summary>
/// 杰出者数据记录
/// </summary>
public class GC2GS_004_042_ReqGraveRecordList : ALBasicProtocolPack._IALProtocolStructure {
private int typeId;
/// <summary>
/// 当前页数
/// </summary>
private int curPage;
/// <summary>
/// 每页数量，不超过50条
/// </summary>
private int pageCount;


public GC2GS_004_042_ReqGraveRecordList() {
	typeId = 0;
	curPage = 0;
	pageCount = 0;
}

public GC2GS_004_042_ReqGraveRecordList(
	int _typeId
	, int _curPage
	, int _pageCount
) {	typeId = _typeId;
	curPage = _curPage;
	pageCount = _pageCount;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)42; }

public int getTypeId() { return typeId; }
public void setTypeId(int _typeId) { typeId = _typeId; }
/// <summary>
/// 当前页数
/// </summary>
public int getCurPage() { return curPage; }
/// <summary>
/// 当前页数
/// </summary>
public void setCurPage(int _curPage) { curPage = _curPage; }
/// <summary>
/// 每页数量，不超过50条
/// </summary>
public int getPageCount() { return pageCount; }
/// <summary>
/// 每页数量，不超过50条
/// </summary>
public void setPageCount(int _pageCount) { pageCount = _pageCount; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	typeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curPage = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pageCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(typeId);
	_buf.putInt(curPage);
	_buf.putInt(pageCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)42);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)42);
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
	builder.Append("typeId").Append(":").Append(typeId.ToString()).Append(", ");
	builder.Append("curPage").Append(":").Append(curPage.ToString()).Append(", ");
	builder.Append("pageCount").Append(":").Append(pageCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

