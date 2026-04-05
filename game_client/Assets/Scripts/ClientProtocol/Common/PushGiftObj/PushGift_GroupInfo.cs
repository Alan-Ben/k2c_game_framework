using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.PushGiftObj
{

/// <summary>
/// 推送礼包-组信息
/// </summary>
public class PushGift_GroupInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 礼包组id
/// </summary>
private long groupId;
/// <summary>
/// 上次触发时间毫秒
/// </summary>
private long lastTriggerTimeMs;
/// <summary>
/// 当前激活的礼包
/// </summary>
private Common.PushGiftObj.PushGift_ActivePackInfo activePack;


public PushGift_GroupInfo() {
	groupId = (long)0;
	lastTriggerTimeMs = (long)0;
	activePack = new Common.PushGiftObj.PushGift_ActivePackInfo();
}

public PushGift_GroupInfo(
	long _groupId
	, long _lastTriggerTimeMs
	, Common.PushGiftObj.PushGift_ActivePackInfo _activePack
) {	groupId = _groupId;
	lastTriggerTimeMs = _lastTriggerTimeMs;
	activePack = _activePack;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 礼包组id
/// </summary>
public long getGroupId() { return groupId; }
/// <summary>
/// 礼包组id
/// </summary>
public void setGroupId(long _groupId) { groupId = _groupId; }
/// <summary>
/// 上次触发时间毫秒
/// </summary>
public long getLastTriggerTimeMs() { return lastTriggerTimeMs; }
/// <summary>
/// 上次触发时间毫秒
/// </summary>
public void setLastTriggerTimeMs(long _lastTriggerTimeMs) { lastTriggerTimeMs = _lastTriggerTimeMs; }
/// <summary>
/// 当前激活的礼包
/// </summary>
public Common.PushGiftObj.PushGift_ActivePackInfo getActivePack() { return activePack; }
/// <summary>
/// 当前激活的礼包
/// </summary>
public void setActivePack(Common.PushGiftObj.PushGift_ActivePackInfo _activePack) { activePack = _activePack; }


public int GetBufSize() {
	int _size = 38;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 40;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastTriggerTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _activePackCustLen = _buf.getInt();
	int _activePackCurPos = _buf.getCurPos();
	activePack.ReadUnzipBuf(_buf, _activePackCurPos + _activePackCustLen);
	_buf.setPosition(_activePackCurPos + _activePackCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(groupId);
	_buf.putLong(lastTriggerTimeMs);
	_buf.putInt(activePack.GetBufSize());
	activePack.PutUnzipBuf(_buf);
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
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("lastTriggerTimeMs").Append(":").Append(lastTriggerTimeMs.ToString()).Append(", ");
	builder.Append("activePack").Append(":").Append(activePack == null ? "null" : activePack.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

