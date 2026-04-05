package GC2GS.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 设置副本自动开启
 **/
public class GC2GS_037_002_ReqSetAutoStartDungeon implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> autoStartDungeonIdList;
private int hour;
private int min;


public GC2GS_037_002_ReqSetAutoStartDungeon() {
	autoStartDungeonIdList = new java.util.ArrayList<Long>();
	hour = 0;
	min = 0;
}

public GC2GS_037_002_ReqSetAutoStartDungeon(
	 java.util.ArrayList<Long> _autoStartDungeonIdList
	, int _hour
	, int _min
) {	autoStartDungeonIdList = _autoStartDungeonIdList;
	hour = _hour;
	min = _min;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)2; }

public java.util.ArrayList<Long> getAutoStartDungeonIdList() { return autoStartDungeonIdList; }
public void addAutoStartDungeonIdList(long _autoStartDungeonIdList) { autoStartDungeonIdList.add(_autoStartDungeonIdList); }
public int getHour() { return hour; }
public void setHour(int _hour) { hour = _hour; }
public int getMin() { return min; }
public void setMin(int _min) { min = _min; }


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
	if(_buf.remaining() > 0) hour = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) min = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)autoStartDungeonIdList.size());
	for(int _i = 0; _i < autoStartDungeonIdList.size(); _i++) { 
		_buf.putLong(autoStartDungeonIdList.get(_i));
	}
	_buf.putInt(hour);
	_buf.putInt(min);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)2);
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

