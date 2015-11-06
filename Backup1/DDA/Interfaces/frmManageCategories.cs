using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DDA.Interfaces
{
    public partial class frmManageCategories : Form
    {
        int curIndex;
        string oldName;

        public frmManageCategories()
        {
            InitializeComponent();
            DDA.DataObjects.AppData.DisableCloseButton(Handle);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            frmTerritory frmTerr = new frmTerritory();
            frmTerr.Show();
            this.Close();
            
        }

        private void frmManageCategories_Load(object sender, EventArgs e)
        {
            DataSet ds;
            ds = DDA.DataAccess.Category_da.GetCategoryList();

            int i;

            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
               lstCategory.Items.Add(ds.Tables[0].Rows[i]["CategoryName"]);
            }

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string catName;
            if (btnSave.Text != "UPDATE")
            {
                try
                {

                    catName = lstCategory.SelectedItem.ToString();

                    int catID;

                    // stupid
                    DataLogic.DBA.DataLogic.PrepareSQL(ref catName);

                    catID = DDA.DataAccess.Category_da.GetCategoryID(catName);

                    if (MessageBox.Show("You are about to remove the category '" + catName + "'.  Are you sure you want to continue?", "Confirm Remove", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DDA.DataAccess.Category_da.RemoveCategory(catID);
                        lstCategory.Items.RemoveAt(lstCategory.SelectedIndex);

                        curIndex = -1;
                    }
                    else
                    {
                        MessageBox.Show("Remove Cancelled.");
                    }
                }
                catch
                {

                }
            }
            else
            {
                MessageBox.Show("You cannot remove a category while you are in edit mode");
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "")
            {

                string catName;
                catName = txtName.Text;

                if (btnSave.Text == "UPDATE")
                {
                    // update
                    int oldID;

                    oldID = DDA.DataAccess.Category_da.GetCategoryID(oldName);
                    DDA.DataAccess.Category_da.UpdateCategory(oldID, catName);
                    lstCategory.Items[curIndex] = txtName.Text;


                }
                else
                {
                    int catID;
                    catID = DataLogic.DBA.DataLogic.GetNextID("Category", "CategoryID");

                    DDA.DataAccess.Category_da.AddCategory(catID, catName);

                    lstCategory.Items.Add(catName);

                }


                MessageBox.Show("Data Saved");
            }
            else
            {
                MessageBox.Show("Please enter a category name before proceeding.");
            }

            curIndex = -1;
            txtName.Text = "";
            txtName.BackColor = System.Drawing.SystemColors.Window;
            btnSave.Text = "ADD";

        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            frmDistributorList frmDistList = new frmDistributorList();

            frmDistList.Show();
            this.Close();
        }

        private void lstCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstCategory_DoubleClick(object sender, EventArgs e)
        {
            txtName.Text = lstCategory.SelectedItem.ToString();
            txtName.BackColor = System.Drawing.Color.Yellow;
            btnSave.Text = "UPDATE";
            curIndex = lstCategory.SelectedIndex;
            oldName = txtName.Text;

        }

    }
}