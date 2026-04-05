package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟事件
 **/
public class Guild_EventInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long dbId;
/** 事件类型 */
private Common.GuildEnum.EGuildEventType type;
/** 事件数据 */
private byte[] data;


public Guild_EventInfo() {
	dbId = (long)0;
	type = Common.GuildEnum.EGuildEventType.values()[0];
	data = null;
}

public Guild_EventInfo(
	 long _dbId
	, Common.GuildEnum.EGuildEventType _type
	, byte[] _data
) {	dbId = _dbId;
	type = _type;
	data = _data;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getDbId() { return dbId; }
public void setDbId(long _dbId) { dbId = _dbId; }
/** 事件类型 */
public Common.GuildEnum.EGuildEventType getType() { return type; }
/** 事件类型 */
public void setType(Common.GuildEnum.EGuildEventType _type) { type = _type; }
/** 事件数据 */
public byte[] getData() { return data; }
public java.nio.ByteBuffer get_buffer_Data() { if(null == data)return null; else return ByteBuffer.wrap(data); }

/** 事件数据 */
public void setData(byte[] _data) { data = _data; }
public void setData(java.nio.ByteBuffer _data) 
{
	if(null == _data){return;}
	int _oldPos = _data.position();
	int _bufLength = _data.remaining();
	data = new byte[_bufLength];
	_data.get(data);
	_data.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 12;
	_size += 4 + (data == null ? 0 : data.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + (data == null ? 0 : data.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.GuildEnum.EGuildEventType.EGuildEventType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _dataCount = _buf.getInt();
	if(0 < _dataCount){
		data = new byte[_dataCount];
		_buf.get(data);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putInt(type.ordinal());

	_buf.putInt((data == null ? 0 : data.length));
	if(null != data){_buf.put(data);}

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

