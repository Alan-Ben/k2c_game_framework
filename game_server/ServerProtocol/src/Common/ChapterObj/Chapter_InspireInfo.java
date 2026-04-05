package Common.ChapterObj;

import java.nio.ByteBuffer;
/*********
 * 关卡鼓舞信息
 **/
public class Chapter_InspireInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 鼓舞信息列表 */
private java.util.ArrayList<Common.ChapterObj.Chapter_SingleInspireInfo> inspireList;


public Chapter_InspireInfo() {
	inspireList = new java.util.ArrayList<Common.ChapterObj.Chapter_SingleInspireInfo>();
}

public Chapter_InspireInfo(
	 java.util.ArrayList<Common.ChapterObj.Chapter_SingleInspireInfo> _inspireList
) {	inspireList = _inspireList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 鼓舞信息列表 */
public java.util.ArrayList<Common.ChapterObj.Chapter_SingleInspireInfo> getInspireList() { return inspireList; }
/** 鼓舞信息列表 */
public void addInspireList(Common.ChapterObj.Chapter_SingleInspireInfo _inspireList) { inspireList.add(_inspireList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (inspireList.size() * 12);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (inspireList.size() * 12);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _inspireListCount = _buf.getShort();
	for(int _i = 0; _i < _inspireListCount; _i++) { 
		Common.ChapterObj.Chapter_SingleInspireInfo _inspireList = new Common.ChapterObj.Chapter_SingleInspireInfo();
		if(_buf.remaining() <= 0) return;
	int __inspireListCustLen = _buf.getInt();
	int __inspireListCurPos = _buf.position();
	_inspireList.ReadUnzipBuf(_buf, __inspireListCurPos + __inspireListCustLen);
	_buf.position(__inspireListCurPos + __inspireListCustLen);

		inspireList.add(_inspireList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)inspireList.size());
	for(int _i = 0; _i < inspireList.size(); _i++) { 
		_buf.putInt(inspireList.get(_i).GetBufSize());
	inspireList.get(_i).PutUnzipBuf(_buf);
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

