using System;
namespace BuckShotCompiler.File
{
    public class Base
    {
        public string FileContent = "";
        public Base()
        {
        }

        public virtual string GetContent(){
            return this.FileContent;
        }

		
    }
}
