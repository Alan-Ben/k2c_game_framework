package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_082_RetGuildBoxInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 宝箱列表 */
private java.util.ArrayList<Common.GuildObj.Guild_BoxInfo> boxList;
private long activePoint;
private int targetLvl;
/** 分享宝箱匿名 */
private boolean isGuildBoxShareAnonymous;


public GS2GC_002_082_RetGuildBoxInit() {
	boxList = new java.util.ArrayList<Common.GuildObj.Guild_BoxInfo>();
	activePoint = (long)0;
	targetLvl = 0;
	isGuildBoxShareAnonymous = false;
}

public GS2GC_002_082_RetGuildBoxInit(
	 java.util.ArrayList<Common.GuildObj.Guild_BoxInfo> _boxList
	, long _activePoint
	, int _targetLvl
	, boolean _isGuildBoxShareAnonymous
) {	boxList = _boxList;
	activePoint = _activePoint;
	targetLvl = _targetLvl;
	isGuildBoxShareAnonymous = _isGuildBoxShareAnonymous;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)82; }

/** 宝箱列表 */
public java.util.ArrayList<Common.GuildObj.Guild_BoxInfo> getBoxList() { return boxList; }
/** 宝箱列表 */
public void addBoxList(Common.GuildObj.Guild_BoxInfo _boxList) { boxList.add(_boxList); }
public long getActivePoint() { return activePoint; }
public void setActivePoint(long _activePoint) { activePoint = _activePoint; }
public int getTargetLvl() { return targetLvl; }
public void setTargetLvl(int _targetLvl) { targetLvl = _targetLvl; }
/** 分享宝箱匿名 */
public boolean getIsGuildBoxShareAnonymous() { return isGuildBoxShareAnonymous; }
/** 分享宝箱匿名 */
public void setIsGuildBoxShareAnonymous(boolean _isGuildBoxShareAnonymous) { isGuildBoxShareAnonymous = _isGuildBoxShareAnonymous; }


public final int GetBufSize() {
	int _size = 13;
	_size += 2 + (boxList.size() * 36);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 15;
	_size += 2 + (boxList.size() * 36);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _boxListCount = _buf.getShort();
	for(int _i = 0; _i < _boxListCount; _i++) { 
		Common.GuildObj.Guild_BoxInfo _boxList = new Common.GuildObj.Guild_BoxInfo();
		if(_buf.remaining() <= 0) return;
	int __boxListCustLen = _buf.getInt();
	int __boxListCurPos = _buf.position();
	_boxList.ReadUnzipBuf(_buf, __boxListCurPos + __boxListCustLen);
	_buf.position(__boxListCurPos + __boxListCustLen);

		boxList.add(_boxList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activePoint = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) targetLvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isGuildBoxShareAnonymous = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)boxList.size());
	for(int _i = 0; _i < boxList.size(); _i++) { 
		_buf.putInt(boxList.get(_i).GetBufSize());
	boxList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(activePoint);
	_buf.putInt(targetLvl);
	_buf.put(isGuildBoxShareAnonymous?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)82);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)82);
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

