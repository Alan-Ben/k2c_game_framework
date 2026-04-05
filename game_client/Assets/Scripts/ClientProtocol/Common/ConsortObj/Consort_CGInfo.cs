using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ConsortObj
{

/// <summary>
/// 家人CG数据
/// </summary>
public class Consort_CGInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 家人CG
/// </summary>
private long cgId;
/// <summary>
/// 是否领取奖励
/// </summary>
private bool rewarded;


public Consort_CGInfo() {
	cgId = (long)0;
	rewarded = false;
}

public Consort_CGInfo(
	long _cgId
	, bool _rewarded
) {	cgId = _cgId;
	rewarded = _rewarded;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 家人CG
/// </summary>
public long getCgId() { return cgId; }
/// <summary>
/// 家人CG
/// </summary>
public void setCgId(long _cgId) { cgId = _cgId; }
/// <summary>
/// 是否领取奖励
/// </summary>
public bool getRewarded() { return rewarded; }
/// <summary>
/// 是否领取奖励
/// </summary>
public void setRewarded(bool _rewarded) { rewarded = _rewarded; }


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
	cgId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rewarded = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cgId);
	_buf.put(rewarded?(byte)1:(byte)0);
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
	builder.Append("cgId").Append(":").Append(cgId.ToString()).Append(", ");
	builder.Append("rewarded").Append(":").Append(rewarded.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

