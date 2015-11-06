using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace DDA.Interfaces
{
    public partial class frmTerritory : Form
    {


        private bool LoadingForm;


        public frmTerritory()
        {
            InitializeComponent();

            DDA.DataObjects.AppData.DisableCloseButton(Handle);
            DDA.DataObjects.AppData.ViewContractMode = false;

            if (DDA.DataObjects.AppData.FromMainDistList == true)
            {
                btnCancel.Visible = false;
            }

            DDA.DataObjects.AppData.DataChanged = false;

               

        }


        private void frmTerritory_Load(object sender, EventArgs e)
        {
            LoadingForm = true;

            DataSet ds;
            ds = DDA.DataAccess.Category_da.GetCategoryList();

            int i;

            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chkCategories.Items.Add(ds.Tables[0].Rows[i]["CategoryName"]);
            }

            ds = DDA.DataAccess.Contract_da.GetContractList(DDA.DataObjects.AppData.DistributorID);

            cboContractNumber.Items.Add("NEW");

            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                cboContractNumber.Items.Add(ds.Tables[0].Rows[i]["ContractNumber"]);
            }


            // Populate state dropdown
            ds = DDA.DataAccess.Location_da.GetStateList();
            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                cboState.Items.Add(ds.Tables[0].Rows[i]["FullName"]);
            }


            LoadSavedData();

            lblMainDistributor.Text = DDA.DataAccess.Distributor_da.GetDistributorName(DDA.DataObjects.AppData.DistributorID);

        }

        private void LoadSavedData()
        {
            DDA.DataObjects.AppData.DistributorEditType = "Main";
            DDA.DataObjects.AppData.DistributorMode = "Edit";

            if (DDA.DataObjects.AppData.CurrentContract.ContractNumber != "")
            {

                // load the data we have stored in current contract
                cboState.Text = DDA.DataObjects.AppData.CurrentContract.StateName;
                cboContractNumber.Text = DDA.DataObjects.AppData.CurrentContract.ContractNumber;

                int i;
                int j;
                string catName;

                ArrayList categories;

                categories = DDA.DataObjects.AppData.CurrentContract.Categories;

                for (i = 0; i < categories.Count; i++)
                {
                    catName = DDA.DataAccess.Category_da.GetCategoryName(Convert.ToInt32(categories[i]));

                    for (j = 0; j < chkCategories.Items.Count; j++)
                    {
                        if (catName == chkCategories.Items[j].ToString())
                        {
                            chkCategories.SetItemChecked(j, true);
                        }
                    }
                }

                if (DDA.DataObjects.AppData.CurrentContract.Counties.Count > 0)
                {
                    cboState.Text = DDA.DataAccess.Location_da.GetStateFullName(Convert.ToInt32(DDA.DataObjects.AppData.CurrentContract.Counties[0].ToString()));
                }

                DDA.DataObjects.AppData.CurrentContract.ClearDeleteCounties();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            frmDistributorInformation frmDI = new frmDistributorInformation();
            frmDI.Show();
            this.Close();

        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            DDA.DataObjects.AppData.CurrentContract.ClearCategories();

            if (cboState.Text != "")
            {
                if (chkCategories.CheckedItems.Count > 0)
                {

                    int i;

                    if (cboContractNumber.Text == "NEW")
                    {
                        int newID;

                        newID = Convert.ToInt32(DataLogic.DBA.DataLogic.GetNextID("Contract", "ContractID"));

                        DDA.DataObjects.AppData.CurrentContract.ContractID = newID;

                        DDA.DataObjects.AppData.CurrentContract.ClearContractData();
                        DDA.DataObjects.AppData.ContractMode = "Add";
                        DDA.DataObjects.AppData.CurrentContract.ContractNumber = lblNewContractNumber.Text;

                        DDA.DataObjects.AppData.CurrentContract.ContractDate = Convert.ToString(System.DateTime.Now.ToShortDateString());
                        DDA.DataObjects.AppData.CurrentContract.ModifyDate = Convert.ToString(System.DateTime.Now.ToShortDateString());
                        

                    }
                    else if (cboContractNumber.Text == "")
                    {
                        MessageBox.Show("Please select \"New\" or a contract number before proceeding.");
                        return;
                    }
                    else
                    {
                        // Add in logic for editing
                        // Get contract id and add it

                        DDA.DataObjects.AppData.ContractMode = "Edit";
                        DDA.DataObjects.AppData.CurrentContract.ContractNumber = cboContractNumber.Text;
                        // write a routine here to load the previous data
                    }


                    DDA.DataObjects.AppData.CurrentContract.StateName = cboState.Text;

                    for (i = 0; i < chkCategories.CheckedItems.Count; i++)
                    {
                        DDA.DataObjects.AppData.CurrentContract.AddCategory(chkCategories.CheckedItems[i].ToString());
                    }


                    frmCounties frmCounties = new frmCounties();
                    frmCounties.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please select at least one category before proceeding.");
                }
            } else {
                MessageBox.Show("Please select a state before proceeding.");
            }
        }

        private void cboContractNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            LoadingForm = true;


            // if the index changes, clear the counties
            DDA.DataObjects.AppData.CurrentContract.ClearCounties();

            int i;
            // clear the checks
            for (i = 0; i < chkCategories.Items.Count; i++)
                chkCategories.SetItemCheckState(i, CheckState.Unchecked);


            if (cboContractNumber.Text == "NEW")
            {
                
                btnView.Enabled = false;
                btnDelete.Enabled = false;
                lblNewContractNumber.Text =  DDA.DataObjects.AppData.CurrentContract.CreateNewContractNumber(DDA.DataObjects.AppData.DistributorID);
                lblNewContractNumberCaption.Visible = true;
                lblNewContractNumber.Visible = true;

                DDA.DataObjects.AppData.CurrentContract.ContractID = -1;
                DDA.DataObjects.AppData.CurrentContract.ContractMode = "NEW";
            } else
            {

                
                DDA.DataAccess.Contract_da.GetContract(cboContractNumber.Text);

                LoadSavedData();

                btnView.Enabled = true;
                btnDelete.Enabled = true;
                lblNewContractNumberCaption.Visible = false;
                lblNewContractNumber.Visible = false;
                DDA.DataObjects.AppData.CurrentContract.ContractMode = "UPDATE";
            }

        }

        private void btnEditCategories_Click(object sender, EventArgs e)
        {
            frmManageCategories frmMC = new frmManageCategories();

            frmMC.Show();
            this.Close();
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            frmDistributorList frmDistList = new frmDistributorList();

            frmDistList.Show();
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (cboContractNumber.Text == "")
            {
                MessageBox.Show("Please select a contract number before proceeding.");
                return;
            }
            else
            {
                // Add in logic for editing
                // Get contract id and add it

                DDA.DataObjects.AppData.ViewContractMode = true;
                DDA.DataObjects.AppData.ContractMode = "Edit";
                DDA.DataObjects.AppData.CurrentContract.ContractNumber = cboContractNumber.Text;
                frmContractInformation frmCI = new frmContractInformation();
                frmCI.Show();
                this.Close();
                // write a routine here to load the previous data
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // this will delete a contract
            // Contract, ContractDistributor, ContractCategory, ContractCounty
            if (MessageBox.Show("Are you sure you want to delete this contract?  This action is irreversible.", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int ContractID;
                ContractID = DDA.DataAccess.Contract_da.GetContractID(cboContractNumber.Text);

                DDA.DataAccess.Contract_da.DeleteContract(ContractID);

                MessageBox.Show("Delete successful");
            }
            else
            {
                MessageBox.Show("Delete cancelled");
            }


        }

        private void chkCategories_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void chkCategories_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if (LoadingForm == false)
            {

                DDA.DataObjects.AppData.DataChanged = true;
            }

            LoadingForm = true;
        }

        void chkCategories_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            LoadingForm = false;
        }

        private void cboState_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}