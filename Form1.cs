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

            RefreshUI();
        }

        private void RefreshUI()
        {
            updateProfiles();
            getCurrentProfile();
        }

        private void getCurrentProfile()
        {
            string s = h.getProfile();

            if (s != null)
            {
                this.lblCurrentProfile.Text = s;
                currentProfile = s;
                this.cbProfiles.SelectedIndex = cbProfiles.FindStringExact(s);
            }
        }

        private void updateProfiles()
        {
            List<string> profiles = getiRacingProfiles();

            cbProfiles.DataSource = profiles;
        }

        private List<string> getiRacingProfiles()
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

            if (profileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.lblMessage.Text = "New profile created!";
            }

            profileDialog.Dispose();

            RefreshUI();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (cbProfiles.FindStringExact(currentProfile) > 0)
            {
                this.lblMessage.Text = $"Saving current profile...";
                h.CopyFilesRecursively(h.iRacingFolder, Path.Combine(h.iRacingProfileFolder, currentProfile));
            }

            string newProfile = this.cbProfiles.SelectedItem.ToString();

            try
            {
                this.lblMessage.Text = $"Deleting backup...";
                Directory.Delete(h.iRacingFolderBackup, true);
            }
            catch { }

            this.lblMessage.Text = $"Creating backup...";
            h.CopyFilesRecursively(h.iRacingFolder, h.iRacingFolderBackup);
            this.lblMessage.Text = $"Restoring profile {newProfile}";
            h.CopyFilesRecursively(Path.Combine(h.iRacingProfileFolder, newProfile), h.iRacingFolder);
            h.createProfileFile(newProfile);

            Cursor = Cursors.Arrow;
            this.lblMessage.Text = $"Profile changed to {newProfile}.";

            RefreshUI();
        }
    }
}
