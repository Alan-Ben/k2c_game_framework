using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_GuildReqDonateCountInfo : ALBasicProtocolPack._IALProtocolStructure {
private long donateUid;
private short donateCount;


public Common_GuildReqDonateCountInfo() {
	donateUid = (long)0;
	donateCount = (short)0;
}

public Common_GuildReqDonateCountInfo(
	long _donateUid
	, short _donateCount
) {	donateUid = _donateUid;
	donateCount = _donateCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getDonateUid() { return donateUid; }
public void setDonateUid(long _donateUid) { donateUid = _donateUid; }
public short getDonateCount() { return donateCount; }
public void setDonateCount(short _donateCount) { donateCount = _donateCount; }


public int GetBufSize() {
	int _size = 10;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 12;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	donateUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	donateCount = _buf.getShort();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(donateUid);
	_buf.putShort(donateCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("donateUid").Append(":").Append(donateUid.ToString()).Append(", ");
	builder.Append("donateCount").Append(":").Append(donateCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

