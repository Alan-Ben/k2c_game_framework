package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星探索-PVP日志索引数据
 **/
public class Mars_ExplorePVPLogIdx implements ALBasicProtocolPack._IALProtocolStructure {
/** 日志实力ID */
private long id;
/** 日志类型 */
private Common.MarsEnum.EMarsExplorePVPLogType logType;
/** 创建时间（毫秒） */
private long createdAt;
/** 额外数据 */
private byte[] exData;


public Mars_ExplorePVPLogIdx() {
	id = (long)0;
	logType = Common.MarsEnum.EMarsExplorePVPLogType.values()[0];
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 日志实力ID */
public long getId() { return id; }
/** 日志实力ID */
public void setId(long _id) { id = _id; }
/** 日志类型 */
public Common.MarsEnum.EMarsExplorePVPLogType getLogType() { return logType; }
/** 日志类型 */
public void setLogType(Common.MarsEnum.EMarsExplorePVPLogType _logType) { logType = _logType; }
/** 创建时间（毫秒） */
public long getCreatedAt() { return createdAt; }
/** 创建时间（毫秒） */
public void setCreatedAt(long _createdAt) { createdAt = _createdAt; }
/** 额外数据 */
public byte[] getExData() { return exData; }
public java.nio.ByteBuffer get_buffer_ExData() { if(null == exData)return null; else return ByteBuffer.wrap(exData); }

/** 额外数据 */
public void setExData(byte[] _exData) { exData = _exData; }
public void setExData(java.nio.ByteBuffer _exData) 
{
	if(null == _exData){return;}
	int _oldPos = _exData.position();
	int _bufLength = _exData.remaining();
	exData = new byte[_bufLength];
	_exData.get(exData);
	_exData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 20;
	_size += 4 + (exData == null ? 0 : exData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += 4 + (exData == null ? 0 : exData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) logType = Common.MarsEnum.EMarsExplorePVPLogType.EMarsExplorePVPLogType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createdAt = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _exDataCount = _buf.getInt();
	if(0 < _exDataCount){
		exData = new byte[_exDataCount];
		_buf.get(exData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putInt(logType.ordinal());

	_buf.putLong(createdAt);
	_buf.putInt((exData == null ? 0 : exData.length));
	if(null != exData){_buf.put(exData);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

