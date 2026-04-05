package GS2GC.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_037_001_RetDungeontGlobalSet implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> autoStartDungeonIdList;
/** 自动开启时间：小时 */
private int autoHour;
/** 自动开启时间：分钟 */
private int autoMin;


public GS2GC_037_001_RetDungeontGlobalSet() {
	autoStartDungeonIdList = new java.util.ArrayList<Long>();
	autoHour = 0;
	autoMin = 0;
}

public GS2GC_037_001_RetDungeontGlobalSet(
	 java.util.ArrayList<Long> _autoStartDungeonIdList
	, int _autoHour
	, int _autoMin
) {	autoStartDungeonIdList = _autoStartDungeonIdList;
	autoHour = _autoHour;
	autoMin = _autoMin;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)1; }

public java.util.ArrayList<Long> getAutoStartDungeonIdList() { return autoStartDungeonIdList; }
public void addAutoStartDungeonIdList(long _autoStartDungeonIdList) { autoStartDungeonIdList.add(_autoStartDungeonIdList); }
/** 自动开启时间：小时 */
public int getAutoHour() { return autoHour; }
/** 自动开启时间：小时 */
public void setAutoHour(int _autoHour) { autoHour = _autoHour; }
/** 自动开启时间：分钟 */
public int getAutoMin() { return autoMin; }
/** 自动开启时间：分钟 */
public void setAutoMin(int _autoMin) { autoMin = _autoMin; }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (autoStartDungeonIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (autoStartDungeonIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _autoStartDungeonIdListCount = _buf.getShort();
	for(int _i = 0; _i < _autoStartDungeonIdListCount; _i++) { 
		long _autoStartDungeonIdList = (long)0;
		if(_buf.remaining() > 0) _autoStartDungeonIdList = _buf.getLong();
		autoStartDungeonIdList.add(_autoStartDungeonIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) autoHour = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) autoMin = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)autoStartDungeonIdList.size());
	for(int _i = 0; _i < autoStartDungeonIdList.size(); _i++) { 
		_buf.putLong(autoStartDungeonIdList.get(_i));
	}
	_buf.putInt(autoHour);
	_buf.putInt(autoMin);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)1);
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

