using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ChildObj
{

/// <summary>
/// 子嗣（成年已婚）数据
/// </summary>
public class Adult_MarriedInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 子嗣数据
/// </summary>
private Common.ChildObj.Adult_Info adult;
/// <summary>
/// 结婚对象玩家CID
/// </summary>
private long marriedCid;
/// <summary>
/// 结婚对象子嗣数据
/// </summary>
private Common.ChildObj.Adult_Info marriedAdult;
/// <summary>
/// 结婚时间（秒）
/// </summary>
private int marriedTs;


public Adult_MarriedInfo() {
	adult = new Common.ChildObj.Adult_Info();
	marriedCid = (long)0;
	marriedAdult = new Common.ChildObj.Adult_Info();
	marriedTs = 0;
}

public Adult_MarriedInfo(
	Common.ChildObj.Adult_Info _adult
	, long _marriedCid
	, Common.ChildObj.Adult_Info _marriedAdult
	, int _marriedTs
) {	adult = _adult;
	marriedCid = _marriedCid;
	marriedAdult = _marriedAdult;
	marriedTs = _marriedTs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 子嗣数据
/// </summary>
public Common.ChildObj.Adult_Info getAdult() { return adult; }
/// <summary>
/// 子嗣数据
/// </summary>
public void setAdult(Common.ChildObj.Adult_Info _adult) { adult = _adult; }
/// <summary>
/// 结婚对象玩家CID
/// </summary>
public long getMarriedCid() { return marriedCid; }
/// <summary>
/// 结婚对象玩家CID
/// </summary>
public void setMarriedCid(long _marriedCid) { marriedCid = _marriedCid; }
/// <summary>
/// 结婚对象子嗣数据
/// </summary>
public Common.ChildObj.Adult_Info getMarriedAdult() { return marriedAdult; }
/// <summary>
/// 结婚对象子嗣数据
/// </summary>
public void setMarriedAdult(Common.ChildObj.Adult_Info _marriedAdult) { marriedAdult = _marriedAdult; }
/// <summary>
/// 结婚时间（秒）
/// </summary>
public int getMarriedTs() { return marriedTs; }
/// <summary>
/// 结婚时间（秒）
/// </summary>
public void setMarriedTs(int _marriedTs) { marriedTs = _marriedTs; }


public int GetBufSize() {
	int _size = 12;
	_size += 4 + adult.GetBufSize();
	_size += 4 + marriedAdult.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + adult.GetBufSize();
	_size += 4 + marriedAdult.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _adultCustLen = _buf.getInt();
	int _adultCurPos = _buf.getCurPos();
	adult.ReadUnzipBuf(_buf, _adultCurPos + _adultCustLen);
	_buf.setPosition(_adultCurPos + _adultCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	marriedCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _marriedAdultCustLen = _buf.getInt();
	int _marriedAdultCurPos = _buf.getCurPos();
	marriedAdult.ReadUnzipBuf(_buf, _marriedAdultCurPos + _marriedAdultCustLen);
	_buf.setPosition(_marriedAdultCurPos + _marriedAdultCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	marriedTs = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(adult.GetBufSize());
	adult.PutUnzipBuf(_buf);
	_buf.putLong(marriedCid);
	_buf.putInt(marriedAdult.GetBufSize());
	marriedAdult.PutUnzipBuf(_buf);
	_buf.putInt(marriedTs);
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
	builder.Append("adult").Append(":").Append(adult == null ? "null" : adult.ToString()).Append(", ");
	builder.Append("marriedCid").Append(":").Append(marriedCid.ToString()).Append(", ");
	builder.Append("marriedAdult").Append(":").Append(marriedAdult == null ? "null" : marriedAdult.ToString()).Append(", ");
	builder.Append("marriedTs").Append(":").Append(marriedTs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

