using System;
using System.Collections.Generic;
namespace BuckShotCompiler
{
    public static class BasicFunctions
    {

        public static void Group(string Name,string[] Arguments, WebObject.Base CurrentObject,WebProject CurrentProject){
            WebObject.Base ArrayObject = Tools.FindWebObjectByName(Arguments[1], CurrentObject.LocalObjectList);
            List<string> DataArray = new List<string>();
            foreach(string Data in ArrayObject.ArrayDatas){
                DataArray.Add(Data);

            }
            WebObject.Base MasterClass = Tools.FindWebObjectByName(Arguments[0], CurrentObject.CurrentProject.ObjectList);
            List<WebObject.Base> ChildObjects = new List<WebObject.Base>();
            foreach(string LocalData in DataArray){
                WebObject.Base NewObject = Tools.CreateWebObject(Name + DataArray.IndexOf(LocalData),MasterClass,MasterClass.HTML_Path, MasterClass.CSS_Path);
                NewObject.ClassName = MasterClass.GetName();
                Tools.SetEmbemdedProp(Arguments[2],NewObject,CurrentProject);
                Tools.SetEmbemdedProp(LocalData,NewObject,CurrentProject);
                CurrentObject.LocalObjectList.Add(NewObject);
            }
        }

        public static void LoadFromJSON(string Name, string[] Arguments, WebObject.Base CurrentObject){

        }
    }
}
