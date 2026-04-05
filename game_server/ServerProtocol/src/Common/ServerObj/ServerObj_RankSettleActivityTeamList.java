package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 排行榜结算时的活动队伍结算数据列表
 **/
public class ServerObj_RankSettleActivityTeamList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.ServerObj.ServerObj_RankSettleActivityTeam> list;


public ServerObj_RankSettleActivityTeamList() {
	list = new java.util.ArrayList<Common.ServerObj.ServerObj_RankSettleActivityTeam>();
}

public ServerObj_RankSettleActivityTeamList(
	 java.util.ArrayList<Common.ServerObj.ServerObj_RankSettleActivityTeam> _list
) {	list = _list;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public java.util.ArrayList<Common.ServerObj.ServerObj_RankSettleActivityTeam> getList() { return list; }
public void addList(Common.ServerObj.ServerObj_RankSettleActivityTeam _list) { list.add(_list); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (list.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (list.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _listCount = _buf.getShort();
	for(int _i = 0; _i < _listCount; _i++) { 
		Common.ServerObj.ServerObj_RankSettleActivityTeam _list = new Common.ServerObj.ServerObj_RankSettleActivityTeam();
		if(_buf.remaining() <= 0) return;
	int __listCustLen = _buf.getInt();
	int __listCurPos = _buf.position();
	_list.ReadUnzipBuf(_buf, __listCurPos + __listCustLen);
	_buf.position(__listCurPos + __listCustLen);

		list.add(_list);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
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

