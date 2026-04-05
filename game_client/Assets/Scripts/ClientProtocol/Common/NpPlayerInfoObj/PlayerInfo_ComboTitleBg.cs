using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpPlayerInfoObj
{

/// <summary>
/// 组合称号-称号底色
/// </summary>
public class PlayerInfo_ComboTitleBg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 称号底色
/// </summary>
private long bgId;
/// <summary>
/// 是否查看过
/// </summary>
private bool viewed;


public PlayerInfo_ComboTitleBg() {
	bgId = (long)0;
	viewed = false;
}

public PlayerInfo_ComboTitleBg(
	long _bgId
	, bool _viewed
) {	bgId = _bgId;
	viewed = _viewed;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 称号底色
/// </summary>
public long getBgId() { return bgId; }
/// <summary>
/// 称号底色
/// </summary>
public void setBgId(long _bgId) { bgId = _bgId; }
/// <summary>
/// 是否查看过
/// </summary>
public bool getViewed() { return viewed; }
/// <summary>
/// 是否查看过
/// </summary>
public void setViewed(bool _viewed) { viewed = _viewed; }


public int GetBufSize() {
	int _size = 9;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	bgId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	viewed = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(bgId);
	_buf.put(viewed?(byte)1:(byte)0);
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
	builder.Append("bgId").Append(":").Append(bgId.ToString()).Append(", ");
	builder.Append("viewed").Append(":").Append(viewed.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

