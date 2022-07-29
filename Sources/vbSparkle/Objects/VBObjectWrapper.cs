using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vbSparkle
{
    //public enum VBObjectType
    //{
    //    VbUnknown,
    //    VbMethod,
    //    VbModule,
    //    VbVariable
    //}

    //public interface IObjectBase 
    //{

    //}
    
    //public class VBObjectWrapper : IVBStatement
    //{
    //    public VBObjectType Type { get; set; }
    //    public IObjectBase Object { get; set; }
    //    public string Identifier { get; set; }

    //    public VBObjectWrapper(string identifier)
    //    {
    //        Type = VBObjectType.VbUnknown;
    //        Identifier = identifier;
    //    }

    //    public VBObjectWrapper(string identifier, IObjectBase obj)
    //    {
    //        Identifier = identifier;
    //        Define(obj);
    //    }

    //    public void RefreshType()
    //    {
    //        Type = VBObjectType.VbUnknown;

    //        if (Object is VBMethod)
    //            Type = VBObjectType.VbMethod;


    //        if (Object is VbUserVariable)
    //            Type = VBObjectType.VbVariable;
    //    }

    //    internal void Define(IObjectBase obj)
    //    {
    //        Object = obj;
    //        RefreshType();
    //    }
        
    //    public DCodeBlock GetCodeBlock()
    //    {
    //        return new DCodeBlock(Identifier);
    //    }

    //    public DExpression Evaluate()
    //    {
    //        switch (Type)
    //        {
    //            case VBObjectType.VbVariable:
    //                return (Object as VbUserVariable).CurrentValue;
    //                break;
    //            default:
    //                return GetCodeBlock();
    //                break;
    //        }
    //    }
        
    //}
}
