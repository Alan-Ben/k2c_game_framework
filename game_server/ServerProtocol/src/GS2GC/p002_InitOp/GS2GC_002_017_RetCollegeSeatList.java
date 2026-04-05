package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_017_RetCollegeSeatList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.CollegeObj.College_SeatInfo> seatList;


public GS2GC_002_017_RetCollegeSeatList() {
	seatList = new java.util.ArrayList<Common.CollegeObj.College_SeatInfo>();
}

public GS2GC_002_017_RetCollegeSeatList(
	 java.util.ArrayList<Common.CollegeObj.College_SeatInfo> _seatList
) {	seatList = _seatList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)17; }

public java.util.ArrayList<Common.CollegeObj.College_SeatInfo> getSeatList() { return seatList; }
public void addSeatList(Common.CollegeObj.College_SeatInfo _seatList) { seatList.add(_seatList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (seatList.size() * 24);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (seatList.size() * 24);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _seatListCount = _buf.getShort();
	for(int _i = 0; _i < _seatListCount; _i++) { 
		Common.CollegeObj.College_SeatInfo _seatList = new Common.CollegeObj.College_SeatInfo();
		if(_buf.remaining() <= 0) return;
	int __seatListCustLen = _buf.getInt();
	int __seatListCurPos = _buf.position();
	_seatList.ReadUnzipBuf(_buf, __seatListCurPos + __seatListCustLen);
	_buf.position(__seatListCurPos + __seatListCustLen);

		seatList.add(_seatList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)seatList.size());
	for(int _i = 0; _i < seatList.size(); _i++) { 
		_buf.putInt(seatList.get(_i).GetBufSize());
	seatList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)17);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)17);
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

