using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Policy;

namespace BuckShotCompiler.LangObject
{
    public class Tools
    {
        List<Type> LangTypes = new List<Type>();
        public Tools()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] LocalAssemblies = currentDomain.GetAssemblies();

            foreach(Assembly LocalAssembly in LocalAssemblies){
                foreach(Type LocalType in LocalAssembly.GetTypes()){
                    if(LocalType.IsSubclassOf(typeof(Base))){
                        this.LangTypes.Add(LocalType);
                    }
                }
            }
        }

        public Type PropType(string Prop)
        {
            foreach(Type LangType in this.LangTypes){
                FieldInfo[] Props = LangType.GetFields();
                foreach(FieldInfo LocalProp in Props){
                    if(LocalProp.Name == Prop){
                        return LangType;
                    }
                }
            }
            return typeof(Base);
        }

        public void SetLangObjectValue(Type LangType,WebObject.Base CurrentObject, string PropName, string PropValue)
        {
            foreach(FieldInfo LocalProp in CurrentObject.GetType().GetRuntimeFields()){
                if(LocalProp.GetValue(CurrentObject).GetType() == LangType){
                    object LangObject = LocalProp.GetValue(CurrentObject);
                    foreach(FieldInfo LocalLangProp in LangObject.GetType().GetRuntimeFields()){
                        if(LocalLangProp.Name == PropName){
                            LocalLangProp.SetValue(LangObject, PropValue);
                        }
                    }
                    LocalProp.SetValue(CurrentObject, LangObject);

                }
            }
        }

        public string GetLangObjectValue(Type LangType, WebObject.Base CurrentObject, string PropName)
        {
            foreach (FieldInfo LocalProp in CurrentObject.GetType().GetRuntimeFields())
            {
                if (LocalProp.GetValue(CurrentObject).GetType() == LangType)
                {
                    object LangObject = LocalProp.GetValue(CurrentObject);
                    foreach (FieldInfo LocalLangProp in LangObject.GetType().GetRuntimeFields())
                    {
                        if (LocalLangProp.Name == PropName)
                        {
                            LocalLangProp.GetValue(LangObject);
                            return LocalLangProp.GetValue(LangObject).ToString();
                        }
                    }
                }
            }
            return "error";
        }
    }
}
