using System;
using System.Collections.Generic;
namespace BuckShotCompiler
{
    public static class BasicFunctions
    {

        public static void Group(string Name,string[] Arguments, WebObject CurrentObject){
            WebObject ArrayObject = Tools.FindWebObjectByName(Arguments[1], CurrentObject.LocalObjectList);
            List<string> DataArray = new List<string>();
            foreach(string Data in ArrayObject.ArrayDatas){
                DataArray.Add(Data);

            }
            WebObject MasterClass = Tools.FindWebObjectByName(Arguments[0], CurrentObject.CurrentProject.ObjectList);
            List<WebObject> ChildObjects = new List<WebObject>();
            foreach(string LocalData in DataArray){
                WebObject NewObject = Tools.CreateWebObject(Name + DataArray.IndexOf(LocalData),MasterClass,MasterClass.HTML_Path, MasterClass.CSS_Path);
                NewObject.ClassName = MasterClass.GetName();
                Tools.SetEmbemdedProp(Arguments[2],NewObject);
                Tools.SetEmbemdedProp(LocalData,NewObject);
                CurrentObject.LocalObjectList.Add(NewObject);
            }
        }
    }
}
