package Common.WeekCardObj;

import java.nio.ByteBuffer;
/*********
 * 周卡-妃子倾诉结算列表
 **/
public class WeekCard_SettleInfo_ConsortRndCall implements ALBasicProtocolPack._IALProtocolStructure {
/** 次数 */
private int num;
/** 获得子嗣数量 */
private int childNum;
private java.util.ArrayList<Common.WeekCardObj.WeekCard_ConsortRndCallInfo> list;


public WeekCard_SettleInfo_ConsortRndCall() {
	num = 0;
	childNum = 0;
	list = new java.util.ArrayList<Common.WeekCardObj.WeekCard_ConsortRndCallInfo>();
}

public WeekCard_SettleInfo_ConsortRndCall(
	 int _num
	, int _childNum
	, java.util.ArrayList<Common.WeekCardObj.WeekCard_ConsortRndCallInfo> _list
) {	num = _num;
	childNum = _childNum;
	list = _list;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 次数 */
public int getNum() { return num; }
/** 次数 */
public void setNum(int _num) { num = _num; }
/** 获得子嗣数量 */
public int getChildNum() { return childNum; }
/** 获得子嗣数量 */
public void setChildNum(int _childNum) { childNum = _childNum; }
public java.util.ArrayList<Common.WeekCardObj.WeekCard_ConsortRndCallInfo> getList() { return list; }
public void addList(Common.WeekCardObj.WeekCard_ConsortRndCallInfo _list) { list.add(_list); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (list.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (list.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) childNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _listCount = _buf.getShort();
	for(int _i = 0; _i < _listCount; _i++) { 
		Common.WeekCardObj.WeekCard_ConsortRndCallInfo _list = new Common.WeekCardObj.WeekCard_ConsortRndCallInfo();
		if(_buf.remaining() <= 0) return;
	int __listCustLen = _buf.getInt();
	int __listCurPos = _buf.position();
	_list.ReadUnzipBuf(_buf, __listCurPos + __listCustLen);
	_buf.position(__listCurPos + __listCustLen);

		list.add(_list);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(num);
	_buf.putInt(childNum);
	_buf.putShort((short)list.size());
	for(int _i = 0; _i < list.size(); _i++) { 
		_buf.putInt(list.get(_i).GetBufSize());
	list.get(_i).PutUnzipBuf(_buf);
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

