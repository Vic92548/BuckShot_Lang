using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Globalization;
namespace BuckShotCompiler
{
    public class WebObject
    {
        protected List<string> Lines = new List<string>();
        public WebProject CurrentProject;
        public List<WebObject> LocalObjectList = new List<WebObject>();
        protected string HTMLObject = "";
        protected string CSSObject = "";
        public string HTML_Path;
        public string CSS_Path;
        protected string ObjectName;
        public List<string> ArrayDatas = new List<string>();
        public string ClassName = "";

        public HTMLObject HTML = new HTMLObject();
        public CSSObject CSS = new CSSObject();

        public WebObject(string Name, WebProject NewProject, string HTMLPath, string CSSPath)
        {
            this.ObjectName = Name;
            this.CurrentProject = NewProject;
            this.HTML_Path = HTMLPath;
            this.CSS_Path = CSSPath;
        }

        public virtual void AddLine(string Line){
            Lines.Add(Line);
        }

        public virtual void CompileObject(){
            this.CompileLines();
            this.HTML.PropertiesValue[0] = this.ObjectName;
            this.HTMLObject = this.HTML.GetCompiledCode();
            this.CSSObject = this.CSS.GetCompiledCode(this.ObjectName, "");
        }
		public virtual void CompileLines()
		{
            foreach(string Line in Lines){
                string newLine = SyntaxTools.GetNewLine(Line);
                string[] Words = newLine.Split('=');
                Tools.ExecuteAllAnalyzerFunc(Words, this);
			}
		}

        public virtual string GetHTML(){
            return this.HTMLObject;
        }

		public virtual string GetCSS()
		{
            return this.CSSObject;
		}

        public virtual string GetName(){
            return this.ObjectName;
        }

        /*public virtual WebObject Clone(string Name){
            WebObject ClonedObject = new WebObject(Name,this.ObjectList);
            FieldInfo[] FieldsProp = this.GetType().GetFields();
            for (int i = 0; i < FieldsProp.Length; i++){
                FieldsProp[i].SetValue(ClonedObject, FieldsProp[i].GetValue(this));
            }

        }*/
    }
}
