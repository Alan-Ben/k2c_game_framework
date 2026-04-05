using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DungeonObj
{

/// <summary>
/// 晚间副本击杀信息
/// </summary>
public class EveningDungeon_DefeatInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家ID
/// </summary>
private long cid;
/// <summary>
/// 击杀时间戳
/// </summary>
private long defeatTimeMs;


public EveningDungeon_DefeatInfo() {
	cid = (long)0;
	defeatTimeMs = (long)0;
}

public EveningDungeon_DefeatInfo(
	long _cid
	, long _defeatTimeMs
) {	cid = _cid;
	defeatTimeMs = _defeatTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 玩家ID
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家ID
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 击杀时间戳
/// </summary>
public long getDefeatTimeMs() { return defeatTimeMs; }
/// <summary>
/// 击杀时间戳
/// </summary>
public void setDefeatTimeMs(long _defeatTimeMs) { defeatTimeMs = _defeatTimeMs; }


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
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	defeatTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putLong(defeatTimeMs);
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
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("defeatTimeMs").Append(":").Append(defeatTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

