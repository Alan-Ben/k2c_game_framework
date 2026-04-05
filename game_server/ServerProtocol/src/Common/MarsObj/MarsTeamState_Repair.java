package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星队伍-维修状态数据
 **/
public class MarsTeamState_Repair implements ALBasicProtocolPack._IALProtocolStructure {
/** 修复士兵数 */
private long repairNum;
/** 公会求助数据ID */
private long guildHelpId;
/** 公会求助时长（秒） */
private int guildHelpSecs;
/** 是否完成，用于钻石完成，true-完成 */
private boolean isDone;
/** 道具求助时长（秒） */
private int itemHelpSecs;
/** 消耗的资源列表，用于取消维修时返还 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> costItemList;
/** 是否取消 */
private boolean isCancel;


public MarsTeamState_Repair() {
	repairNum = (long)0;
	guildHelpId = (long)0;
	guildHelpSecs = 0;
	isDone = false;
	itemHelpSecs = 0;
	costItemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	isCancel = false;
}

public MarsTeamState_Repair(
	 long _repairNum
	, long _guildHelpId
	, int _guildHelpSecs
	, boolean _isDone
	, int _itemHelpSecs
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _costItemList
	, boolean _isCancel
) {	repairNum = _repairNum;
	guildHelpId = _guildHelpId;
	guildHelpSecs = _guildHelpSecs;
	isDone = _isDone;
	itemHelpSecs = _itemHelpSecs;
	costItemList = _costItemList;
	isCancel = _isCancel;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 修复士兵数 */
public long getRepairNum() { return repairNum; }
/** 修复士兵数 */
public void setRepairNum(long _repairNum) { repairNum = _repairNum; }
/** 公会求助数据ID */
public long getGuildHelpId() { return guildHelpId; }
/** 公会求助数据ID */
public void setGuildHelpId(long _guildHelpId) { guildHelpId = _guildHelpId; }
/** 公会求助时长（秒） */
public int getGuildHelpSecs() { return guildHelpSecs; }
/** 公会求助时长（秒） */
public void setGuildHelpSecs(int _guildHelpSecs) { guildHelpSecs = _guildHelpSecs; }
/** 是否完成，用于钻石完成，true-完成 */
public boolean getIsDone() { return isDone; }
/** 是否完成，用于钻石完成，true-完成 */
public void setIsDone(boolean _isDone) { isDone = _isDone; }
/** 道具求助时长（秒） */
public int getItemHelpSecs() { return itemHelpSecs; }
/** 道具求助时长（秒） */
public void setItemHelpSecs(int _itemHelpSecs) { itemHelpSecs = _itemHelpSecs; }
/** 消耗的资源列表，用于取消维修时返还 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getCostItemList() { return costItemList; }
/** 消耗的资源列表，用于取消维修时返还 */
public void addCostItemList(NPCommon.NPCommon_ItemInfo _costItemList) { costItemList.add(_costItemList); }
/** 是否取消 */
public boolean getIsCancel() { return isCancel; }
/** 是否取消 */
public void setIsCancel(boolean _isCancel) { isCancel = _isCancel; }


public final int GetBufSize() {
	int _size = 26;
	_size += 2;
	for(int _i = 0; _i < costItemList.size(); _i++) {
	_size += 4 + costItemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 28;
	_size += 2;
	for(int _i = 0; _i < costItemList.size(); _i++) {
	_size += 4 + costItemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) repairNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildHelpId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildHelpSecs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isDone = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemHelpSecs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _costItemListCount = _buf.getShort();
	for(int _i = 0; _i < _costItemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _costItemList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __costItemListCustLen = _buf.getInt();
	int __costItemListCurPos = _buf.position();
	_costItemList.ReadUnzipBuf(_buf, __costItemListCurPos + __costItemListCustLen);
	_buf.position(__costItemListCurPos + __costItemListCustLen);

		costItemList.add(_costItemList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isCancel = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(repairNum);
	_buf.putLong(guildHelpId);
	_buf.putInt(guildHelpSecs);
	_buf.put(isDone?(byte)1:(byte)0);
	_buf.putInt(itemHelpSecs);
	_buf.putShort((short)costItemList.size());
	for(int _i = 0; _i < costItemList.size(); _i++) { 
		_buf.putInt(costItemList.get(_i).GetBufSize());
	costItemList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.put(isCancel?(byte)1:(byte)0);
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

