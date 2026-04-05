using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_064_OnGuildJoinRequestRemove : ALBasicProtocolPack._IALProtocolStructure {
private List<long> requestDbIdList;


public GS2GC_032_064_OnGuildJoinRequestRemove() {
	requestDbIdList = new List<long>();
}

public GS2GC_032_064_OnGuildJoinRequestRemove(
	List<long> _requestDbIdList
) {	requestDbIdList = _requestDbIdList;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)64; }

public List<long> getRequestDbIdList() { return requestDbIdList; }
public void addRequestDbIdList(long _requestDbIdList) { requestDbIdList.Add(_requestDbIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (requestDbIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (requestDbIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _requestDbIdListCount = _buf.getShort();
	for(int _i = 0; _i < _requestDbIdListCount; _i++) { 
		long _requestDbIdList = (long)0;
		_requestDbIdList = _buf.getLong();
		requestDbIdList.Add(_requestDbIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)requestDbIdList.Count);
	for(int _i = 0; _i < requestDbIdList.Count; _i++) { 
		_buf.putLong(requestDbIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)64);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)64);
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
	builder.Append("requestDbIdList").Append(":").Append(requestDbIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

