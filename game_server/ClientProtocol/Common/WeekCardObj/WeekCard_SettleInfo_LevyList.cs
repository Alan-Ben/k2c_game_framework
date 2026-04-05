using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.WeekCardObj
{

/// <summary>
/// 周卡-征收结算列表
/// </summary>
public class WeekCard_SettleInfo_LevyList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 列表
/// </summary>
private List<Common.WeekCardObj.WeekCard_SettleInfo_LevyInfo> list;


public WeekCard_SettleInfo_LevyList() {
	list = new List<Common.WeekCardObj.WeekCard_SettleInfo_LevyInfo>();
}

public WeekCard_SettleInfo_LevyList(
	List<Common.WeekCardObj.WeekCard_SettleInfo_LevyInfo> _list
) {	list = _list;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 列表
/// </summary>
public List<Common.WeekCardObj.WeekCard_SettleInfo_LevyInfo> getList() { return list; }
/// <summary>
/// 列表
/// </summary>
public void addList(Common.WeekCardObj.WeekCard_SettleInfo_LevyInfo _list) { list.Add(_list); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (list.Count * 12);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (list.Count * 12);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _listCount = _buf.getShort();
	for(int _i = 0; _i < _listCount; _i++) { 
		Common.WeekCardObj.WeekCard_SettleInfo_LevyInfo _list = new Common.WeekCardObj.WeekCard_SettleInfo_LevyInfo();
		int __listCustLen = _buf.getInt();
	int __listCurPos = _buf.getCurPos();
	_list.ReadUnzipBuf(_buf, __listCurPos + __listCustLen);
	_buf.setPosition(__listCurPos + __listCustLen);

		list.Add(_list);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)list.Count);
	for(int _i = 0; _i < list.Count; _i++) { 
		_buf.putInt(list[_i].GetBufSize());
	list[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("list").Append(":").Append(list.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

