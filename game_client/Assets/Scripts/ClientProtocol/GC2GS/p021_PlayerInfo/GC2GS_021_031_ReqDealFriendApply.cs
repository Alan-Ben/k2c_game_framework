using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p021_PlayerInfo
{

public class GC2GS_021_031_ReqDealFriendApply : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// true-同意，false-拒绝
/// </summary>
private bool isAgree;
private long applyCid;


public GC2GS_021_031_ReqDealFriendApply() {
	isAgree = false;
	applyCid = (long)0;
}

public GC2GS_021_031_ReqDealFriendApply(
	bool _isAgree
	, long _applyCid
) {	isAgree = _isAgree;
	applyCid = _applyCid;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)31; }

/// <summary>
/// true-同意，false-拒绝
/// </summary>
public bool getIsAgree() { return isAgree; }
/// <summary>
/// true-同意，false-拒绝
/// </summary>
public void setIsAgree(bool _isAgree) { isAgree = _isAgree; }
public long getApplyCid() { return applyCid; }
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }


public int GetBufSize() {
	int _size = 9;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAgree = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	applyCid = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isAgree?(byte)1:(byte)0);
	_buf.putLong(applyCid);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)31);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)31);
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
	builder.Append("isAgree").Append(":").Append(isAgree.ToString()).Append(", ");
	builder.Append("applyCid").Append(":").Append(applyCid.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

