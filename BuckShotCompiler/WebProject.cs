using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
namespace BuckShotCompiler
{
    public class WebProject
    {
        public List<WebObject> ObjectList = new List<WebObject>();
        private WebDataStruct LocalData = new WebDataStruct();
        public Stopwatch ProjectStopWatch = new Stopwatch();
        public WebProject()
        {
            this.LocalData.ChildsData = new List<WebDataStruct>();
            this.LocalData.ChildsCategories = new List<WebDataStruct>();
        }

        public void AddData(string FullKey, string Value){
            WebDataStruct SearchedDataObj = WebDataStruct.FindDataFromFullKey(this.LocalData, FullKey);
            SearchedDataObj.Value = Value;
        }

        public void AddDataCat(string FullKey,string CatName){
            WebDataStruct ParentCat = WebDataStruct.FindCategoryFromFullKey(this.LocalData, FullKey);
            WebDataStruct NewCat = new WebDataStruct();
            NewCat.CatName = CatName;
            ParentCat.ChildsCategories.Add(NewCat);
        }

        public string GetData(string FullKey){
            return WebDataStruct.FindDataFromFullKey(this.LocalData, FullKey).Value;

        }

        public void DisplayCompileTime(){
            this.ProjectStopWatch.Stop();
            string CompileTime = ProjectStopWatch.ElapsedMilliseconds + "ms";
            Console.WriteLine("CompileTime = " + CompileTime);
        }
    }
}
