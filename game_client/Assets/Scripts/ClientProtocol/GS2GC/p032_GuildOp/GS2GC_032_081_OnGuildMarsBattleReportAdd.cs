using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

/// <summary>
/// 火星矿-新增联盟战报推送
/// </summary>
public class GS2GC_032_081_OnGuildMarsBattleReportAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 最新联盟战报数据库ID
/// </summary>
private long id;


public GS2GC_032_081_OnGuildMarsBattleReportAdd() {
	id = (long)0;
}

public GS2GC_032_081_OnGuildMarsBattleReportAdd(
	long _id
) {	id = _id;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)81; }

/// <summary>
/// 最新联盟战报数据库ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 最新联盟战报数据库ID
/// </summary>
public void setId(long _id) { id = _id; }


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
	id = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)81);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)81);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

