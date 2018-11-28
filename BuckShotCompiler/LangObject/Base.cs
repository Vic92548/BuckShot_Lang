using System;
using System.Collections.Generic;
using System.Reflection;

namespace BuckShotCompiler.LangObject
{
    public class Base
    {
        public Base()
        {
        }

        public List<FieldInfo> GetAllProp()
        {
            List<FieldInfo> PropList = new List<FieldInfo>();
            FieldInfo[] LangProps = this.GetType().GetFields();
            foreach (FieldInfo LangProp in LangProps)
            {
                PropList.Add(LangProp);
            }
            return PropList;
        }
    }
}
