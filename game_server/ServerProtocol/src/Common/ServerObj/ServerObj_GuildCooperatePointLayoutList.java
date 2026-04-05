package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 联盟协作-据点布局数据列表
 **/
public class ServerObj_GuildCooperatePointLayoutList implements ALBasicProtocolPack._IALProtocolStructure {
/** 布局列表 */
private java.util.ArrayList<Common.ServerObj.ServerObj_GuildCooperatePropertyPointLayout> layoutList;


public ServerObj_GuildCooperatePointLayoutList() {
	layoutList = new java.util.ArrayList<Common.ServerObj.ServerObj_GuildCooperatePropertyPointLayout>();
}

public ServerObj_GuildCooperatePointLayoutList(
	 java.util.ArrayList<Common.ServerObj.ServerObj_GuildCooperatePropertyPointLayout> _layoutList
) {	layoutList = _layoutList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 布局列表 */
public java.util.ArrayList<Common.ServerObj.ServerObj_GuildCooperatePropertyPointLayout> getLayoutList() { return layoutList; }
/** 布局列表 */
public void addLayoutList(Common.ServerObj.ServerObj_GuildCooperatePropertyPointLayout _layoutList) { layoutList.add(_layoutList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < layoutList.size(); _i++) {
	_size += 4 + layoutList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < layoutList.size(); _i++) {
	_size += 4 + layoutList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _layoutListCount = _buf.getShort();
	for(int _i = 0; _i < _layoutListCount; _i++) { 
		Common.ServerObj.ServerObj_GuildCooperatePropertyPointLayout _layoutList = new Common.ServerObj.ServerObj_GuildCooperatePropertyPointLayout();
		if(_buf.remaining() <= 0) return;
	int __layoutListCustLen = _buf.getInt();
	int __layoutListCurPos = _buf.position();
	_layoutList.ReadUnzipBuf(_buf, __layoutListCurPos + __layoutListCustLen);
	_buf.position(__layoutListCurPos + __layoutListCustLen);

		layoutList.add(_layoutList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)layoutList.size());
	for(int _i = 0; _i < layoutList.size(); _i++) { 
		_buf.putInt(layoutList.get(_i).GetBufSize());
	layoutList.get(_i).PutUnzipBuf(_buf);
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

