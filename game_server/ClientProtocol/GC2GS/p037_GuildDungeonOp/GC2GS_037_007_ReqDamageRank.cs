using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p037_GuildDungeonOp
{

/// <summary>
/// 公会副本-伤害排行榜
/// </summary>
public class GC2GS_037_007_ReqDamageRank : ALBasicProtocolPack._IALProtocolStructure {
private int page;
/// <summary>
/// 每页数量，不超过100
/// </summary>
private int pageNum;


public GC2GS_037_007_ReqDamageRank() {
	page = 0;
	pageNum = 0;
}

public GC2GS_037_007_ReqDamageRank(
	int _page
	, int _pageNum
) {	page = _page;
	pageNum = _pageNum;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)7; }

public int getPage() { return page; }
public void setPage(int _page) { page = _page; }
/// <summary>
/// 每页数量，不超过100
/// </summary>
public int getPageNum() { return pageNum; }
/// <summary>
/// 每页数量，不超过100
/// </summary>
public void setPageNum(int _pageNum) { pageNum = _pageNum; }


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
	pageNum = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(page);
	_buf.putInt(pageNum);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)7);
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
	builder.Append("pageNum").Append(":").Append(pageNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

