package Common.InnObj;

import java.nio.ByteBuffer;
/*********
 * 旅店_结算信息
 **/
public class Inn_SettleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 接待数量 */
private int receiveNum;
/** 获得人气值 */
private long addPopularity;
/** 获得心意值 */
private long addAffection;
/** 获得设施蓝图数量 */
private long addStationBlueprintNum;
/** 菜品结算列表 */
private java.util.ArrayList<Common.InnObj.Inn_DishSettleInfo> dishList;


public Inn_SettleInfo() {
	receiveNum = 0;
	addPopularity = (long)0;
	addAffection = (long)0;
	addStationBlueprintNum = (long)0;
	dishList = new java.util.ArrayList<Common.InnObj.Inn_DishSettleInfo>();
}

public Inn_SettleInfo(
	 int _receiveNum
	, long _addPopularity
	, long _addAffection
	, long _addStationBlueprintNum
	, java.util.ArrayList<Common.InnObj.Inn_DishSettleInfo> _dishList
) {	receiveNum = _receiveNum;
	addPopularity = _addPopularity;
	addAffection = _addAffection;
	addStationBlueprintNum = _addStationBlueprintNum;
	dishList = _dishList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 接待数量 */
public int getReceiveNum() { return receiveNum; }
/** 接待数量 */
public void setReceiveNum(int _receiveNum) { receiveNum = _receiveNum; }
/** 获得人气值 */
public long getAddPopularity() { return addPopularity; }
/** 获得人气值 */
public void setAddPopularity(long _addPopularity) { addPopularity = _addPopularity; }
/** 获得心意值 */
public long getAddAffection() { return addAffection; }
/** 获得心意值 */
public void setAddAffection(long _addAffection) { addAffection = _addAffection; }
/** 获得设施蓝图数量 */
public long getAddStationBlueprintNum() { return addStationBlueprintNum; }
/** 获得设施蓝图数量 */
public void setAddStationBlueprintNum(long _addStationBlueprintNum) { addStationBlueprintNum = _addStationBlueprintNum; }
/** 菜品结算列表 */
public java.util.ArrayList<Common.InnObj.Inn_DishSettleInfo> getDishList() { return dishList; }
/** 菜品结算列表 */
public void addDishList(Common.InnObj.Inn_DishSettleInfo _dishList) { dishList.add(_dishList); }


public final int GetBufSize() {
	int _size = 28;
	_size += 2 + (dishList.size() * 24);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;
	_size += 2 + (dishList.size() * 24);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) receiveNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addPopularity = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addAffection = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addStationBlueprintNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dishListCount = _buf.getShort();
	for(int _i = 0; _i < _dishListCount; _i++) { 
		Common.InnObj.Inn_DishSettleInfo _dishList = new Common.InnObj.Inn_DishSettleInfo();
		if(_buf.remaining() <= 0) return;
	int __dishListCustLen = _buf.getInt();
	int __dishListCurPos = _buf.position();
	_dishList.ReadUnzipBuf(_buf, __dishListCurPos + __dishListCustLen);
	_buf.position(__dishListCurPos + __dishListCustLen);

		dishList.add(_dishList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(receiveNum);
	_buf.putLong(addPopularity);
	_buf.putLong(addAffection);
	_buf.putLong(addStationBlueprintNum);
	_buf.putShort((short)dishList.size());
	for(int _i = 0; _i < dishList.size(); _i++) { 
		_buf.putInt(dishList.get(_i).GetBufSize());
	dishList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

