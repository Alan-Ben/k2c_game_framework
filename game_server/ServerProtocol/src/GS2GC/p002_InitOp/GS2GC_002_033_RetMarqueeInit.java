package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 请求跑马灯初始化
 **/
public class GS2GC_002_033_RetMarqueeInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 跑马灯展示位置信息列表 */
private java.util.ArrayList<Common.Common_MarqueeShowPosInfo> marqueeList;


public GS2GC_002_033_RetMarqueeInit() {
	marqueeList = new java.util.ArrayList<Common.Common_MarqueeShowPosInfo>();
}

public GS2GC_002_033_RetMarqueeInit(
	 java.util.ArrayList<Common.Common_MarqueeShowPosInfo> _marqueeList
) {	marqueeList = _marqueeList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)33; }

/** 跑马灯展示位置信息列表 */
public java.util.ArrayList<Common.Common_MarqueeShowPosInfo> getMarqueeList() { return marqueeList; }
/** 跑马灯展示位置信息列表 */
public void addMarqueeList(Common.Common_MarqueeShowPosInfo _marqueeList) { marqueeList.add(_marqueeList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < marqueeList.size(); _i++) {
	_size += 4 + marqueeList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < marqueeList.size(); _i++) {
	_size += 4 + marqueeList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _marqueeListCount = _buf.getShort();
	for(int _i = 0; _i < _marqueeListCount; _i++) { 
		Common.Common_MarqueeShowPosInfo _marqueeList = new Common.Common_MarqueeShowPosInfo();
		if(_buf.remaining() <= 0) return;
	int __marqueeListCustLen = _buf.getInt();
	int __marqueeListCurPos = _buf.position();
	_marqueeList.ReadUnzipBuf(_buf, __marqueeListCurPos + __marqueeListCustLen);
	_buf.position(__marqueeListCurPos + __marqueeListCustLen);

		marqueeList.add(_marqueeList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)marqueeList.size());
	for(int _i = 0; _i < marqueeList.size(); _i++) { 
		_buf.putInt(marqueeList.get(_i).GetBufSize());
	marqueeList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)33);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)33);
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

