using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace iRacingProfilChanger
{
    public partial class NewProfile : Form
    {
        helper h = new helper();

        public NewProfile()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var _profileName = this.txtProfileName.Text;

            string dst = Path.Combine(h.iRacingProfileFolder, _profileName);
            //Directory.Move(src, dst);
            h.CopyFilesRecursively(h.iRacingFolder, dst);
            h.createProfileFile(_profileName);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
