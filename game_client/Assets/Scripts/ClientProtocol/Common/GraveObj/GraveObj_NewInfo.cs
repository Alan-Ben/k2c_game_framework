using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GraveObj
{

/// <summary>
/// 新晋杰出者信息
/// </summary>
public class GraveObj_NewInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 达成称号
/// </summary>
private long titleId;
/// <summary>
/// 玩家CID
/// </summary>
private long cid;


public GraveObj_NewInfo() {
	titleId = (long)0;
	cid = (long)0;
}

public GraveObj_NewInfo(
	long _titleId
	, long _cid
) {	titleId = _titleId;
	cid = _cid;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 达成称号
/// </summary>
public long getTitleId() { return titleId; }
/// <summary>
/// 达成称号
/// </summary>
public void setTitleId(long _titleId) { titleId = _titleId; }
/// <summary>
/// 玩家CID
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家CID
/// </summary>
public void setCid(long _cid) { cid = _cid; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	titleId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(titleId);
	_buf.putLong(cid);
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
	builder.Append("titleId").Append(":").Append(titleId.ToString()).Append(", ");
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

