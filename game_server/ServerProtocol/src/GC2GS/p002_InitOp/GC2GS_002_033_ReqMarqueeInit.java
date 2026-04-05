package GC2GS.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 请求跑马灯初始化
 **/
public class GC2GS_002_033_ReqMarqueeInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 跑马灯展示位置读取信息列表 */
private java.util.ArrayList<Common.Common_MarqueeShowPosReadInfo> marqueeReadList;


public GC2GS_002_033_ReqMarqueeInit() {
	marqueeReadList = new java.util.ArrayList<Common.Common_MarqueeShowPosReadInfo>();
}

public GC2GS_002_033_ReqMarqueeInit(
	 java.util.ArrayList<Common.Common_MarqueeShowPosReadInfo> _marqueeReadList
) {	marqueeReadList = _marqueeReadList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)33; }

/** 跑马灯展示位置读取信息列表 */
public java.util.ArrayList<Common.Common_MarqueeShowPosReadInfo> getMarqueeReadList() { return marqueeReadList; }
/** 跑马灯展示位置读取信息列表 */
public void addMarqueeReadList(Common.Common_MarqueeShowPosReadInfo _marqueeReadList) { marqueeReadList.add(_marqueeReadList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (marqueeReadList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (marqueeReadList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _marqueeReadListCount = _buf.getShort();
	for(int _i = 0; _i < _marqueeReadListCount; _i++) { 
		Common.Common_MarqueeShowPosReadInfo _marqueeReadList = new Common.Common_MarqueeShowPosReadInfo();
		if(_buf.remaining() <= 0) return;
	int __marqueeReadListCustLen = _buf.getInt();
	int __marqueeReadListCurPos = _buf.position();
	_marqueeReadList.ReadUnzipBuf(_buf, __marqueeReadListCurPos + __marqueeReadListCustLen);
	_buf.position(__marqueeReadListCurPos + __marqueeReadListCustLen);

		marqueeReadList.add(_marqueeReadList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)marqueeReadList.size());
	for(int _i = 0; _i < marqueeReadList.size(); _i++) { 
		_buf.putInt(marqueeReadList.get(_i).GetBufSize());
	marqueeReadList.get(_i).PutUnzipBuf(_buf);
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

