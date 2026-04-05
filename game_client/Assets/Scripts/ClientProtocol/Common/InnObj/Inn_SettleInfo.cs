using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.InnObj
{

/// <summary>
/// 旅店_结算信息
/// </summary>
public class Inn_SettleInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 接待数量
/// </summary>
private int receiveNum;
/// <summary>
/// 获得人气值
/// </summary>
private long addPopularity;
/// <summary>
/// 获得心意值
/// </summary>
private long addAffection;
/// <summary>
/// 获得设施蓝图数量
/// </summary>
private long addStationBlueprintNum;
/// <summary>
/// 菜品结算列表
/// </summary>
private List<Common.InnObj.Inn_DishSettleInfo> dishList;


public Inn_SettleInfo() {
	receiveNum = 0;
	addPopularity = (long)0;
	addAffection = (long)0;
	addStationBlueprintNum = (long)0;
	dishList = new List<Common.InnObj.Inn_DishSettleInfo>();
}

public Inn_SettleInfo(
	int _receiveNum
	, long _addPopularity
	, long _addAffection
	, long _addStationBlueprintNum
	, List<Common.InnObj.Inn_DishSettleInfo> _dishList
) {	receiveNum = _receiveNum;
	addPopularity = _addPopularity;
	addAffection = _addAffection;
	addStationBlueprintNum = _addStationBlueprintNum;
	dishList = _dishList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 接待数量
/// </summary>
public int getReceiveNum() { return receiveNum; }
/// <summary>
/// 接待数量
/// </summary>
public void setReceiveNum(int _receiveNum) { receiveNum = _receiveNum; }
/// <summary>
/// 获得人气值
/// </summary>
public long getAddPopularity() { return addPopularity; }
/// <summary>
/// 获得人气值
/// </summary>
public void setAddPopularity(long _addPopularity) { addPopularity = _addPopularity; }
/// <summary>
/// 获得心意值
/// </summary>
public long getAddAffection() { return addAffection; }
/// <summary>
/// 获得心意值
/// </summary>
public void setAddAffection(long _addAffection) { addAffection = _addAffection; }
/// <summary>
/// 获得设施蓝图数量
/// </summary>
public long getAddStationBlueprintNum() { return addStationBlueprintNum; }
/// <summary>
/// 获得设施蓝图数量
/// </summary>
public void setAddStationBlueprintNum(long _addStationBlueprintNum) { addStationBlueprintNum = _addStationBlueprintNum; }
/// <summary>
/// 菜品结算列表
/// </summary>
public List<Common.InnObj.Inn_DishSettleInfo> getDishList() { return dishList; }
/// <summary>
/// 菜品结算列表
/// </summary>
public void addDishList(Common.InnObj.Inn_DishSettleInfo _dishList) { dishList.Add(_dishList); }


public int GetBufSize() {
	int _size = 28;
	_size += 2 + (dishList.Count * 24);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;
	_size += 2 + (dishList.Count * 24);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	receiveNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addPopularity = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addAffection = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addStationBlueprintNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _dishListCount = _buf.getShort();
	for(int _i = 0; _i < _dishListCount; _i++) { 
		Common.InnObj.Inn_DishSettleInfo _dishList = new Common.InnObj.Inn_DishSettleInfo();
		int __dishListCustLen = _buf.getInt();
	int __dishListCurPos = _buf.getCurPos();
	_dishList.ReadUnzipBuf(_buf, __dishListCurPos + __dishListCustLen);
	_buf.setPosition(__dishListCurPos + __dishListCustLen);

		dishList.Add(_dishList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(receiveNum);
	_buf.putLong(addPopularity);
	_buf.putLong(addAffection);
	_buf.putLong(addStationBlueprintNum);
	_buf.putShort((short)dishList.Count);
	for(int _i = 0; _i < dishList.Count; _i++) { 
		_buf.putInt(dishList[_i].GetBufSize());
	dishList[_i].PutUnzipBuf(_buf);
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
	builder.Append("receiveNum").Append(":").Append(receiveNum.ToString()).Append(", ");
	builder.Append("addPopularity").Append(":").Append(addPopularity.ToString()).Append(", ");
	builder.Append("addAffection").Append(":").Append(addAffection.ToString()).Append(", ");
	builder.Append("addStationBlueprintNum").Append(":").Append(addStationBlueprintNum.ToString()).Append(", ");
	builder.Append("dishList").Append(":").Append(dishList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

