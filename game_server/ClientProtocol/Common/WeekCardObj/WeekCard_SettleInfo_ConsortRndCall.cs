using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.WeekCardObj
{

/// <summary>
/// 周卡-妃子倾诉结算列表
/// </summary>
public class WeekCard_SettleInfo_ConsortRndCall : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 次数
/// </summary>
private int num;
/// <summary>
/// 获得子嗣数量
/// </summary>
private int childNum;
private List<Common.WeekCardObj.WeekCard_ConsortRndCallInfo> list;


public WeekCard_SettleInfo_ConsortRndCall() {
	num = 0;
	childNum = 0;
	list = new List<Common.WeekCardObj.WeekCard_ConsortRndCallInfo>();
}

public WeekCard_SettleInfo_ConsortRndCall(
	int _num
	, int _childNum
	, List<Common.WeekCardObj.WeekCard_ConsortRndCallInfo> _list
) {	num = _num;
	childNum = _childNum;
	list = _list;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 次数
/// </summary>
public int getNum() { return num; }
/// <summary>
/// 次数
/// </summary>
public void setNum(int _num) { num = _num; }
/// <summary>
/// 获得子嗣数量
/// </summary>
public int getChildNum() { return childNum; }
/// <summary>
/// 获得子嗣数量
/// </summary>
public void setChildNum(int _childNum) { childNum = _childNum; }
public List<Common.WeekCardObj.WeekCard_ConsortRndCallInfo> getList() { return list; }
public void addList(Common.WeekCardObj.WeekCard_ConsortRndCallInfo _list) { list.Add(_list); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (list.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (list.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	num = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	childNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _listCount = _buf.getShort();
	for(int _i = 0; _i < _listCount; _i++) { 
		Common.WeekCardObj.WeekCard_ConsortRndCallInfo _list = new Common.WeekCardObj.WeekCard_ConsortRndCallInfo();
		int __listCustLen = _buf.getInt();
	int __listCurPos = _buf.getCurPos();
	_list.ReadUnzipBuf(_buf, __listCurPos + __listCustLen);
	_buf.setPosition(__listCurPos + __listCustLen);

		list.Add(_list);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(num);
	_buf.putInt(childNum);
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
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("childNum").Append(":").Append(childNum.ToString()).Append(", ");
	builder.Append("list").Append(":").Append(list.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

