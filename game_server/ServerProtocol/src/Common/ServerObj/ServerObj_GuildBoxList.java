package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 联盟宝箱列表
 **/
public class ServerObj_GuildBoxList implements ALBasicProtocolPack._IALProtocolStructure {
/** 宝箱类型 */
private Common.GuildEnum.EGuildBoxType boxType;
private java.util.ArrayList<Common.GuildObj.Guild_BoxInfo> boxList;


public ServerObj_GuildBoxList() {
	boxType = Common.GuildEnum.EGuildBoxType.values()[0];
	boxList = new java.util.ArrayList<Common.GuildObj.Guild_BoxInfo>();
}

public ServerObj_GuildBoxList(
	 Common.GuildEnum.EGuildBoxType _boxType
	, java.util.ArrayList<Common.GuildObj.Guild_BoxInfo> _boxList
) {	boxType = _boxType;
	boxList = _boxList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 宝箱类型 */
public Common.GuildEnum.EGuildBoxType getBoxType() { return boxType; }
/** 宝箱类型 */
public void setBoxType(Common.GuildEnum.EGuildBoxType _boxType) { boxType = _boxType; }
public java.util.ArrayList<Common.GuildObj.Guild_BoxInfo> getBoxList() { return boxList; }
public void addBoxList(Common.GuildObj.Guild_BoxInfo _boxList) { boxList.add(_boxList); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2 + (boxList.size() * 36);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (boxList.size() * 36);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) boxType = Common.GuildEnum.EGuildBoxType.EGuildBoxType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _boxListCount = _buf.getShort();
	for(int _i = 0; _i < _boxListCount; _i++) { 
		Common.GuildObj.Guild_BoxInfo _boxList = new Common.GuildObj.Guild_BoxInfo();
		if(_buf.remaining() <= 0) return;
	int __boxListCustLen = _buf.getInt();
	int __boxListCurPos = _buf.position();
	_boxList.ReadUnzipBuf(_buf, __boxListCurPos + __boxListCustLen);
	_buf.position(__boxListCurPos + __boxListCustLen);

		boxList.add(_boxList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(boxType.ordinal());

	_buf.putShort((short)boxList.size());
	for(int _i = 0; _i < boxList.size(); _i++) { 
		_buf.putInt(boxList.get(_i).GetBufSize());
	boxList.get(_i).PutUnzipBuf(_buf);
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

