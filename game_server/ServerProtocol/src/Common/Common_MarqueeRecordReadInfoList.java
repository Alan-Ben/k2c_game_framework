package Common;

import java.nio.ByteBuffer;
public class Common_MarqueeRecordReadInfoList implements ALBasicProtocolPack._IALProtocolStructure {
/** 已经读取的跑马灯信息列表 */
private java.util.ArrayList<Common.Common_MarqueeRecordReadInfo> infoList;


public Common_MarqueeRecordReadInfoList() {
	infoList = new java.util.ArrayList<Common.Common_MarqueeRecordReadInfo>();
}

public Common_MarqueeRecordReadInfoList(
	 java.util.ArrayList<Common.Common_MarqueeRecordReadInfo> _infoList
) {	infoList = _infoList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 已经读取的跑马灯信息列表 */
public java.util.ArrayList<Common.Common_MarqueeRecordReadInfo> getInfoList() { return infoList; }
/** 已经读取的跑马灯信息列表 */
public void addInfoList(Common.Common_MarqueeRecordReadInfo _infoList) { infoList.add(_infoList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < infoList.size(); _i++) {
	_size += 4 + infoList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < infoList.size(); _i++) {
	_size += 4 + infoList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _infoListCount = _buf.getShort();
	for(int _i = 0; _i < _infoListCount; _i++) { 
		Common.Common_MarqueeRecordReadInfo _infoList = new Common.Common_MarqueeRecordReadInfo();
		if(_buf.remaining() <= 0) return;
	int __infoListCustLen = _buf.getInt();
	int __infoListCurPos = _buf.position();
	_infoList.ReadUnzipBuf(_buf, __infoListCurPos + __infoListCustLen);
	_buf.position(__infoListCurPos + __infoListCustLen);

		infoList.add(_infoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)infoList.size());
	for(int _i = 0; _i < infoList.size(); _i++) { 
		_buf.putInt(infoList.get(_i).GetBufSize());
	infoList.get(_i).PutUnzipBuf(_buf);
	}
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

