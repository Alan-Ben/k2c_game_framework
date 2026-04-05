package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 火星探险-PVP日志数据
 **/
public class ServerObj_MarsExplorePVPLog implements ALBasicProtocolPack._IALProtocolStructure {
/** 日志类型 */
private Common.MarsEnum.EMarsExplorePVPLogType logType;
/** 创建时间（毫秒） */
private long createdAt;
/** 日志数据 */
private byte[] logData;


public ServerObj_MarsExplorePVPLog() {
	logType = Common.MarsEnum.EMarsExplorePVPLogType.values()[0];
	createdAt = (long)0;
	logData = null;
}

public ServerObj_MarsExplorePVPLog(
	 Common.MarsEnum.EMarsExplorePVPLogType _logType
	, long _createdAt
	, byte[] _logData
) {	logType = _logType;
	createdAt = _createdAt;
	logData = _logData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 日志类型 */
public Common.MarsEnum.EMarsExplorePVPLogType getLogType() { return logType; }
/** 日志类型 */
public void setLogType(Common.MarsEnum.EMarsExplorePVPLogType _logType) { logType = _logType; }
/** 创建时间（毫秒） */
public long getCreatedAt() { return createdAt; }
/** 创建时间（毫秒） */
public void setCreatedAt(long _createdAt) { createdAt = _createdAt; }
/** 日志数据 */
public byte[] getLogData() { return logData; }
public java.nio.ByteBuffer get_buffer_LogData() { if(null == logData)return null; else return ByteBuffer.wrap(logData); }

/** 日志数据 */
public void setLogData(byte[] _logData) { logData = _logData; }
public void setLogData(java.nio.ByteBuffer _logData) 
{
	if(null == _logData){return;}
	int _oldPos = _logData.position();
	int _bufLength = _logData.remaining();
	logData = new byte[_bufLength];
	_logData.get(logData);
	_logData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 12;
	_size += 4 + (logData == null ? 0 : logData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + (logData == null ? 0 : logData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) logType = Common.MarsEnum.EMarsExplorePVPLogType.EMarsExplorePVPLogType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createdAt = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _logDataCount = _buf.getInt();
	if(0 < _logDataCount){
		logData = new byte[_logDataCount];
		_buf.get(logData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(logType.ordinal());

	_buf.putLong(createdAt);
	_buf.putInt((logData == null ? 0 : logData.length));
	if(null != logData){_buf.put(logData);}

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

