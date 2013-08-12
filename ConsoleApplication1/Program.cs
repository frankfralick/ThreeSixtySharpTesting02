using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeSixtySharp;
using ThreeSixtySharp.Objects;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = "frankfralick@beckgroup.com";
            string password = "MyPassword";  //fill this in

            Field field = new Field(username, password);
            AuthTicket t = field.GetTicket();
            List<Project> projects = field.GetProjects(t);

            //I only have one project.
            Project testProject = projects[0];

            List<File> files = field.GetAllFiles(t, testProject);

            //File I want to upload as a revision.  This is just some random text file I have been working with, I will include in repo:
            string testFile = "C:\\Projects\\ThreeSixtyTesting\\ProjectJSON.txt";

            //Get a file and compare it.
            System.IO.FileInfo origin_file_info = new System.IO.FileInfo(testFile);

            //Try to find a corresponding file.
            //This says 'Give me a list of File instances that either have the same 
            //name as testFile or have the same name sans the '_#' revision numbering that is added when 
            //adding a new version through the web interface'.  In my test case there are 3 revisions at 
            //this point, and each JSON return appears to be the same.
            List<File> destination_files = files.Where(p => p.Get_Original_Name() == origin_file_info.Name || p.Filename == origin_file_info.Name).ToList();

            if (destination_files.Count != 0)
            {
                File destination_file = destination_files[0];

                //It blows up here.  The methods "field.PublishNew" and "field.PublishBaseRevision" work fine and both return the same JSON.  
                //The error you will get is "Root element missing" and this just means that it got nothing back in the response and tried to 
                //create an instance of File with it and it obviously fails to do so.  If you are not familiar with RestSharp it should still
                //be easy to look at the Field.PublishRevision method to see how the request is being formed.
                //This returns status 500.
                File new_revised_file = field.PublishRevision(t, testProject, destination_file.Document_Path, origin_file_info.FullName, destination_file.Document_Id);
            }
        }
    }
}
