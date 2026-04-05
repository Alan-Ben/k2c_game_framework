using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星居民-信件数据
/// </summary>
public class Mars_Letter : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long id;
/// <summary>
/// 信件ID
/// </summary>
private long letterId;
private long npcId;
/// <summary>
/// 已处理
/// </summary>
private bool isDealed;


public Mars_Letter() {
	id = (long)0;
	letterId = (long)0;
	npcId = (long)0;
	isDealed = false;
}

public Mars_Letter(
	long _id
	, long _letterId
	, long _npcId
	, bool _isDealed
) {	id = _id;
	letterId = _letterId;
	npcId = _npcId;
	isDealed = _isDealed;
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
/// 信件ID
/// </summary>
public long getLetterId() { return letterId; }
/// <summary>
/// 信件ID
/// </summary>
public void setLetterId(long _letterId) { letterId = _letterId; }
public long getNpcId() { return npcId; }
public void setNpcId(long _npcId) { npcId = _npcId; }
/// <summary>
/// 已处理
/// </summary>
public bool getIsDealed() { return isDealed; }
/// <summary>
/// 已处理
/// </summary>
public void setIsDealed(bool _isDealed) { isDealed = _isDealed; }


public int GetBufSize() {
	int _size = 25;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 27;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	letterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	npcId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isDealed = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(letterId);
	_buf.putLong(npcId);
	_buf.put(isDealed?(byte)1:(byte)0);
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
	builder.Append("letterId").Append(":").Append(letterId.ToString()).Append(", ");
	builder.Append("npcId").Append(":").Append(npcId.ToString()).Append(", ");
	builder.Append("isDealed").Append(":").Append(isDealed.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

