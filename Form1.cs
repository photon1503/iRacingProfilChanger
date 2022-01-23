using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace iRacingProfilChanger
{
    public partial class Form1 : Form
    {
        helper h = new helper();
        string currentProfile;

        public Form1()
        {
            InitializeComponent();

            if (!System.IO.Directory.Exists(h.iRacingProfileFolder))
                System.IO.Directory.CreateDirectory(h.iRacingProfileFolder);
            
            updateProfiles();
            getCurrentProfile();
            

        }

        private void getCurrentProfile()
        {           // try to read _profile.ini

            try
            {
                using (StreamReader sr = File.OpenText(h.iRacingProfileFile))
                {
                    string s = "";
                    s = sr.ReadLine();

                    if (s != null) {
                        this.lblCurrentProfile.Text = s;
                        currentProfile = s;
                        this.cbProfiles.SelectedIndex = cbProfiles.FindStringExact(s);
                        }
                    else
                        this.lblCurrentProfile.Text = "Profile invalid!";
                }
            }
            catch
            {
                this.lblCurrentProfile.Text = "No profile found";
            }

        }

        private void updateProfiles()
        {
            List<string> profiles = getiRacingProfiles();

            cbProfiles.DataSource = profiles;            
        }

        private List<string>  getiRacingProfiles()
        {
            DirectoryInfo di = new DirectoryInfo(h.iRacingProfileFolder);
            DirectoryInfo[] arrDir = di.GetDirectories();

            List<string> l = new List<string>();

            foreach (DirectoryInfo dir in arrDir)
            {
                l.Add(dir.Name);
            }

            return l;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewProfile profileDialog = new NewProfile();
            

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (profileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.lblMessage.Text = "New profile created!";
                // Read the contents of testDialog's TextBox.
                //var _profileName = profileDialog.txtProfileName.Text;
            }
     
            profileDialog.Dispose();
            updateProfiles();
            getCurrentProfile();
         
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult dialogResult;

            //check current Profile
            if (cbProfiles.FindStringExact(currentProfile) > 0)
            {                
                //dialogResult = MessageBox.Show("Save current profile?", "Save Profile?", MessageBoxButtons.YesNo);
                //if (dialogResult == DialogResult.Yes)
                //{
                    h.CopyFilesRecursively(h.iRacingFolder, Path.Combine(h.iRacingProfileFolder, currentProfile));
                //}
            }

            string newProfile = this.cbProfiles.SelectedItem.ToString();
            
            //dialogResult = MessageBox.Show("Change profile to " + newProfile +"?", "Set Profile?", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
                //do something
                try
                {
                    Directory.Delete(h.iRacingFolderBackup, true);
                }
                catch { }

                //Directory.Move(h.iRacingFolder, h.iRacingFolderBackup);
                h.CopyFilesRecursively(h.iRacingFolder, h.iRacingFolderBackup);
                h.CopyFilesRecursively(Path.Combine(h.iRacingProfileFolder, newProfile), h.iRacingFolder);                
                h.createProfileFile(newProfile);
                

                updateProfiles();
                getCurrentProfile();
            //}
        }
    }
}
