using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.InnObj
{

/// <summary>
/// 旅店_客人信息
/// </summary>
public class Inn_GuestInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 客人ID
/// </summary>
private long guestId;
/// <summary>
/// 是否领取图鉴奖励
/// </summary>
private bool hadDrawHandbookReward;
/// <summary>
/// 开始排队的ID
/// </summary>
private long startLineUpId;


public Inn_GuestInfo() {
	guestId = (long)0;
	hadDrawHandbookReward = false;
	startLineUpId = (long)0;
}

public Inn_GuestInfo(
	long _guestId
	, bool _hadDrawHandbookReward
	, long _startLineUpId
) {	guestId = _guestId;
	hadDrawHandbookReward = _hadDrawHandbookReward;
	startLineUpId = _startLineUpId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 客人ID
/// </summary>
public long getGuestId() { return guestId; }
/// <summary>
/// 客人ID
/// </summary>
public void setGuestId(long _guestId) { guestId = _guestId; }
/// <summary>
/// 是否领取图鉴奖励
/// </summary>
public bool getHadDrawHandbookReward() { return hadDrawHandbookReward; }
/// <summary>
/// 是否领取图鉴奖励
/// </summary>
public void setHadDrawHandbookReward(bool _hadDrawHandbookReward) { hadDrawHandbookReward = _hadDrawHandbookReward; }
/// <summary>
/// 开始排队的ID
/// </summary>
public long getStartLineUpId() { return startLineUpId; }
/// <summary>
/// 开始排队的ID
/// </summary>
public void setStartLineUpId(long _startLineUpId) { startLineUpId = _startLineUpId; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guestId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDrawHandbookReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startLineUpId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(guestId);
	_buf.put(hadDrawHandbookReward?(byte)1:(byte)0);
	_buf.putLong(startLineUpId);
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
	builder.Append("guestId").Append(":").Append(guestId.ToString()).Append(", ");
	builder.Append("hadDrawHandbookReward").Append(":").Append(hadDrawHandbookReward.ToString()).Append(", ");
	builder.Append("startLineUpId").Append(":").Append(startLineUpId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

