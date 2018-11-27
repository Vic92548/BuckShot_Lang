using System;
namespace BuckShotCompiler
{
    public class FILE
    {
        public string FileContent = "";
        public FILE()
        {
        }

        public virtual string GetContent(){
            return this.FileContent;
        }

		
    }
}
