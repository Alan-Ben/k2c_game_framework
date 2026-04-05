using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ChildObj
{

/// <summary>
/// 子嗣数据奖励
/// </summary>
public class Adult_MarryReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已婚子嗣数据
/// </summary>
private Common.ChildObj.Adult_MarriedInfo marriedInfo;
/// <summary>
/// 奖励物品
/// </summary>
private NPCommon.NPCommon_ItemInfo item;


public Adult_MarryReward() {
	marriedInfo = new Common.ChildObj.Adult_MarriedInfo();
	item = new NPCommon.NPCommon_ItemInfo();
}

public Adult_MarryReward(
	Common.ChildObj.Adult_MarriedInfo _marriedInfo
	, NPCommon.NPCommon_ItemInfo _item
) {	marriedInfo = _marriedInfo;
	item = _item;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 已婚子嗣数据
/// </summary>
public Common.ChildObj.Adult_MarriedInfo getMarriedInfo() { return marriedInfo; }
/// <summary>
/// 已婚子嗣数据
/// </summary>
public void setMarriedInfo(Common.ChildObj.Adult_MarriedInfo _marriedInfo) { marriedInfo = _marriedInfo; }
/// <summary>
/// 奖励物品
/// </summary>
public NPCommon.NPCommon_ItemInfo getItem() { return item; }
/// <summary>
/// 奖励物品
/// </summary>
public void setItem(NPCommon.NPCommon_ItemInfo _item) { item = _item; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + marriedInfo.GetBufSize();
	_size += 4 + item.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + marriedInfo.GetBufSize();
	_size += 4 + item.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _marriedInfoCustLen = _buf.getInt();
	int _marriedInfoCurPos = _buf.getCurPos();
	marriedInfo.ReadUnzipBuf(_buf, _marriedInfoCurPos + _marriedInfoCustLen);
	_buf.setPosition(_marriedInfoCurPos + _marriedInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _itemCustLen = _buf.getInt();
	int _itemCurPos = _buf.getCurPos();
	item.ReadUnzipBuf(_buf, _itemCurPos + _itemCustLen);
	_buf.setPosition(_itemCurPos + _itemCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(marriedInfo.GetBufSize());
	marriedInfo.PutUnzipBuf(_buf);
	_buf.putInt(item.GetBufSize());
	item.PutUnzipBuf(_buf);
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
	builder.Append("marriedInfo").Append(":").Append(marriedInfo == null ? "null" : marriedInfo.ToString()).Append(", ");
	builder.Append("item").Append(":").Append(item == null ? "null" : item.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

