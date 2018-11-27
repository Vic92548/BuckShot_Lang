using System;
namespace BuckShotCompiler
{
    public class HTML : FILE
    {
        protected string HeadLinks = "";

        protected string BodyContent = "";



        public HTML()
        {
        }

        public void AddHead(string Title){
            FileContent += "<head>\n<title>" + Title + "</title>\n" + this.HeadLinks + "<meta charset=\"utf-8\">\n" + "</head>\n";
        }

        public void AddLink(string type, string href){
            HeadLinks += "<link rel=\"" + type + "\" href=\"" + href + "\">\n";
        }

        public void AddBasicEl(string Type, string Class, string Style, string Id, string Content){
            this.BodyContent += "<" + Type + " class=\"" + Class + "\" style=\"" + Style + "\" id=\"" + Id + "\">" + Content + "</" + Type + ">\n"; 
        }

        public void AddToBody(string element){
            this.BodyContent += element;
        }

        public void CloseBody(){
            this.BodyContent = "<body style=\"margin:0px;padding:0px\">\n" + this.BodyContent + "</body>\n";
            this.FileContent += this.BodyContent;
        }

        public string GetBodyContent(){
            Console.WriteLine(this.BodyContent);
            return this.BodyContent;
        }


    }
}
