package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_079_RetActivityHotRefList implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动热更配置列表 */
private java.util.ArrayList<Common.ActivityObj.Activity_HotRefInfo> hotRefList;


public GS2GC_002_079_RetActivityHotRefList() {
	hotRefList = new java.util.ArrayList<Common.ActivityObj.Activity_HotRefInfo>();
}

public GS2GC_002_079_RetActivityHotRefList(
	 java.util.ArrayList<Common.ActivityObj.Activity_HotRefInfo> _hotRefList
) {	hotRefList = _hotRefList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)79; }

/** 活动热更配置列表 */
public java.util.ArrayList<Common.ActivityObj.Activity_HotRefInfo> getHotRefList() { return hotRefList; }
/** 活动热更配置列表 */
public void addHotRefList(Common.ActivityObj.Activity_HotRefInfo _hotRefList) { hotRefList.add(_hotRefList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < hotRefList.size(); _i++) {
	_size += 4 + hotRefList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < hotRefList.size(); _i++) {
	_size += 4 + hotRefList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hotRefListCount = _buf.getShort();
	for(int _i = 0; _i < _hotRefListCount; _i++) { 
		Common.ActivityObj.Activity_HotRefInfo _hotRefList = new Common.ActivityObj.Activity_HotRefInfo();
		if(_buf.remaining() <= 0) return;
	int __hotRefListCustLen = _buf.getInt();
	int __hotRefListCurPos = _buf.position();
	_hotRefList.ReadUnzipBuf(_buf, __hotRefListCurPos + __hotRefListCustLen);
	_buf.position(__hotRefListCurPos + __hotRefListCustLen);

		hotRefList.add(_hotRefList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)hotRefList.size());
	for(int _i = 0; _i < hotRefList.size(); _i++) { 
		_buf.putInt(hotRefList.get(_i).GetBufSize());
	hotRefList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)79);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)79);
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

