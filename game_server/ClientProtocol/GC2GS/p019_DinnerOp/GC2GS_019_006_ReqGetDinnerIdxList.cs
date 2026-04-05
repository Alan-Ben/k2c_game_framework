using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p019_DinnerOp
{

/// <summary>
/// 宴会索引列表
/// </summary>
public class GC2GS_019_006_ReqGetDinnerIdxList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 当前页数
/// </summary>
private int page;
/// <summary>
/// 展示数量，上限不超过100
/// </summary>
private int num;


public GC2GS_019_006_ReqGetDinnerIdxList() {
	page = 0;
	num = 0;
}

public GC2GS_019_006_ReqGetDinnerIdxList(
	int _page
	, int _num
) {	page = _page;
	num = _num;
}

public byte getMainOrder() { return (byte)19; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 当前页数
/// </summary>
public int getPage() { return page; }
/// <summary>
/// 当前页数
/// </summary>
public void setPage(int _page) { page = _page; }
/// <summary>
/// 展示数量，上限不超过100
/// </summary>
public int getNum() { return num; }
/// <summary>
/// 展示数量，上限不超过100
/// </summary>
public void setNum(int _num) { num = _num; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	page = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	num = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(page);
	_buf.putInt(num);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)6);
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
	builder.Append("page").Append(":").Append(page.ToString()).Append(", ");
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

