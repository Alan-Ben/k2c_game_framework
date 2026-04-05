using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.InnObj
{

/// <summary>
/// 旅店_菜品信息
/// </summary>
public class Inn_DishInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 菜品ID
/// </summary>
private long dishId;
/// <summary>
/// 等级
/// </summary>
private int level;
/// <summary>
/// 熟练度
/// </summary>
private long finesse;
/// <summary>
/// 是否解锁
/// </summary>
private bool hadUnlock;
/// <summary>
/// 是否获得菜谱
/// </summary>
private bool hadGainRecipe;
/// <summary>
/// 开始排队的ID
/// </summary>
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
	, bool _hadUnlock
	, bool _hadGainRecipe
	, long _startLineUpId
) {	dishId = _dishId;
	level = _level;
	finesse = _finesse;
	hadUnlock = _hadUnlock;
	hadGainRecipe = _hadGainRecipe;
	startLineUpId = _startLineUpId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 菜品ID
/// </summary>
public long getDishId() { return dishId; }
/// <summary>
/// 菜品ID
/// </summary>
public void setDishId(long _dishId) { dishId = _dishId; }
/// <summary>
/// 等级
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 等级
/// </summary>
public void setLevel(int _level) { level = _level; }
/// <summary>
/// 熟练度
/// </summary>
public long getFinesse() { return finesse; }
/// <summary>
/// 熟练度
/// </summary>
public void setFinesse(long _finesse) { finesse = _finesse; }
/// <summary>
/// 是否解锁
/// </summary>
public bool getHadUnlock() { return hadUnlock; }
/// <summary>
/// 是否解锁
/// </summary>
public void setHadUnlock(bool _hadUnlock) { hadUnlock = _hadUnlock; }
/// <summary>
/// 是否获得菜谱
/// </summary>
public bool getHadGainRecipe() { return hadGainRecipe; }
/// <summary>
/// 是否获得菜谱
/// </summary>
public void setHadGainRecipe(bool _hadGainRecipe) { hadGainRecipe = _hadGainRecipe; }
/// <summary>
/// 开始排队的ID
/// </summary>
public long getStartLineUpId() { return startLineUpId; }
/// <summary>
/// 开始排队的ID
/// </summary>
public void setStartLineUpId(long _startLineUpId) { startLineUpId = _startLineUpId; }


public int GetBufSize() {
	int _size = 30;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 32;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dishId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	finesse = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadUnlock = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadGainRecipe = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startLineUpId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dishId);
	_buf.putInt(level);
	_buf.putLong(finesse);
	_buf.put(hadUnlock?(byte)1:(byte)0);
	_buf.put(hadGainRecipe?(byte)1:(byte)0);
	_buf.putLong(startLineUpId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("dishId").Append(":").Append(dishId.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("finesse").Append(":").Append(finesse.ToString()).Append(", ");
	builder.Append("hadUnlock").Append(":").Append(hadUnlock.ToString()).Append(", ");
	builder.Append("hadGainRecipe").Append(":").Append(hadGainRecipe.ToString()).Append(", ");
	builder.Append("startLineUpId").Append(":").Append(startLineUpId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

