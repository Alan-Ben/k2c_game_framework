using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p040_MarsPeopleOp
{

/// <summary>
/// 人口数量变化
/// </summary>
public class GS2GC_040_050_OnMarsPeopleNumChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 居民数量
/// </summary>
private Common.MarsObj.Mars_PeopleNum peopleNum;


public GS2GC_040_050_OnMarsPeopleNumChg() {
	peopleNum = new Common.MarsObj.Mars_PeopleNum();
}

public GS2GC_040_050_OnMarsPeopleNumChg(
	Common.MarsObj.Mars_PeopleNum _peopleNum
) {	peopleNum = _peopleNum;
}

public byte getMainOrder() { return (byte)40; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 居民数量
/// </summary>
public Common.MarsObj.Mars_PeopleNum getPeopleNum() { return peopleNum; }
/// <summary>
/// 居民数量
/// </summary>
public void setPeopleNum(Common.MarsObj.Mars_PeopleNum _peopleNum) { peopleNum = _peopleNum; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _peopleNumCustLen = _buf.getInt();
	int _peopleNumCurPos = _buf.getCurPos();
	peopleNum.ReadUnzipBuf(_buf, _peopleNumCurPos + _peopleNumCustLen);
	_buf.setPosition(_peopleNumCurPos + _peopleNumCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(peopleNum.GetBufSize());
	peopleNum.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)50);
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
	builder.Append("peopleNum").Append(":").Append(peopleNum == null ? "null" : peopleNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

