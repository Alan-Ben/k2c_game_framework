using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟分享矿信息
/// </summary>
public class Guild_MineShareInfo : ALBasicProtocolPack._IALProtocolStructure {
private long id;
/// <summary>
/// 矿实例ID
/// </summary>
private long mineInstanceId;
/// <summary>
/// 分享者CID
/// </summary>
private long finderCid;
/// <summary>
/// 矿有效期毫秒时间戳
/// </summary>
private long mineEndShowMs;


public Guild_MineShareInfo() {
	id = (long)0;
	mineInstanceId = (long)0;
	finderCid = (long)0;
	mineEndShowMs = (long)0;
}

public Guild_MineShareInfo(
	long _id
	, long _mineInstanceId
	, long _finderCid
	, long _mineEndShowMs
) {	id = _id;
	mineInstanceId = _mineInstanceId;
	finderCid = _finderCid;
	mineEndShowMs = _mineEndShowMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
/// <summary>
/// 矿实例ID
/// </summary>
public long getMineInstanceId() { return mineInstanceId; }
/// <summary>
/// 矿实例ID
/// </summary>
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }
/// <summary>
/// 分享者CID
/// </summary>
public long getFinderCid() { return finderCid; }
/// <summary>
/// 分享者CID
/// </summary>
public void setFinderCid(long _finderCid) { finderCid = _finderCid; }
/// <summary>
/// 矿有效期毫秒时间戳
/// </summary>
public long getMineEndShowMs() { return mineEndShowMs; }
/// <summary>
/// 矿有效期毫秒时间戳
/// </summary>
public void setMineEndShowMs(long _mineEndShowMs) { mineEndShowMs = _mineEndShowMs; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mineInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	finderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mineEndShowMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(mineInstanceId);
	_buf.putLong(finderCid);
	_buf.putLong(mineEndShowMs);
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
	builder.Append("mineInstanceId").Append(":").Append(mineInstanceId.ToString()).Append(", ");
	builder.Append("finderCid").Append(":").Append(finderCid.ToString()).Append(", ");
	builder.Append("mineEndShowMs").Append(":").Append(mineEndShowMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

