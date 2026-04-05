using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.InnObj
{

/// <summary>
/// 旅店_接待客人信息
/// </summary>
public class Inn_ReceiveGuestInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 队列中的索引
/// </summary>
private int index;
/// <summary>
/// 客人ID
/// </summary>
private long guestId;
/// <summary>
/// 菜品ID
/// </summary>
private long dishId;


public Inn_ReceiveGuestInfo() {
	index = 0;
	guestId = (long)0;
	dishId = (long)0;
}

public Inn_ReceiveGuestInfo(
	int _index
	, long _guestId
	, long _dishId
) {	index = _index;
	guestId = _guestId;
	dishId = _dishId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 队列中的索引
/// </summary>
public int getIndex() { return index; }
/// <summary>
/// 队列中的索引
/// </summary>
public void setIndex(int _index) { index = _index; }
/// <summary>
/// 客人ID
/// </summary>
public long getGuestId() { return guestId; }
/// <summary>
/// 客人ID
/// </summary>
public void setGuestId(long _guestId) { guestId = _guestId; }
/// <summary>
/// 菜品ID
/// </summary>
public long getDishId() { return dishId; }
/// <summary>
/// 菜品ID
/// </summary>
public void setDishId(long _dishId) { dishId = _dishId; }


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
	index = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guestId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dishId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(index);
	_buf.putLong(guestId);
	_buf.putLong(dishId);
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
	builder.Append("index").Append(":").Append(index.ToString()).Append(", ");
	builder.Append("guestId").Append(":").Append(guestId.ToString()).Append(", ");
	builder.Append("dishId").Append(":").Append(dishId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

