using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.PushGiftObj
{

/// <summary>
/// 推送礼包-激活礼包信息
/// </summary>
public class PushGift_ActivePackInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 推送礼包id
/// </summary>
private long pushGiftId;
/// <summary>
/// 激活时间毫秒
/// </summary>
private long activateTimeMs;
/// <summary>
/// 是否已读
/// </summary>
private bool hasRead;
/// <summary>
/// 是否自动激活
/// </summary>
private bool isAutoActive;


public PushGift_ActivePackInfo() {
	pushGiftId = (long)0;
	activateTimeMs = (long)0;
	hasRead = false;
	isAutoActive = false;
}

public PushGift_ActivePackInfo(
	long _pushGiftId
	, long _activateTimeMs
	, bool _hasRead
	, bool _isAutoActive
) {	pushGiftId = _pushGiftId;
	activateTimeMs = _activateTimeMs;
	hasRead = _hasRead;
	isAutoActive = _isAutoActive;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 推送礼包id
/// </summary>
public long getPushGiftId() { return pushGiftId; }
/// <summary>
/// 推送礼包id
/// </summary>
public void setPushGiftId(long _pushGiftId) { pushGiftId = _pushGiftId; }
/// <summary>
/// 激活时间毫秒
/// </summary>
public long getActivateTimeMs() { return activateTimeMs; }
/// <summary>
/// 激活时间毫秒
/// </summary>
public void setActivateTimeMs(long _activateTimeMs) { activateTimeMs = _activateTimeMs; }
/// <summary>
/// 是否已读
/// </summary>
public bool getHasRead() { return hasRead; }
/// <summary>
/// 是否已读
/// </summary>
public void setHasRead(bool _hasRead) { hasRead = _hasRead; }
/// <summary>
/// 是否自动激活
/// </summary>
public bool getIsAutoActive() { return isAutoActive; }
/// <summary>
/// 是否自动激活
/// </summary>
public void setIsAutoActive(bool _isAutoActive) { isAutoActive = _isAutoActive; }


public int GetBufSize() {
	int _size = 18;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 20;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pushGiftId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activateTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasRead = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAutoActive = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(pushGiftId);
	_buf.putLong(activateTimeMs);
	_buf.put(hasRead?(byte)1:(byte)0);
	_buf.put(isAutoActive?(byte)1:(byte)0);
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
	builder.Append("pushGiftId").Append(":").Append(pushGiftId.ToString()).Append(", ");
	builder.Append("activateTimeMs").Append(":").Append(activateTimeMs.ToString()).Append(", ");
	builder.Append("hasRead").Append(":").Append(hasRead.ToString()).Append(", ");
	builder.Append("isAutoActive").Append(":").Append(isAutoActive.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

