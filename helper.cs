using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace iRacingProfilChanger
{
     class helper
    {
        public string iRacingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "iRacing");
        public string iRacingFolderBackup = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "iRacing-Backup");
        public string iRacingProfileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "iRacing-Profiles");
        public string iRacingProfileFile = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "iRacing", "_profile.ini"));

        public void createProfileFile(string profileName)
        {
            if (File.Exists(iRacingProfileFile))
            {
                File.Delete(iRacingProfileFile);
            }

            // Create a new file     
            using (StreamWriter fs = File.CreateText(iRacingProfileFile))
            {
                // Add some text to file    
                fs.WriteLine(profileName);
            }
        }

        public string getProfile()
        {
            string rc = null;

            try
            {
                using (StreamReader sr = File.OpenText(iRacingProfileFile))
                {
                    rc = sr.ReadLine();
                }
            }
            catch {}

            return rc;
        }


        public  void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}
