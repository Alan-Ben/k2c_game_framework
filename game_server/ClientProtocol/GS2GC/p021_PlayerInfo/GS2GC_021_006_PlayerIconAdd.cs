using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

public class GS2GC_021_006_PlayerIconAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 头像唯一Id
/// </summary>
private long id;
/// <summary>
/// 超时时间标记，单位秒。-1表示永久
/// </summary>
private int expireTimeTagS;
/// <summary>
/// 是否查看过
/// </summary>
private bool viewed;


public GS2GC_021_006_PlayerIconAdd() {
	id = (long)0;
	expireTimeTagS = 0;
	viewed = false;
}

public GS2GC_021_006_PlayerIconAdd(
	long _id
	, int _expireTimeTagS
	, bool _viewed
) {	id = _id;
	expireTimeTagS = _expireTimeTagS;
	viewed = _viewed;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 头像唯一Id
/// </summary>
public long getId() { return id; }
/// <summary>
/// 头像唯一Id
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 超时时间标记，单位秒。-1表示永久
/// </summary>
public int getExpireTimeTagS() { return expireTimeTagS; }
/// <summary>
/// 超时时间标记，单位秒。-1表示永久
/// </summary>
public void setExpireTimeTagS(int _expireTimeTagS) { expireTimeTagS = _expireTimeTagS; }
/// <summary>
/// 是否查看过
/// </summary>
public bool getViewed() { return viewed; }
/// <summary>
/// 是否查看过
/// </summary>
public void setViewed(bool _viewed) { viewed = _viewed; }


public int GetBufSize() {
	int _size = 13;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expireTimeTagS = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	viewed = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putInt(expireTimeTagS);
	_buf.put(viewed?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("expireTimeTagS").Append(":").Append(expireTimeTagS.ToString()).Append(", ");
	builder.Append("viewed").Append(":").Append(viewed.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

