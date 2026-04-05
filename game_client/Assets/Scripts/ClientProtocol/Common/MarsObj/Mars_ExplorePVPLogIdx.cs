using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星探索-PVP日志索引数据
/// </summary>
public class Mars_ExplorePVPLogIdx : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 日志实力ID
/// </summary>
private long id;
/// <summary>
/// 日志类型
/// </summary>
private Common.MarsEnum.EMarsExplorePVPLogType logType;
/// <summary>
/// 创建时间（毫秒）
/// </summary>
private long createdAt;
/// <summary>
/// 额外数据
/// </summary>
private byte[] exData;


public Mars_ExplorePVPLogIdx() {
	id = (long)0;
	logType = 0;
	createdAt = (long)0;
	exData = null;
}

public Mars_ExplorePVPLogIdx(
	long _id
	, Common.MarsEnum.EMarsExplorePVPLogType _logType
	, long _createdAt
	, byte[] _exData
) {	id = _id;
	logType = _logType;
	createdAt = _createdAt;
	exData = _exData;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 日志实力ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 日志实力ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 日志类型
/// </summary>
public Common.MarsEnum.EMarsExplorePVPLogType getLogType() { return logType; }
/// <summary>
/// 日志类型
/// </summary>
public void setLogType(Common.MarsEnum.EMarsExplorePVPLogType _logType) { logType = _logType; }
/// <summary>
/// 创建时间（毫秒）
/// </summary>
public long getCreatedAt() { return createdAt; }
/// <summary>
/// 创建时间（毫秒）
/// </summary>
public void setCreatedAt(long _createdAt) { createdAt = _createdAt; }
/// <summary>
/// 额外数据
/// </summary>
public byte[] getExData() { return exData; }

/// <summary>
/// 额外数据
/// </summary>
public void setExData(byte[] _exData) { exData = _exData; }



public int GetBufSize() {
	int _size = 20;
	_size += 4 + (exData == null ? 0 : exData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;
	_size += 4 + (exData == null ? 0 : exData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	logType = (Common.MarsEnum.EMarsExplorePVPLogType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createdAt = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putInt((int)logType);

	_buf.putLong(createdAt);
	_buf.putByteBuffer(exData);

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
	builder.Append("logType").Append(":").Append(logType.ToString()).Append(", ");
	builder.Append("createdAt").Append(":").Append(createdAt.ToString()).Append(", ");
	builder.Append("exData").Append(":").Append(exData == null ? "null" : exData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

