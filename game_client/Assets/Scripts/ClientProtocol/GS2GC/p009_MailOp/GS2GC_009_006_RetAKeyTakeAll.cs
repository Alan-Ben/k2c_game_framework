using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p009_MailOp
{

public class GS2GC_009_006_RetAKeyTakeAll : ALBasicProtocolPack._IALProtocolStructure {
private List<NPCommon.NPCommon_ItemInfo> gainItemList;
/// <summary>
/// 未读的必读邮件id列表
/// </summary>
private List<long> mustReadMailList;


public GS2GC_009_006_RetAKeyTakeAll() {
	gainItemList = new List<NPCommon.NPCommon_ItemInfo>();
	mustReadMailList = new List<long>();
}

public GS2GC_009_006_RetAKeyTakeAll(
	List<NPCommon.NPCommon_ItemInfo> _gainItemList
	, List<long> _mustReadMailList
) {	gainItemList = _gainItemList;
	mustReadMailList = _mustReadMailList;
}

public byte getMainOrder() { return (byte)9; }

public byte getSubOrder() { return (byte)6; }

public List<NPCommon.NPCommon_ItemInfo> getGainItemList() { return gainItemList; }
public void addGainItemList(NPCommon.NPCommon_ItemInfo _gainItemList) { gainItemList.Add(_gainItemList); }
/// <summary>
/// 未读的必读邮件id列表
/// </summary>
public List<long> getMustReadMailList() { return mustReadMailList; }
/// <summary>
/// 未读的必读邮件id列表
/// </summary>
public void addMustReadMailList(long _mustReadMailList) { mustReadMailList.Add(_mustReadMailList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < gainItemList.Count; _i++) {
	_size += 4 + gainItemList[_i].GetBufSize();
	}

	_size += 2 + (mustReadMailList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < gainItemList.Count; _i++) {
	_size += 4 + gainItemList[_i].GetBufSize();
	}

	_size += 2 + (mustReadMailList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _gainItemListCount = _buf.getShort();
	for(int _i = 0; _i < _gainItemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _gainItemList = new NPCommon.NPCommon_ItemInfo();
		int __gainItemListCustLen = _buf.getInt();
	int __gainItemListCurPos = _buf.getCurPos();
	_gainItemList.ReadUnzipBuf(_buf, __gainItemListCurPos + __gainItemListCustLen);
	_buf.setPosition(__gainItemListCurPos + __gainItemListCustLen);

		gainItemList.Add(_gainItemList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _mustReadMailListCount = _buf.getShort();
	for(int _i = 0; _i < _mustReadMailListCount; _i++) { 
		long _mustReadMailList = (long)0;
		_mustReadMailList = _buf.getLong();
		mustReadMailList.Add(_mustReadMailList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)gainItemList.Count);
	for(int _i = 0; _i < gainItemList.Count; _i++) { 
		_buf.putInt(gainItemList[_i].GetBufSize());
	gainItemList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)mustReadMailList.Count);
	for(int _i = 0; _i < mustReadMailList.Count; _i++) { 
		_buf.putLong(mustReadMailList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)6);
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
	builder.Append("gainItemList").Append(":").Append(gainItemList.ToString()).Append(", ");
	builder.Append("mustReadMailList").Append(":").Append(mustReadMailList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

