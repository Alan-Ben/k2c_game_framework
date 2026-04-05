package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 火星科技数据初始化
 **/
public class GS2GC_002_078_RetMarsTechInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 科技列表 */
private java.util.ArrayList<Common.MarsObj.Mars_Technology> technologyList;


public GS2GC_002_078_RetMarsTechInit() {
	technologyList = new java.util.ArrayList<Common.MarsObj.Mars_Technology>();
}

public GS2GC_002_078_RetMarsTechInit(
	 java.util.ArrayList<Common.MarsObj.Mars_Technology> _technologyList
) {	technologyList = _technologyList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)78; }

/** 科技列表 */
public java.util.ArrayList<Common.MarsObj.Mars_Technology> getTechnologyList() { return technologyList; }
/** 科技列表 */
public void addTechnologyList(Common.MarsObj.Mars_Technology _technologyList) { technologyList.add(_technologyList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (technologyList.size() * 49);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (technologyList.size() * 49);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _technologyListCount = _buf.getShort();
	for(int _i = 0; _i < _technologyListCount; _i++) { 
		Common.MarsObj.Mars_Technology _technologyList = new Common.MarsObj.Mars_Technology();
		if(_buf.remaining() <= 0) return;
	int __technologyListCustLen = _buf.getInt();
	int __technologyListCurPos = _buf.position();
	_technologyList.ReadUnzipBuf(_buf, __technologyListCurPos + __technologyListCustLen);
	_buf.position(__technologyListCurPos + __technologyListCustLen);

		technologyList.add(_technologyList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)technologyList.size());
	for(int _i = 0; _i < technologyList.size(); _i++) { 
		_buf.putInt(technologyList.get(_i).GetBufSize());
	technologyList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)78);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)78);
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

