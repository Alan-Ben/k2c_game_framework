package GS2GC.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 宴会交互记录
 **/
public class GS2GC_019_003_RetJoinedPlayerList implements ALBasicProtocolPack._IALProtocolStructure {
/** 赴宴玩家记录 */
private java.util.ArrayList<Common.DinnerObj.Dinner_JoinerLogList> joinerList;


public GS2GC_019_003_RetJoinedPlayerList() {
	joinerList = new java.util.ArrayList<Common.DinnerObj.Dinner_JoinerLogList>();
}

public GS2GC_019_003_RetJoinedPlayerList(
	 java.util.ArrayList<Common.DinnerObj.Dinner_JoinerLogList> _joinerList
) {	joinerList = _joinerList;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)3; }

/** 赴宴玩家记录 */
public java.util.ArrayList<Common.DinnerObj.Dinner_JoinerLogList> getJoinerList() { return joinerList; }
/** 赴宴玩家记录 */
public void addJoinerList(Common.DinnerObj.Dinner_JoinerLogList _joinerList) { joinerList.add(_joinerList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (joinerList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (joinerList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _joinerListCount = _buf.getShort();
	for(int _i = 0; _i < _joinerListCount; _i++) { 
		Common.DinnerObj.Dinner_JoinerLogList _joinerList = new Common.DinnerObj.Dinner_JoinerLogList();
		if(_buf.remaining() <= 0) return;
	int __joinerListCustLen = _buf.getInt();
	int __joinerListCurPos = _buf.position();
	_joinerList.ReadUnzipBuf(_buf, __joinerListCurPos + __joinerListCustLen);
	_buf.position(__joinerListCurPos + __joinerListCustLen);

		joinerList.add(_joinerList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)joinerList.size());
	for(int _i = 0; _i < joinerList.size(); _i++) { 
		_buf.putInt(joinerList.get(_i).GetBufSize());
	joinerList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)3);
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

