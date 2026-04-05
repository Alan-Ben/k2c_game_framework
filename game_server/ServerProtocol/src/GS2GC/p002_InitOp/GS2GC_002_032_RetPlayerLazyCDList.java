package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 请求CD组件初始化
 **/
public class GS2GC_002_032_RetPlayerLazyCDList implements ALBasicProtocolPack._IALProtocolStructure {
/** cd数据列表 */
private java.util.ArrayList<NPCommon.NPCommon_PlayerLazyCD> cdList;


public GS2GC_002_032_RetPlayerLazyCDList() {
	cdList = new java.util.ArrayList<NPCommon.NPCommon_PlayerLazyCD>();
}

public GS2GC_002_032_RetPlayerLazyCDList(
	 java.util.ArrayList<NPCommon.NPCommon_PlayerLazyCD> _cdList
) {	cdList = _cdList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)32; }

/** cd数据列表 */
public java.util.ArrayList<NPCommon.NPCommon_PlayerLazyCD> getCdList() { return cdList; }
/** cd数据列表 */
public void addCdList(NPCommon.NPCommon_PlayerLazyCD _cdList) { cdList.add(_cdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (cdList.size() * 48);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (cdList.size() * 48);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _cdListCount = _buf.getShort();
	for(int _i = 0; _i < _cdListCount; _i++) { 
		NPCommon.NPCommon_PlayerLazyCD _cdList = new NPCommon.NPCommon_PlayerLazyCD();
		if(_buf.remaining() <= 0) return;
	int __cdListCustLen = _buf.getInt();
	int __cdListCurPos = _buf.position();
	_cdList.ReadUnzipBuf(_buf, __cdListCurPos + __cdListCustLen);
	_buf.position(__cdListCurPos + __cdListCustLen);

		cdList.add(_cdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)cdList.size());
	for(int _i = 0; _i < cdList.size(); _i++) { 
		_buf.putInt(cdList.get(_i).GetBufSize());
	cdList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)32);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)32);
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

