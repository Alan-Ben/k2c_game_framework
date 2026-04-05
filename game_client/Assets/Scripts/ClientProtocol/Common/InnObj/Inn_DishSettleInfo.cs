using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.InnObj
{

/// <summary>
/// 旅店_菜品结算信息
/// </summary>
public class Inn_DishSettleInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 菜品ID
/// </summary>
private long dishId;
/// <summary>
/// 接待数量
/// </summary>
private int receiveNum;
/// <summary>
/// 获得熟练度
/// </summary>
private long addFinesse;


public Inn_DishSettleInfo() {
	dishId = (long)0;
	receiveNum = 0;
	addFinesse = (long)0;
}

public Inn_DishSettleInfo(
	long _dishId
	, int _receiveNum
	, long _addFinesse
) {	dishId = _dishId;
	receiveNum = _receiveNum;
	addFinesse = _addFinesse;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 菜品ID
/// </summary>
public long getDishId() { return dishId; }
/// <summary>
/// 菜品ID
/// </summary>
public void setDishId(long _dishId) { dishId = _dishId; }
/// <summary>
/// 接待数量
/// </summary>
public int getReceiveNum() { return receiveNum; }
/// <summary>
/// 接待数量
/// </summary>
public void setReceiveNum(int _receiveNum) { receiveNum = _receiveNum; }
/// <summary>
/// 获得熟练度
/// </summary>
public long getAddFinesse() { return addFinesse; }
/// <summary>
/// 获得熟练度
/// </summary>
public void setAddFinesse(long _addFinesse) { addFinesse = _addFinesse; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dishId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	receiveNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addFinesse = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dishId);
	_buf.putInt(receiveNum);
	_buf.putLong(addFinesse);
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
	builder.Append("dishId").Append(":").Append(dishId.ToString()).Append(", ");
	builder.Append("receiveNum").Append(":").Append(receiveNum.ToString()).Append(", ");
	builder.Append("addFinesse").Append(":").Append(addFinesse.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

