package Common.GuildDungeonObj;

import java.nio.ByteBuffer;
/*********
 * 公会副本日志
 **/
public class GuildDungeon_Log implements ALBasicProtocolPack._IALProtocolStructure {
/** 日志类型 */
private Common.GuildDungeonEnum.EGuildDungeon_LogType logType;
/** 创建时间 */
private int createdAt;
/** 日志数据 */
private byte[] info;


public GuildDungeon_Log() {
	logType = Common.GuildDungeonEnum.EGuildDungeon_LogType.values()[0];
	createdAt = 0;
	info = null;
}

public GuildDungeon_Log(
	 Common.GuildDungeonEnum.EGuildDungeon_LogType _logType
	, int _createdAt
	, byte[] _info
) {	logType = _logType;
	createdAt = _createdAt;
	info = _info;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 日志类型 */
public Common.GuildDungeonEnum.EGuildDungeon_LogType getLogType() { return logType; }
/** 日志类型 */
public void setLogType(Common.GuildDungeonEnum.EGuildDungeon_LogType _logType) { logType = _logType; }
/** 创建时间 */
public int getCreatedAt() { return createdAt; }
/** 创建时间 */
public void setCreatedAt(int _createdAt) { createdAt = _createdAt; }
/** 日志数据 */
public byte[] getInfo() { return info; }
public java.nio.ByteBuffer get_buffer_Info() { if(null == info)return null; else return ByteBuffer.wrap(info); }

/** 日志数据 */
public void setInfo(byte[] _info) { info = _info; }
public void setInfo(java.nio.ByteBuffer _info) 
{
	if(null == _info){return;}
	int _oldPos = _info.position();
	int _bufLength = _info.remaining();
	info = new byte[_bufLength];
	_info.get(info);
	_info.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 8;
	_size += 4 + (info == null ? 0 : info.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (info == null ? 0 : info.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) logType = Common.GuildDungeonEnum.EGuildDungeon_LogType.EGuildDungeon_LogType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createdAt = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCount = _buf.getInt();
	if(0 < _infoCount){
		info = new byte[_infoCount];
		_buf.get(info);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(logType.ordinal());

	_buf.putInt(createdAt);
	_buf.putInt((info == null ? 0 : info.length));
	if(null != info){_buf.put(info);}

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

