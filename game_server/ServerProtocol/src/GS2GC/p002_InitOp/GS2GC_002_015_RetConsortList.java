package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 家人初始化
 **/
public class GS2GC_002_015_RetConsortList implements ALBasicProtocolPack._IALProtocolStructure {
/** 已获得家人列表 */
private java.util.ArrayList<Common.ConsortObj.Consort_Info> consortList;
/** 已解锁的CG列表 */
private java.util.ArrayList<Common.ConsortObj.Consort_CGInfo> unlockedCGList;
/** 随机邀约的指定妃子ID列表 */
private java.util.ArrayList<Long> randCallConsortIdList;


public GS2GC_002_015_RetConsortList() {
	consortList = new java.util.ArrayList<Common.ConsortObj.Consort_Info>();
	unlockedCGList = new java.util.ArrayList<Common.ConsortObj.Consort_CGInfo>();
	randCallConsortIdList = new java.util.ArrayList<Long>();
}

public GS2GC_002_015_RetConsortList(
	 java.util.ArrayList<Common.ConsortObj.Consort_Info> _consortList
	, java.util.ArrayList<Common.ConsortObj.Consort_CGInfo> _unlockedCGList
	, java.util.ArrayList<Long> _randCallConsortIdList
) {	consortList = _consortList;
	unlockedCGList = _unlockedCGList;
	randCallConsortIdList = _randCallConsortIdList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)15; }

/** 已获得家人列表 */
public java.util.ArrayList<Common.ConsortObj.Consort_Info> getConsortList() { return consortList; }
/** 已获得家人列表 */
public void addConsortList(Common.ConsortObj.Consort_Info _consortList) { consortList.add(_consortList); }
/** 已解锁的CG列表 */
public java.util.ArrayList<Common.ConsortObj.Consort_CGInfo> getUnlockedCGList() { return unlockedCGList; }
/** 已解锁的CG列表 */
public void addUnlockedCGList(Common.ConsortObj.Consort_CGInfo _unlockedCGList) { unlockedCGList.add(_unlockedCGList); }
/** 随机邀约的指定妃子ID列表 */
public java.util.ArrayList<Long> getRandCallConsortIdList() { return randCallConsortIdList; }
/** 随机邀约的指定妃子ID列表 */
public void addRandCallConsortIdList(long _randCallConsortIdList) { randCallConsortIdList.add(_randCallConsortIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < consortList.size(); _i++) {
	_size += 4 + consortList.get(_i).GetBufSize();
	}

	_size += 2 + (unlockedCGList.size() * 13);
	_size += 2 + (randCallConsortIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < consortList.size(); _i++) {
	_size += 4 + consortList.get(_i).GetBufSize();
	}

	_size += 2 + (unlockedCGList.size() * 13);
	_size += 2 + (randCallConsortIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _consortListCount = _buf.getShort();
	for(int _i = 0; _i < _consortListCount; _i++) { 
		Common.ConsortObj.Consort_Info _consortList = new Common.ConsortObj.Consort_Info();
		if(_buf.remaining() <= 0) return;
	int __consortListCustLen = _buf.getInt();
	int __consortListCurPos = _buf.position();
	_consortList.ReadUnzipBuf(_buf, __consortListCurPos + __consortListCustLen);
	_buf.position(__consortListCurPos + __consortListCustLen);

		consortList.add(_consortList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _unlockedCGListCount = _buf.getShort();
	for(int _i = 0; _i < _unlockedCGListCount; _i++) { 
		Common.ConsortObj.Consort_CGInfo _unlockedCGList = new Common.ConsortObj.Consort_CGInfo();
		if(_buf.remaining() <= 0) return;
	int __unlockedCGListCustLen = _buf.getInt();
	int __unlockedCGListCurPos = _buf.position();
	_unlockedCGList.ReadUnzipBuf(_buf, __unlockedCGListCurPos + __unlockedCGListCustLen);
	_buf.position(__unlockedCGListCurPos + __unlockedCGListCustLen);

		unlockedCGList.add(_unlockedCGList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _randCallConsortIdListCount = _buf.getShort();
	for(int _i = 0; _i < _randCallConsortIdListCount; _i++) { 
		long _randCallConsortIdList = (long)0;
		if(_buf.remaining() > 0) _randCallConsortIdList = _buf.getLong();
		randCallConsortIdList.add(_randCallConsortIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)consortList.size());
	for(int _i = 0; _i < consortList.size(); _i++) { 
		_buf.putInt(consortList.get(_i).GetBufSize());
	consortList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)unlockedCGList.size());
	for(int _i = 0; _i < unlockedCGList.size(); _i++) { 
		_buf.putInt(unlockedCGList.get(_i).GetBufSize());
	unlockedCGList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)randCallConsortIdList.size());
	for(int _i = 0; _i < randCallConsortIdList.size(); _i++) { 
		_buf.putLong(randCallConsortIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)15);
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

