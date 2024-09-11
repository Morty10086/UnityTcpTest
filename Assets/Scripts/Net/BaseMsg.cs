using System.Collections;
using System.Collections.Generic;


public class BaseMsg : BaseData
{
    public override int GetBytesNum()
    {
        throw new System.NotImplementedException();
    }

    public override int Reading(byte[] bytes, int index)
    {
        throw new System.NotImplementedException();
    }

    public override byte[] Writing()
    {
        throw new System.NotImplementedException();
    }

    protected virtual int GetMsgID()
    {
        return 0;
    }
}
