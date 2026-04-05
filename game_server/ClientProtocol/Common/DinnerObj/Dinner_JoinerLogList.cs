using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DinnerObj
{

/// <summary>
/// 宴会玩家交互记录
/// </summary>
public class Dinner_JoinerLogList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家CID
/// </summary>
private long cid;
/// <summary>
/// 己方参加玩家宴会次数
/// </summary>
private int joinedCount;
/// <summary>
/// 玩家参加己方宴会次数
/// </summary>
private int beJoinedCount;


public Dinner_JoinerLogList() {
	cid = (long)0;
	joinedCount = 0;
	beJoinedCount = 0;
}

public Dinner_JoinerLogList(
	long _cid
	, int _joinedCount
	, int _beJoinedCount
) {	cid = _cid;
	joinedCount = _joinedCount;
	beJoinedCount = _beJoinedCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 玩家CID
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家CID
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 己方参加玩家宴会次数
/// </summary>
public int getJoinedCount() { return joinedCount; }
/// <summary>
/// 己方参加玩家宴会次数
/// </summary>
public void setJoinedCount(int _joinedCount) { joinedCount = _joinedCount; }
/// <summary>
/// 玩家参加己方宴会次数
/// </summary>
public int getBeJoinedCount() { return beJoinedCount; }
/// <summary>
/// 玩家参加己方宴会次数
/// </summary>
public void setBeJoinedCount(int _beJoinedCount) { beJoinedCount = _beJoinedCount; }


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
	joinedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	beJoinedCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putInt(joinedCount);
	_buf.putInt(beJoinedCount);
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
	builder.Append("joinedCount").Append(":").Append(joinedCount.ToString()).Append(", ");
	builder.Append("beJoinedCount").Append(":").Append(beJoinedCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

