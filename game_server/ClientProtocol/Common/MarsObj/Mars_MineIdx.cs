using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星探索-矿产数据索引
/// </summary>
public class Mars_MineIdx : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long id;
/// <summary>
/// 配置ID
/// </summary>
private long refId;
/// <summary>
/// 位置ID
/// </summary>
private long pos;
/// <summary>
/// 结束展示时间（毫秒）
/// </summary>
private long endShowMs;
/// <summary>
/// 开始展示时间（毫秒）
/// </summary>
private long startShowMs;


public Mars_MineIdx() {
	id = (long)0;
	refId = (long)0;
	pos = (long)0;
	endShowMs = (long)0;
	startShowMs = (long)0;
}

public Mars_MineIdx(
	long _id
	, long _refId
	, long _pos
	, long _endShowMs
	, long _startShowMs
) {	id = _id;
	refId = _refId;
	pos = _pos;
	endShowMs = _endShowMs;
	startShowMs = _startShowMs;
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
/// 配置ID
/// </summary>
public long getRefId() { return refId; }
/// <summary>
/// 配置ID
/// </summary>
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// 位置ID
/// </summary>
public long getPos() { return pos; }
/// <summary>
/// 位置ID
/// </summary>
public void setPos(long _pos) { pos = _pos; }
/// <summary>
/// 结束展示时间（毫秒）
/// </summary>
public long getEndShowMs() { return endShowMs; }
/// <summary>
/// 结束展示时间（毫秒）
/// </summary>
public void setEndShowMs(long _endShowMs) { endShowMs = _endShowMs; }
/// <summary>
/// 开始展示时间（毫秒）
/// </summary>
public long getStartShowMs() { return startShowMs; }
/// <summary>
/// 开始展示时间（毫秒）
/// </summary>
public void setStartShowMs(long _startShowMs) { startShowMs = _startShowMs; }


public int GetBufSize() {
	int _size = 40;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pos = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endShowMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startShowMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(refId);
	_buf.putLong(pos);
	_buf.putLong(endShowMs);
	_buf.putLong(startShowMs);
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
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("pos").Append(":").Append(pos.ToString()).Append(", ");
	builder.Append("endShowMs").Append(":").Append(endShowMs.ToString()).Append(", ");
	builder.Append("startShowMs").Append(":").Append(startShowMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

