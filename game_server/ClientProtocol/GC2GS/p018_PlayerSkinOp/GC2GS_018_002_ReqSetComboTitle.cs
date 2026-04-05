using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p018_PlayerSkinOp
{

/// <summary>
/// 穿戴组合称号
/// </summary>
public class GC2GS_018_002_ReqSetComboTitle : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private long preId;
/// <summary>
/// 空
/// </summary>
private long sfxId;
/// <summary>
/// 空
/// </summary>
private long bgId;


public GC2GS_018_002_ReqSetComboTitle() {
	preId = (long)0;
	sfxId = (long)0;
	bgId = (long)0;
}

public GC2GS_018_002_ReqSetComboTitle(
	long _preId
	, long _sfxId
	, long _bgId
) {	preId = _preId;
	sfxId = _sfxId;
	bgId = _bgId;
}

public byte getMainOrder() { return (byte)18; }

public byte getSubOrder() { return (byte)2; }

/// <summary>
/// 空
/// </summary>
public long getPreId() { return preId; }
/// <summary>
/// 空
/// </summary>
public void setPreId(long _preId) { preId = _preId; }
/// <summary>
/// 空
/// </summary>
public long getSfxId() { return sfxId; }
/// <summary>
/// 空
/// </summary>
public void setSfxId(long _sfxId) { sfxId = _sfxId; }
/// <summary>
/// 空
/// </summary>
public long getBgId() { return bgId; }
/// <summary>
/// 空
/// </summary>
public void setBgId(long _bgId) { bgId = _bgId; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	preId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sfxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	bgId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(preId);
	_buf.putLong(sfxId);
	_buf.putLong(bgId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)2);
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
	builder.Append("preId").Append(":").Append(preId.ToString()).Append(", ");
	builder.Append("sfxId").Append(":").Append(sfxId.ToString()).Append(", ");
	builder.Append("bgId").Append(":").Append(bgId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

