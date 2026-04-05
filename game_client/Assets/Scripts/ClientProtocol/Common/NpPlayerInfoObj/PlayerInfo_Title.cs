using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpPlayerInfoObj
{

/// <summary>
/// 普通称号
/// </summary>
public class PlayerInfo_Title : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 称号唯一Id
/// </summary>
private long id;
/// <summary>
/// 超时时间标记，单位秒。0表示永久
/// </summary>
private int expireTimeTagS;
/// <summary>
/// 是否查看过
/// </summary>
private bool viewed;
/// <summary>
/// 最近一次获得时间
/// </summary>
private int lastGainTs;
/// <summary>
/// 获得次数
/// </summary>
private int gainCount;


public PlayerInfo_Title() {
	id = (long)0;
	expireTimeTagS = 0;
	viewed = false;
	lastGainTs = 0;
	gainCount = 0;
}

public PlayerInfo_Title(
	long _id
	, int _expireTimeTagS
	, bool _viewed
	, int _lastGainTs
	, int _gainCount
) {	id = _id;
	expireTimeTagS = _expireTimeTagS;
	viewed = _viewed;
	lastGainTs = _lastGainTs;
	gainCount = _gainCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 称号唯一Id
/// </summary>
public long getId() { return id; }
/// <summary>
/// 称号唯一Id
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 超时时间标记，单位秒。0表示永久
/// </summary>
public int getExpireTimeTagS() { return expireTimeTagS; }
/// <summary>
/// 超时时间标记，单位秒。0表示永久
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
/// <summary>
/// 最近一次获得时间
/// </summary>
public int getLastGainTs() { return lastGainTs; }
/// <summary>
/// 最近一次获得时间
/// </summary>
public void setLastGainTs(int _lastGainTs) { lastGainTs = _lastGainTs; }
/// <summary>
/// 获得次数
/// </summary>
public int getGainCount() { return gainCount; }
/// <summary>
/// 获得次数
/// </summary>
public void setGainCount(int _gainCount) { gainCount = _gainCount; }


public int GetBufSize() {
	int _size = 21;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expireTimeTagS = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	viewed = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastGainTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putInt(expireTimeTagS);
	_buf.put(viewed?(byte)1:(byte)0);
	_buf.putInt(lastGainTs);
	_buf.putInt(gainCount);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("expireTimeTagS").Append(":").Append(expireTimeTagS.ToString()).Append(", ");
	builder.Append("viewed").Append(":").Append(viewed.ToString()).Append(", ");
	builder.Append("lastGainTs").Append(":").Append(lastGainTs.ToString()).Append(", ");
	builder.Append("gainCount").Append(":").Append(gainCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

