package Common;

import java.nio.ByteBuffer;
public class Common_MarqueeShowPosInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int showPosId;
private java.util.ArrayList<Common.Common_MarqueeInfo> marqueeList;


public Common_MarqueeShowPosInfo() {
	showPosId = 0;
	marqueeList = new java.util.ArrayList<Common.Common_MarqueeInfo>();
}

public Common_MarqueeShowPosInfo(
	 int _showPosId
	, java.util.ArrayList<Common.Common_MarqueeInfo> _marqueeList
) {	showPosId = _showPosId;
	marqueeList = _marqueeList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getShowPosId() { return showPosId; }
public void setShowPosId(int _showPosId) { showPosId = _showPosId; }
public java.util.ArrayList<Common.Common_MarqueeInfo> getMarqueeList() { return marqueeList; }
public void addMarqueeList(Common.Common_MarqueeInfo _marqueeList) { marqueeList.add(_marqueeList); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2;
	for(int _i = 0; _i < marqueeList.size(); _i++) {
	_size += 4 + marqueeList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2;
	for(int _i = 0; _i < marqueeList.size(); _i++) {
	_size += 4 + marqueeList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) showPosId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _marqueeListCount = _buf.getShort();
	for(int _i = 0; _i < _marqueeListCount; _i++) { 
		Common.Common_MarqueeInfo _marqueeList = new Common.Common_MarqueeInfo();
		if(_buf.remaining() <= 0) return;
	int __marqueeListCustLen = _buf.getInt();
	int __marqueeListCurPos = _buf.position();
	_marqueeList.ReadUnzipBuf(_buf, __marqueeListCurPos + __marqueeListCustLen);
	_buf.position(__marqueeListCurPos + __marqueeListCustLen);

		marqueeList.add(_marqueeList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(showPosId);
	_buf.putShort((short)marqueeList.size());
	for(int _i = 0; _i < marqueeList.size(); _i++) { 
		_buf.putInt(marqueeList.get(_i).GetBufSize());
	marqueeList.get(_i).PutUnzipBuf(_buf);
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

