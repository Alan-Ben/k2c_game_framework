using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ChildObj
{

/// <summary>
/// 向玩家请求指定联姻请求数据
/// </summary>
public class Adult_ToMeApplyInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 请求玩家CID
/// </summary>
private long applyCid;
/// <summary>
/// 请求子嗣的数据
/// </summary>
private Common.ChildObj.Adult_Info applyAdult;


public Adult_ToMeApplyInfo() {
	applyCid = (long)0;
	applyAdult = new Common.ChildObj.Adult_Info();
}

public Adult_ToMeApplyInfo(
	long _applyCid
	, Common.ChildObj.Adult_Info _applyAdult
) {	applyCid = _applyCid;
	applyAdult = _applyAdult;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 请求玩家CID
/// </summary>
public long getApplyCid() { return applyCid; }
/// <summary>
/// 请求玩家CID
/// </summary>
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/// <summary>
/// 请求子嗣的数据
/// </summary>
public Common.ChildObj.Adult_Info getApplyAdult() { return applyAdult; }
/// <summary>
/// 请求子嗣的数据
/// </summary>
public void setApplyAdult(Common.ChildObj.Adult_Info _applyAdult) { applyAdult = _applyAdult; }


public int GetBufSize() {
	int _size = 8;
	_size += 4 + applyAdult.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + applyAdult.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _applyAdultCustLen = _buf.getInt();
	int _applyAdultCurPos = _buf.getCurPos();
	applyAdult.ReadUnzipBuf(_buf, _applyAdultCurPos + _applyAdultCustLen);
	_buf.setPosition(_applyAdultCurPos + _applyAdultCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(applyCid);
	_buf.putInt(applyAdult.GetBufSize());
	applyAdult.PutUnzipBuf(_buf);
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
	builder.Append("applyCid").Append(":").Append(applyCid.ToString()).Append(", ");
	builder.Append("applyAdult").Append(":").Append(applyAdult == null ? "null" : applyAdult.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

