using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

/// <summary>
/// 礼包组变更通知
/// </summary>
public class GS2GC_004_074_OnPushGiftPackGroupChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 礼包组信息
/// </summary>
private Common.PushGiftObj.PushGift_GroupInfo groupInfo;


public GS2GC_004_074_OnPushGiftPackGroupChg() {
	groupInfo = new Common.PushGiftObj.PushGift_GroupInfo();
}

public GS2GC_004_074_OnPushGiftPackGroupChg(
	Common.PushGiftObj.PushGift_GroupInfo _groupInfo
) {	groupInfo = _groupInfo;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)74; }

/// <summary>
/// 礼包组信息
/// </summary>
public Common.PushGiftObj.PushGift_GroupInfo getGroupInfo() { return groupInfo; }
/// <summary>
/// 礼包组信息
/// </summary>
public void setGroupInfo(Common.PushGiftObj.PushGift_GroupInfo _groupInfo) { groupInfo = _groupInfo; }


public int GetBufSize() {
	int _size = 42;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 44;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _groupInfoCustLen = _buf.getInt();
	int _groupInfoCurPos = _buf.getCurPos();
	groupInfo.ReadUnzipBuf(_buf, _groupInfoCurPos + _groupInfoCustLen);
	_buf.setPosition(_groupInfoCurPos + _groupInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(groupInfo.GetBufSize());
	groupInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)74);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)74);
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
	builder.Append("groupInfo").Append(":").Append(groupInfo == null ? "null" : groupInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

