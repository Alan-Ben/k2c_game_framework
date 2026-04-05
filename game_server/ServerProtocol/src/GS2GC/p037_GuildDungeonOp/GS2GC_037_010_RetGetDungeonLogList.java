package GS2GC.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_037_010_RetGetDungeonLogList implements ALBasicProtocolPack._IALProtocolStructure {
/** 副本日志数据 */
private java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_Log> logList;


public GS2GC_037_010_RetGetDungeonLogList() {
	logList = new java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_Log>();
}

public GS2GC_037_010_RetGetDungeonLogList(
	 java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_Log> _logList
) {	logList = _logList;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)10; }

/** 副本日志数据 */
public java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_Log> getLogList() { return logList; }
/** 副本日志数据 */
public void addLogList(Common.GuildDungeonObj.GuildDungeon_Log _logList) { logList.add(_logList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < logList.size(); _i++) {
	_size += 4 + logList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < logList.size(); _i++) {
	_size += 4 + logList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _logListCount = _buf.getShort();
	for(int _i = 0; _i < _logListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_Log _logList = new Common.GuildDungeonObj.GuildDungeon_Log();
		if(_buf.remaining() <= 0) return;
	int __logListCustLen = _buf.getInt();
	int __logListCurPos = _buf.position();
	_logList.ReadUnzipBuf(_buf, __logListCurPos + __logListCustLen);
	_buf.position(__logListCurPos + __logListCustLen);

		logList.add(_logList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)logList.size());
	for(int _i = 0; _i < logList.size(); _i++) { 
		_buf.putInt(logList.get(_i).GetBufSize());
	logList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)10);
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

