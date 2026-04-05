using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_007_RetGetOnlineCidList : ALBasicProtocolPack._IALProtocolStructure {
private List<long> cidList;


public GS2GC_007_007_RetGetOnlineCidList() {
	cidList = new List<long>();
}

public GS2GC_007_007_RetGetOnlineCidList(
	List<long> _cidList
) {	cidList = _cidList;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)7; }

public List<long> getCidList() { return cidList; }
public void addCidList(long _cidList) { cidList.Add(_cidList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (cidList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (cidList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _cidListCount = _buf.getShort();
	for(int _i = 0; _i < _cidListCount; _i++) { 
		long _cidList = (long)0;
		_cidList = _buf.getLong();
		cidList.Add(_cidList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)cidList.Count);
	for(int _i = 0; _i < cidList.Count; _i++) { 
		_buf.putLong(cidList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)7);
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
	builder.Append("cidList").Append(":").Append(cidList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

