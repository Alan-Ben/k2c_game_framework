package Common.InnObj;

import java.nio.ByteBuffer;
/*********
 * 旅店_菜品信息
 **/
public class Inn_DishInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 菜品ID */
private long dishId;
/** 等级 */
private int level;
/** 熟练度 */
private long finesse;
/** 是否解锁 */
private boolean hadUnlock;
/** 是否获得菜谱 */
private boolean hadGainRecipe;
/** 开始排队的ID */
private long startLineUpId;


public Inn_DishInfo() {
	dishId = (long)0;
	level = 0;
	finesse = (long)0;
	hadUnlock = false;
	hadGainRecipe = false;
	startLineUpId = (long)0;
}

public Inn_DishInfo(
	 long _dishId
	, int _level
	, long _finesse
	, boolean _hadUnlock
	, boolean _hadGainRecipe
	, long _startLineUpId
) {	dishId = _dishId;
	level = _level;
	finesse = _finesse;
	hadUnlock = _hadUnlock;
	hadGainRecipe = _hadGainRecipe;
	startLineUpId = _startLineUpId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 菜品ID */
public long getDishId() { return dishId; }
/** 菜品ID */
public void setDishId(long _dishId) { dishId = _dishId; }
/** 等级 */
public int getLevel() { return level; }
/** 等级 */
public void setLevel(int _level) { level = _level; }
/** 熟练度 */
public long getFinesse() { return finesse; }
/** 熟练度 */
public void setFinesse(long _finesse) { finesse = _finesse; }
/** 是否解锁 */
public boolean getHadUnlock() { return hadUnlock; }
/** 是否解锁 */
public void setHadUnlock(boolean _hadUnlock) { hadUnlock = _hadUnlock; }
/** 是否获得菜谱 */
public boolean getHadGainRecipe() { return hadGainRecipe; }
/** 是否获得菜谱 */
public void setHadGainRecipe(boolean _hadGainRecipe) { hadGainRecipe = _hadGainRecipe; }
/** 开始排队的ID */
public long getStartLineUpId() { return startLineUpId; }
/** 开始排队的ID */
public void setStartLineUpId(long _startLineUpId) { startLineUpId = _startLineUpId; }


public final int GetBufSize() {
	int _size = 30;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 32;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dishId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) finesse = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadUnlock = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadGainRecipe = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startLineUpId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dishId);
	_buf.putInt(level);
	_buf.putLong(finesse);
	_buf.put(hadUnlock?(byte)1:(byte)0);
	_buf.put(hadGainRecipe?(byte)1:(byte)0);
	_buf.putLong(startLineUpId);
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

