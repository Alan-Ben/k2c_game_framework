using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPLS2GC.p001_BasicOp
{

public class NPLS2GC_001_001_RetBasicInfo : ALBasicProtocolPack._IALProtocolStructure {
private string uid;
private int countryIdx;


public NPLS2GC_001_001_RetBasicInfo() {
	uid = "";
	countryIdx = 0;
}

public NPLS2GC_001_001_RetBasicInfo(
	string _uid
	, int _countryIdx
) {	uid = _uid;
	countryIdx = _countryIdx;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)1; }

public string getUid() { return uid; }
public void setUid(string _uid) { uid = _uid; }
public int getCountryIdx() { return countryIdx; }
public void setCountryIdx(int _countryIdx) { countryIdx = _countryIdx; }


public int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uid = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	countryIdx = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(uid);
	_buf.putInt(countryIdx);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)1);
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
	builder.Append("uid").Append(":").Append(uid.ToString()).Append(", ");
	builder.Append("countryIdx").Append(":").Append(countryIdx.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

