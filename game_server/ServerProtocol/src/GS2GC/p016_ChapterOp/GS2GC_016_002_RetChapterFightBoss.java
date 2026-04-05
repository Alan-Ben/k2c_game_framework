package GS2GC.p016_ChapterOp;

import java.nio.ByteBuffer;
public class GS2GC_016_002_RetChapterFightBoss implements ALBasicProtocolPack._IALProtocolStructure {
private long chapterId;
/** 物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> rewardList;


public GS2GC_016_002_RetChapterFightBoss() {
	chapterId = (long)0;
	rewardList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_016_002_RetChapterFightBoss(
	 long _chapterId
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _rewardList
) {	chapterId = _chapterId;
	rewardList = _rewardList;
}

public final byte getMainOrder() { return (byte)16; }

public final byte getSubOrder() { return (byte)2; }

public long getChapterId() { return chapterId; }
public void setChapterId(long _chapterId) { chapterId = _chapterId; }
/** 物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getRewardList() { return rewardList; }
/** 物品列表 */
public void addRewardList(NPCommon.NPCommon_ItemInfo _rewardList) { rewardList.add(_rewardList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chapterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _rewardList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.position();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.position(__rewardListCurPos + __rewardListCustLen);

		rewardList.add(_rewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(chapterId);
	_buf.putShort((short)rewardList.size());
	for(int _i = 0; _i < rewardList.size(); _i++) { 
		_buf.putInt(rewardList.get(_i).GetBufSize());
	rewardList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
	_recBuf.put((byte)2);
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

