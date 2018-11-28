using System;
using System.IO;
using System.Collections.Generic;
namespace BuckShotCompiler.WebObject
{
    public class Page : Base
    {
        protected string PageName;
        protected File.HTML htmlFile = new File.HTML();
        protected File.Base cssFile = new File.Base();
        protected List<string> NeededCSSClass = new List<string>();
        public Page(string Name, WebProject CurrentProject, string HTMLPath, string CSSPath) : base(Name, CurrentProject, HTMLPath, CSSPath)
        {
            this.PageName = Name;
        }
        public override void CompileObject()
        {
            //base.CompileObject();
            this.CompileLines();
            this.htmlFile.AddLink("stylesheet","../CSS/" + this.ObjectName + ".css");
            this.htmlFile.AddHead(this.PageName);
            this.htmlFile.CloseBody();
            System.IO.File.WriteAllText(this.HTML_Path + '/' + this.PageName + ".html", this.htmlFile.GetContent());
            System.IO.File.WriteAllText(this.CSS_Path + '/' + this.PageName + ".css", this.cssFile.GetContent());
        }
		public override void CompileLines()
		{
            base.CompileLines();
            foreach (Base LocalObject in this.LocalObjectList){
                LocalObject.HTML.PropertiesValue[1] = LocalObject.CSS.GetEmbemdedCSS(Tools.FindWebObjectByName(LocalObject.ClassName, this.CurrentProject.ObjectList));
                this.CompileAllChilds(LocalObject,LocalObject.LocalObjectList);
                this.htmlFile.AddToBody(LocalObject.HTML.GetCompiledCode());
                if(!this.CSSClassAlreadyAdded(LocalObject.ClassName)){
                    this.NeededCSSClass.Add(LocalObject.ClassName);
                }
            }
            foreach(string LocalClassName in NeededCSSClass){
                this.cssFile.FileContent += Tools.FindWebObjectByName(LocalClassName,this.CurrentProject.ObjectList).CSS.GetCompiledCode(LocalClassName, "");
            }

		}

        private void CompileAllChilds(Base MasterObject,List<Base> LocalObjects){
            foreach (Base ChildLocalObject in LocalObjects)
			{
                CompileAllChilds(ChildLocalObject, ChildLocalObject.LocalObjectList);
                ChildLocalObject.HTML.PropertiesValue[1] = ChildLocalObject.CSS.GetEmbemdedCSS(Tools.FindWebObjectByName(ChildLocalObject.ClassName, this.CurrentProject.ObjectList));
                MasterObject.HTML.content += ChildLocalObject.HTML.GetCompiledCode();
				if (!this.CSSClassAlreadyAdded(ChildLocalObject.ClassName))
				{
					this.NeededCSSClass.Add(ChildLocalObject.ClassName);
				}
			}
        }

        public bool CSSClassAlreadyAdded(string ClassName){
            foreach(string LocalClass in this.NeededCSSClass){
                if(LocalClass == ClassName){
                    return true;
                }
            }
            return false;
        }
    }
}
