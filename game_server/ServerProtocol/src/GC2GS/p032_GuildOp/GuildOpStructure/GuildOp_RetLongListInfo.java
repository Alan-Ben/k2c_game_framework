package GC2GS.p032_GuildOp.GuildOpStructure;

import java.nio.ByteBuffer;
/*********
 * 返回一个长整形队列数据
 **/
public class GuildOp_RetLongListInfo implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> list;


public GuildOp_RetLongListInfo() {
	list = new java.util.ArrayList<Long>();
}

public GuildOp_RetLongListInfo(
	 java.util.ArrayList<Long> _list
) {	list = _list;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public java.util.ArrayList<Long> getList() { return list; }
public void addList(long _list) { list.add(_list); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (list.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (list.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _listCount = _buf.getShort();
	for(int _i = 0; _i < _listCount; _i++) { 
		long _list = (long)0;
		if(_buf.remaining() > 0) _list = _buf.getLong();
		list.add(_list);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)list.size());
	for(int _i = 0; _i < list.size(); _i++) { 
		_buf.putLong(list.get(_i));
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

