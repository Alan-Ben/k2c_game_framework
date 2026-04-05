using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟宝箱数据
/// </summary>
public class Guild_BoxInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long id;
/// <summary>
/// 宝箱ID
/// </summary>
private long boxId;
/// <summary>
/// 分享玩家CID
/// </summary>
private long shareCid;
/// <summary>
/// 截至时间（毫秒）
/// </summary>
private long endMs;


public Guild_BoxInfo() {
	id = (long)0;
	boxId = (long)0;
	shareCid = (long)0;
	endMs = (long)0;
}

public Guild_BoxInfo(
	long _id
	, long _boxId
	, long _shareCid
	, long _endMs
) {	id = _id;
	boxId = _boxId;
	shareCid = _shareCid;
	endMs = _endMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 宝箱ID
/// </summary>
public long getBoxId() { return boxId; }
/// <summary>
/// 宝箱ID
/// </summary>
public void setBoxId(long _boxId) { boxId = _boxId; }
/// <summary>
/// 分享玩家CID
/// </summary>
public long getShareCid() { return shareCid; }
/// <summary>
/// 分享玩家CID
/// </summary>
public void setShareCid(long _shareCid) { shareCid = _shareCid; }
/// <summary>
/// 截至时间（毫秒）
/// </summary>
public long getEndMs() { return endMs; }
/// <summary>
/// 截至时间（毫秒）
/// </summary>
public void setEndMs(long _endMs) { endMs = _endMs; }


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
	boxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	shareCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(boxId);
	_buf.putLong(shareCid);
	_buf.putLong(endMs);
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
	builder.Append("boxId").Append(":").Append(boxId.ToString()).Append(", ");
	builder.Append("shareCid").Append(":").Append(shareCid.ToString()).Append(", ");
	builder.Append("endMs").Append(":").Append(endMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

