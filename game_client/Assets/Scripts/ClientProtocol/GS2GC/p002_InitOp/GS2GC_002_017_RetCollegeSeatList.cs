using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_017_RetCollegeSeatList : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.CollegeObj.College_SeatInfo> seatList;


public GS2GC_002_017_RetCollegeSeatList() {
	seatList = new List<Common.CollegeObj.College_SeatInfo>();
}

public GS2GC_002_017_RetCollegeSeatList(
	List<Common.CollegeObj.College_SeatInfo> _seatList
) {	seatList = _seatList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)17; }

public List<Common.CollegeObj.College_SeatInfo> getSeatList() { return seatList; }
public void addSeatList(Common.CollegeObj.College_SeatInfo _seatList) { seatList.Add(_seatList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (seatList.Count * 24);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (seatList.Count * 24);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _seatListCount = _buf.getShort();
	for(int _i = 0; _i < _seatListCount; _i++) { 
		Common.CollegeObj.College_SeatInfo _seatList = new Common.CollegeObj.College_SeatInfo();
		int __seatListCustLen = _buf.getInt();
	int __seatListCurPos = _buf.getCurPos();
	_seatList.ReadUnzipBuf(_buf, __seatListCurPos + __seatListCustLen);
	_buf.setPosition(__seatListCurPos + __seatListCustLen);

		seatList.Add(_seatList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)seatList.Count);
	for(int _i = 0; _i < seatList.Count; _i++) { 
		_buf.putInt(seatList[_i].GetBufSize());
	seatList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)17);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)17);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("seatList").Append(":").Append(seatList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

