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
    public partial class frmCounties : Form
    {
        private int currentStateID;
        private DataSet dsChosenCounties;
        private bool isLoading;
        private bool LoadingForm;


        public frmCounties()
        {
            InitializeComponent();
            DDA.DataObjects.AppData.DisableCloseButton(Handle);
            DDA.DataObjects.AppData.PreviousForm = "frmCounties";
            LoadingForm = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int countyID;
            int stateID;
            
            stateID = DDA.DataAccess.Location_da.GetStateID(cboState.Text);


            DDA.DataAccess.Contract_da.DeleteCounties_byState_ContractID(stateID, DDA.DataObjects.AppData.CurrentContract.ContractID);

            //DDA.DataObjects.AppData.CurrentContract.ClearCounties();

            int i;
            string tempStr;
            tempStr = "";
            for (i = 0; i < chklbCounties.Items.Count; i++)
            {

                // Debug only
                //if (chklbCounties.GetItemCheckState(i) == CheckState.Indeterminate)
                //    MessageBox.Show(chklbCounties.Items[i].ToString());

                if (chklbCounties.GetItemCheckState(i) == CheckState.Checked)
                {
                    // Add in state restriction
                    countyID = DDA.DataAccess.Location_da.GetCountyID(Convert.ToString(chklbCounties.Items[i].ToString()), currentStateID);

                    DDA.DataObjects.AppData.CurrentContract.AddCounty(countyID);
                    tempStr = tempStr + "," + Convert.ToString(countyID);

                }
            }

            // debug only
            //MessageBox.Show(tempStr);

            DDA.DataObjects.AppData.DistributorBranchListContractMode = true;
            frmDistributorBranchList frmDBL = new frmDistributorBranchList();
            frmDBL.Show();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            frmTerritory frmTerritory = new frmTerritory();
            frmTerritory.Show();
            this.Close();
        }

        private void frmCounties_Load(object sender, EventArgs e)
        {
            LoadingForm = true;
            isLoading = true;

            DataSet ds;
            ds = DDA.DataAccess.Location_da.GetStateList();

            int i;

            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                cboState.Items.Add(ds.Tables[0].Rows[i]["FullName"]);
            }

            cboState.Text = DDA.DataObjects.AppData.CurrentContract.StateName;

            int stateID;
            stateID = DDA.DataObjects.AppData.CurrentContract.StateID;

            ds = DDA.DataAccess.Location_da.GetCountyList(stateID);


            //for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    chklbCounties.Items.Add(ds.Tables[0].Rows[i]["CountyName"]);
                
            //}

            int j;

            ArrayList counties;
            counties = DDA.DataObjects.AppData.CurrentContract.Counties;
            string countyName;
            int CountyID;

            for (i = 0; i < counties.Count; i++)
            {
                CountyID = Convert.ToInt32(counties[i]);

                if (DDA.DataAccess.Location_da.CheckIfCountyIDIsInState(CountyID, currentStateID) == true)
                {
                    countyName = Convert.ToString(DDA.DataAccess.Location_da.GetCountyName(CountyID));

                    for (j = 0; j < chklbCounties.Items.Count; j++)
                    {
                        if (countyName == chklbCounties.Items[j].ToString())
                        {
                            chklbCounties.SetItemCheckState(j, CheckState.Checked);
                        }
                    }

                }
            }


            CheckOccupiedCounties(currentStateID);


        }

        private void CheckOccupiedCounties(int currentStateID)
        {
            string countyName;
            int i, j;


            dsChosenCounties = DDA.DataAccess.Contract_da.GetOccupiedCounties(currentStateID, DDA.DataObjects.AppData.CurrentContract.Categories, DDA.DataObjects.AppData.CurrentContract.ContractID, DDA.DataObjects.AppData.CurrentContract.DeleteCounties);

            for (i = 0; i < dsChosenCounties.Tables[0].Rows.Count; i++)
            {
                countyName = dsChosenCounties.Tables[0].Rows[i][0].ToString();

                for (j = 0; j < chklbCounties.Items.Count; j++)
                {
                    if (countyName == chklbCounties.Items[j].ToString())
                    {
                        chklbCounties.SetItemCheckState(j, CheckState.Indeterminate);
                    }
                }

            }
        }
        
        private void chklbCounties_Click(object sender, EventArgs e)
        {

            // Check to see which counties have been chosen for the category already
            // see if the item that was clicked is the same as one in the list
            // if so, prompt the user to reassociate
            //            chklbCounties.SetItemCheckState(j, CheckState.Checked);

            // put in the stupid logic to handle how to change the checkstate...ugh.
            try
            {

            string selectedCounty;
            string tempCounty;
            selectedCounty = chklbCounties.SelectedItem.ToString();

            int i;

            for (i = 0; i < dsChosenCounties.Tables[0].Rows.Count; i++)
            {
                tempCounty = dsChosenCounties.Tables[0].Rows[i][0].ToString();
                if (selectedCounty == tempCounty)
                {
                    string distName;
                    string contractNum;
                    int contractID;
                    contractID = Convert.ToInt32(dsChosenCounties.Tables[0].Rows[i]["fk_ContractID"].ToString());
                    
                    distName = DDA.DataAccess.Contract_da.GetContractDistributor2(contractID);
                    contractNum = DDA.DataAccess.Contract_da.GetContractNumber(contractID);

                    if (MessageBox.Show("This county is currently associated to contract number '" + contractNum + "' under the distributor '" + distName + "' within this category.  Are you sure you wish to continue?", "Confirm Re-association", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {

                        // remove the row we found
                        DDA.DataObjects.AppData.CurrentContract.AddDeleteCounty(selectedCounty, currentStateID);
                        dsChosenCounties.Tables[0].Rows.RemoveAt(i);
                        chklbCounties.SetItemCheckState(chklbCounties.SelectedIndex, CheckState.Checked);
                        return;

                    } else {
                        // they decided not to re-associate.  continue
                        chklbCounties.SetItemCheckState(chklbCounties.SelectedIndex, CheckState.Indeterminate);
                        return;
                    }

                }
            }


        }
        catch
        {

        }
        }


        private void chklbCounties_ItemCheck(object sender, EventArgs e)
        {
            if (LoadingForm == false)
            {
                DDA.DataObjects.AppData.DataChanged = true;  // Modified
            }

            LoadingForm = true;
        }

        private void cboState_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }

        private void StateChanged()
        {
            //DDA.DataObjects.AppData.CurrentContract.ClearCounties();

            int i;
            int countyID;
            int stateID;

            stateID = DDA.DataAccess.Location_da.GetStateID(cboState.Text);

            if (isLoading == false)
            {
                //DDA.DataAccess.Contract_da.DeleteCounties_byState_ContractID(stateID, DDA.DataObjects.AppData.CurrentContract.ContractID);

            }

            for (i = 0; i < chklbCounties.CheckedItems.Count; i++)
            {
                // Add in state restriction
                countyID = DDA.DataAccess.Location_da.GetCountyID(Convert.ToString(chklbCounties.CheckedItems[i]), currentStateID);
                if (countyID > -1)
                    DDA.DataObjects.AppData.CurrentContract.AddCounty(countyID);

            }

            //DDA.DataObjects.AppData.DistributorBranchListContractMode = true;
            //frmDistributorBranchList frmDBL = new frmDistributorBranchList();
            //frmDBL.Show();
            //this.Close();

            currentStateID = DDA.DataAccess.Location_da.GetStateID(cboState.Text);


            // Now load new counties into the list
            chklbCounties.Items.Clear();

            DataSet ds = new DataSet();

            ds = DDA.DataAccess.Location_da.GetCountyList(currentStateID);


            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chklbCounties.Items.Add(ds.Tables[0].Rows[i]["CountyName"]);
            }

            int j;

            ArrayList counties;
            counties = DDA.DataObjects.AppData.CurrentContract.Counties;
            string countyName;
            int CountyID;

            for (i = 0; i < counties.Count; i++)
            {
                CountyID = Convert.ToInt32(counties[i]);

                if (DDA.DataAccess.Location_da.CheckIfCountyIDIsInState(CountyID, currentStateID) == true)
                {
                    countyName = Convert.ToString(DDA.DataAccess.Location_da.GetCountyName(CountyID));

                    for (j = 0; j < chklbCounties.Items.Count; j++)
                    {
                        if (countyName == chklbCounties.Items[j].ToString())
                        {
                            chklbCounties.SetItemCheckState(j, CheckState.Checked);
                        }
                    }

                }
            }

            CheckOccupiedCounties(currentStateID);
            isLoading = false;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            int i, j;

            for (i = 0; i < chklbCounties.Items.Count; i++)
            {
                if (chklbCounties.GetItemCheckState(i) == CheckState.Indeterminate)
                {
                    if (MessageBox.Show("One or more of the counties have already been selected in another contract for one of the categories in use.  This action will re-associate those counties with this contract.  Are you sure you want to continue?", "Re-association Alert", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    } else 
                    {
                        i = chklbCounties.Items.Count + 2;
                    }
                }
            }

            string tempCounty;
            string selectedCounty;

            for (i = 0; i < chklbCounties.Items.Count; i++)
            {
                // special processing if indeterminate
                if (chklbCounties.GetItemCheckState(i) == CheckState.Indeterminate)
                {
                    selectedCounty = chklbCounties.Items[i].ToString();

                        for (j = 0; j < dsChosenCounties.Tables[0].Rows.Count; j++)
                        {
                            tempCounty = dsChosenCounties.Tables[0].Rows[j][0].ToString();

                            if (tempCounty == selectedCounty)  // found a match
                            {
                                DDA.DataObjects.AppData.CurrentContract.AddDeleteCounty(selectedCounty, currentStateID);
                                dsChosenCounties.Tables[0].Rows.RemoveAt(j);
                            }
                        }
                }
                // check each item as we loop through
                chklbCounties.SetItemCheckState(i, CheckState.Checked);
            }


            
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            int i;

            for (i = 0; i < chklbCounties.Items.Count; i++)
            {
                if (chklbCounties.GetItemCheckState(i) != CheckState.Indeterminate)
                {
                    chklbCounties.SetItemCheckState(i, CheckState.Unchecked);
                }
            }
        }

        private void chklbCounties_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            frmDistributorList frmDistList = new frmDistributorList();

            frmDistList.Show();
            this.Close();
        }



        void chklbCounties_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            LoadingForm = false;
        }

        private void cboState_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            StateChanged();
        }

        private void btnEditList_Click(object sender, EventArgs e)
        {
            frmManageCountyList fmcl = new frmManageCountyList();
            this.Hide();
            fmcl.ShowDialog();
            this.Show();

            StateChanged();
        }

    }
}