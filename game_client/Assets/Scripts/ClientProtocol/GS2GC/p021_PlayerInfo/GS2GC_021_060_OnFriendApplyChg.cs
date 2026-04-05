using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

/// <summary>
/// 推送好友申请变更消息
/// </summary>
public class GS2GC_021_060_OnFriendApplyChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 申请数据
/// </summary>
private Common.FriendObj.Friend_ApplyInfo apply;


public GS2GC_021_060_OnFriendApplyChg() {
	apply = new Common.FriendObj.Friend_ApplyInfo();
}

public GS2GC_021_060_OnFriendApplyChg(
	Common.FriendObj.Friend_ApplyInfo _apply
) {	apply = _apply;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)60; }

/// <summary>
/// 申请数据
/// </summary>
public Common.FriendObj.Friend_ApplyInfo getApply() { return apply; }
/// <summary>
/// 申请数据
/// </summary>
public void setApply(Common.FriendObj.Friend_ApplyInfo _apply) { apply = _apply; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _applyCustLen = _buf.getInt();
	int _applyCurPos = _buf.getCurPos();
	apply.ReadUnzipBuf(_buf, _applyCurPos + _applyCustLen);
	_buf.setPosition(_applyCurPos + _applyCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(apply.GetBufSize());
	apply.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)60);
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
	builder.Append("apply").Append(":").Append(apply == null ? "null" : apply.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

