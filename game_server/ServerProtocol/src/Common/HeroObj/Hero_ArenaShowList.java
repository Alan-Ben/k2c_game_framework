package Common.HeroObj;

import java.nio.ByteBuffer;
/*********
 * 大臣竞技场展示信息列表
 **/
public class Hero_ArenaShowList implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣列表 */
private java.util.ArrayList<Common.HeroObj.Hero_ArenaShowInfo> heroList;


public Hero_ArenaShowList() {
	heroList = new java.util.ArrayList<Common.HeroObj.Hero_ArenaShowInfo>();
}

public Hero_ArenaShowList(
	 java.util.ArrayList<Common.HeroObj.Hero_ArenaShowInfo> _heroList
) {	heroList = _heroList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 大臣列表 */
public java.util.ArrayList<Common.HeroObj.Hero_ArenaShowInfo> getHeroList() { return heroList; }
/** 大臣列表 */
public void addHeroList(Common.HeroObj.Hero_ArenaShowInfo _heroList) { heroList.add(_heroList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (heroList.size() * 32);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (heroList.size() * 32);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _heroListCount = _buf.getShort();
	for(int _i = 0; _i < _heroListCount; _i++) { 
		Common.HeroObj.Hero_ArenaShowInfo _heroList = new Common.HeroObj.Hero_ArenaShowInfo();
		if(_buf.remaining() <= 0) return;
	int __heroListCustLen = _buf.getInt();
	int __heroListCurPos = _buf.position();
	_heroList.ReadUnzipBuf(_buf, __heroListCurPos + __heroListCustLen);
	_buf.position(__heroListCurPos + __heroListCustLen);

		heroList.add(_heroList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)heroList.size());
	for(int _i = 0; _i < heroList.size(); _i++) { 
		_buf.putInt(heroList.get(_i).GetBufSize());
	heroList.get(_i).PutUnzipBuf(_buf);
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

