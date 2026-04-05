using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.InnObj
{

/// <summary>
/// 旅店_特殊客人信息
/// </summary>
public class Inn_SpecialGuestInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 特殊客人ID
/// </summary>
private long specialGuestId;
/// <summary>
/// 是否接待过
/// </summary>
private bool hadBeenServe;
/// <summary>
/// 是否领取图鉴奖励
/// </summary>
private bool hadDrawHandbookReward;


public Inn_SpecialGuestInfo() {
	specialGuestId = (long)0;
	hadBeenServe = false;
	hadDrawHandbookReward = false;
}

public Inn_SpecialGuestInfo(
	long _specialGuestId
	, bool _hadBeenServe
	, bool _hadDrawHandbookReward
) {	specialGuestId = _specialGuestId;
	hadBeenServe = _hadBeenServe;
	hadDrawHandbookReward = _hadDrawHandbookReward;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 特殊客人ID
/// </summary>
public long getSpecialGuestId() { return specialGuestId; }
/// <summary>
/// 特殊客人ID
/// </summary>
public void setSpecialGuestId(long _specialGuestId) { specialGuestId = _specialGuestId; }
/// <summary>
/// 是否接待过
/// </summary>
public bool getHadBeenServe() { return hadBeenServe; }
/// <summary>
/// 是否接待过
/// </summary>
public void setHadBeenServe(bool _hadBeenServe) { hadBeenServe = _hadBeenServe; }
/// <summary>
/// 是否领取图鉴奖励
/// </summary>
public bool getHadDrawHandbookReward() { return hadDrawHandbookReward; }
/// <summary>
/// 是否领取图鉴奖励
/// </summary>
public void setHadDrawHandbookReward(bool _hadDrawHandbookReward) { hadDrawHandbookReward = _hadDrawHandbookReward; }


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
	specialGuestId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadBeenServe = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDrawHandbookReward = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(specialGuestId);
	_buf.put(hadBeenServe?(byte)1:(byte)0);
	_buf.put(hadDrawHandbookReward?(byte)1:(byte)0);
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
	builder.Append("specialGuestId").Append(":").Append(specialGuestId.ToString()).Append(", ");
	builder.Append("hadBeenServe").Append(":").Append(hadBeenServe.ToString()).Append(", ");
	builder.Append("hadDrawHandbookReward").Append(":").Append(hadDrawHandbookReward.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

