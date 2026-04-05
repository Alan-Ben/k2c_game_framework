package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_012_RetHeroInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣列表 */
private java.util.ArrayList<Common.HeroObj.Hero_Info> heroList;
/** 套系列表 */
private java.util.ArrayList<Common.HeroObj.Hero_SuitInfo> suitList;


public GS2GC_002_012_RetHeroInit() {
	heroList = new java.util.ArrayList<Common.HeroObj.Hero_Info>();
	suitList = new java.util.ArrayList<Common.HeroObj.Hero_SuitInfo>();
}

public GS2GC_002_012_RetHeroInit(
	 java.util.ArrayList<Common.HeroObj.Hero_Info> _heroList
	, java.util.ArrayList<Common.HeroObj.Hero_SuitInfo> _suitList
) {	heroList = _heroList;
	suitList = _suitList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)12; }

/** 大臣列表 */
public java.util.ArrayList<Common.HeroObj.Hero_Info> getHeroList() { return heroList; }
/** 大臣列表 */
public void addHeroList(Common.HeroObj.Hero_Info _heroList) { heroList.add(_heroList); }
/** 套系列表 */
public java.util.ArrayList<Common.HeroObj.Hero_SuitInfo> getSuitList() { return suitList; }
/** 套系列表 */
public void addSuitList(Common.HeroObj.Hero_SuitInfo _suitList) { suitList.add(_suitList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < heroList.size(); _i++) {
	_size += 4 + heroList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < suitList.size(); _i++) {
	_size += 4 + suitList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < heroList.size(); _i++) {
	_size += 4 + heroList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < suitList.size(); _i++) {
	_size += 4 + suitList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _heroListCount = _buf.getShort();
	for(int _i = 0; _i < _heroListCount; _i++) { 
		Common.HeroObj.Hero_Info _heroList = new Common.HeroObj.Hero_Info();
		if(_buf.remaining() <= 0) return;
	int __heroListCustLen = _buf.getInt();
	int __heroListCurPos = _buf.position();
	_heroList.ReadUnzipBuf(_buf, __heroListCurPos + __heroListCustLen);
	_buf.position(__heroListCurPos + __heroListCustLen);

		heroList.add(_heroList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _suitListCount = _buf.getShort();
	for(int _i = 0; _i < _suitListCount; _i++) { 
		Common.HeroObj.Hero_SuitInfo _suitList = new Common.HeroObj.Hero_SuitInfo();
		if(_buf.remaining() <= 0) return;
	int __suitListCustLen = _buf.getInt();
	int __suitListCurPos = _buf.position();
	_suitList.ReadUnzipBuf(_buf, __suitListCurPos + __suitListCustLen);
	_buf.position(__suitListCurPos + __suitListCustLen);

		suitList.add(_suitList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)heroList.size());
	for(int _i = 0; _i < heroList.size(); _i++) { 
		_buf.putInt(heroList.get(_i).GetBufSize());
	heroList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)suitList.size());
	for(int _i = 0; _i < suitList.size(); _i++) { 
		_buf.putInt(suitList.get(_i).GetBufSize());
	suitList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)12);
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

