using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpPlayerInfoObj
{

/// <summary>
/// 组合称号
/// </summary>
public class PlayerInfo_ComboTitle : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 称号前缀
/// </summary>
private long preId;
/// <summary>
/// 称号后缀
/// </summary>
private long sfxId;
/// <summary>
/// 称号底色
/// </summary>
private long bgId;


public PlayerInfo_ComboTitle() {
	preId = (long)0;
	sfxId = (long)0;
	bgId = (long)0;
}

public PlayerInfo_ComboTitle(
	long _preId
	, long _sfxId
	, long _bgId
) {	preId = _preId;
	sfxId = _sfxId;
	bgId = _bgId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 称号前缀
/// </summary>
public long getPreId() { return preId; }
/// <summary>
/// 称号前缀
/// </summary>
public void setPreId(long _preId) { preId = _preId; }
/// <summary>
/// 称号后缀
/// </summary>
public long getSfxId() { return sfxId; }
/// <summary>
/// 称号后缀
/// </summary>
public void setSfxId(long _sfxId) { sfxId = _sfxId; }
/// <summary>
/// 称号底色
/// </summary>
public long getBgId() { return bgId; }
/// <summary>
/// 称号底色
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
	builder.Append("preId").Append(":").Append(preId.ToString()).Append(", ");
	builder.Append("sfxId").Append(":").Append(sfxId.ToString()).Append(", ");
	builder.Append("bgId").Append(":").Append(bgId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

