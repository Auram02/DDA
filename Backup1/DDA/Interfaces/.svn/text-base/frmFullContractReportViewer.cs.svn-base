using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Collections;
using System.Xml;
using System.IO;
using DDA.DataObjects;


using System.Diagnostics;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;

using System.Text.RegularExpressions;

// split/merge multiple pdf documents
using Gios.Pdf.SplitMerge;



using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace DDA.Interfaces
{
    public partial class frmFullContractReportViewer : Form
    {

        public static DDA.Report_Datasources.rptFCR tmpData = new DDA.Report_Datasources.rptFCR();

        public static string contractNumber;
        public static int distID;
        public static bool instantPrint;
        public static string printerName;

        public static ReportDocument rptDoc;
        public static PdfSplitterMerger psm;
        public static PdfDocument outputDocument = new PdfDocument();


        public frmFullContractReportViewer()
        {
            InitializeComponent();
        }

        public frmFullContractReportViewer(string p_contractNumber, int p_distID, bool p_instantPrint)
        {
            InitializeComponent();

            contractNumber = p_contractNumber;
            distID = p_distID;
            instantPrint = p_instantPrint;
        }

        public static void PrintOnly(string p_contractNumber, int p_distID, bool p_instantPrint, string p_printerName)
        {
            contractNumber = p_contractNumber;
            distID = p_distID;
            instantPrint = p_instantPrint;

            //printerName = p_printerName;

            SetupReport();
        }

        public static void SetupReport()
        {
            GetData("");
            
            try
            {


                //rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
               // rptDoc.Refresh();
                rptDoc.SetDataSource(tmpData);

            }
            catch (Exception ex)
            {
                string rptPath;
                rptPath = Application.StartupPath + "\\Reports\\rptFullContract.rpt";
                DDA.Interfaces.frmFullContractReportViewer.rptDoc = new ReportDocument();

                DDA.Interfaces.frmFullContractReportViewer.rptDoc.Load(rptPath);
                rptDoc.SetDataSource(tmpData);
                
                try
                {

                    outputDocument.Save("errorOutput.pdf");
                }
                catch
                {
                }

                
                //MessageBox.Show("An Error Has Occurred: " + ex.Message);
                //rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);

            }

            Sections crSections;

            crSections = rptDoc.ReportDefinition.Sections;
            SubreportObject crSubreportObject;
            ReportDocument crSubreportDoc = new ReportDocument();

            ReportObjects crObjects;

            foreach (Section crSection in crSections)
            {
                crObjects = crSection.ReportObjects;

                foreach (ReportObject crObject in crObjects)
                {
                    if (crObject.Kind == ReportObjectKind.SubreportObject)
                    {

                        crSubreportObject = (SubreportObject)crObject;
                        crSubreportDoc = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);

                        break;
                    }
                }

            }

            

            crSubreportDoc.SetDataSource(tmpData);

            if (instantPrint == true)
            {
                //this.crystalReportViewer1.PrintReport();
                //rptDoc.PrintOptions.PrinterName = printerName;
                
                // ............................................................

                // Open the output document
                

                // Iterate files
                //for (int z = 0; z < 2;z++ )
                //{
                    //string file;

                    //file = "tempOutput.pdf";
                    // Open the document to import pages from it.


                    PdfDocument inputDocument = PdfReader.Open(rptDoc.ExportToStream(ExportFormatType.PortableDocFormat), PdfDocumentOpenMode.Import);

                    // Iterate pages
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        // Get the page from the external document...
                        PdfPage page = inputDocument.Pages[idx];
                        // ...and add it to the output document.
                        outputDocument.AddPage(page);
                    }

                
                //}
                // ............................................................

                tmpData.Clear();

            }



            
        }



        private void frmFullContractReportViewer_Load(object sender, EventArgs e)
        {

            //set the local report path (note: the properties of the project post-build event
            // include this command in order to use this embedded report from the bin directory:
            // copy "$(ProjectDir)\Report1.rdlc" "$(TargetDir)"

            //set the local report path (note: the properties of the project post-build event
            // include this command in order to use this embedded report from the bin directory:
            // copy "$(ProjectDir)\Report1.rdlc" "$(TargetDir)"

            //this.reportViewer1.Clear();

            //this.reportViewer1.LocalReport.ReportPath = "rptContractInfoQuickview.rdlc";
            // reportViewer1.LocalReport.DataSources.Add(GetData("rptCIV_SingleItemData_dtSingleItems"));
            //reportViewer1.Refresh();
            //reportViewer1.LocalReport.DataSources.Clear();
            //reportViewer1.LocalReport.Refresh();

            //GetData("");


            //this.reportViewer1.RefreshReport();
             rptDoc = new ReportDocument();

            SetupReport();

            crystalReportViewer1.ReportSource = rptDoc;
            

        }

        private static void GetData(string reportSource)
        {
            ReportDataSource rds;

            DataTable dt;
            dt = new DataTable();

            dt.Columns.Add("Name");
            dt.Columns.Add("Title");

            string names;
            names = "";

            DataRow dr = tmpData.MainDistributor.NewRow();

            DataSet ds;
            
            //distID = 34;
            //contractNumber = "AIR982006-586";


            //int contractID;
            //contractID = DDA.DataAccess.Contract_da.GetContractID(contractNumber);
            //DDA.DataAccess.Contract_da.GetDistributorContract()



            ////    sql = "SELECT SAP, Node, DistName, BillingAddress, BillingCityName, fk_BillingStateID, fk_BillingZipID, fk_BillingCountryID, ShippingAddress, Distributor.Phone, " +
            ////"Distributor.Fax, Distributor.CityName, State.FullName, Country.CountryName, " +
            ////"Contacts, Distributor.fk_ZipID, Distributor.PartsOnly " +
            ////"FROM Distributor, City, State, Country " +
            ////"WHERE State.StateID = Distributor.fk_StateID " +
            ////"AND Country.CountryID = Distributor.fk_CountryID " +
            ////"AND pk_DistributorID = " + p_distrituborID;

            ds = DDA.DataAccess.Distributor_da.GetDistributorInformation(distID);

            DataRow drTemp = ds.Tables[0].Rows[0];

            names = "";
            string titles = "";

            
            DDA.DataObjects.Distributor.GetContactNames_Titles(distID, ref names, ref titles);

            dr["ContractNumber"] = contractNumber;
            dr["ContactNames"] = names;
            dr["ContactTitles"] = titles;

            // get the first branch, then use its id in the territory/regional manager lookup.
            DataSet dsTemp = DDA.DataAccess.Distributor_da.GetDistributorBranchList(distID,"");
            int tempBranchID = -1;
            try
            {
                tempBranchID = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["pk_DistributorID"]);
            }
            catch
            {
                tempBranchID = -1;
            }

            dr["TerritoryManager"] = DDA.DataAccess.Representative_da.GetRepresentativeName(DDA.DataAccess.Representative_da.GetActiveRepForDistributor(tempBranchID, "territory"));
            dr["RegionalManager"] = DDA.DataAccess.Representative_da.GetRepresentativeName(DDA.DataAccess.Representative_da.GetActiveRepForDistributor(tempBranchID, "service"));

            if (dr["TerritoryManager"].ToString() == "")
                dr["TerritoryManager"] = "(none)";

            if (dr["RegionalManager"].ToString() == "")
                dr["RegionalManager"] = "(none)";

            
            dr["Node"] = "Node " + drTemp["Node"];
            dr["SAP"] = "SAP " + drTemp["SAP"];
            dr["DistributorName"] = drTemp["DistName"];
            dr["BillingAddress"] = drTemp["BillingAddress"] + "\n" + drTemp["BillingCityName"] + ", " + DDA.DataAccess.Location_da.GetStateAbbreviation(Convert.ToInt32(drTemp["fk_BillingStateID"])) + " " + drTemp["fk_BillingZipID"];
            string stateFullName = drTemp["fk_BillingStateID"].ToString();
            dr["ShippingAddress"] = drTemp["ShippingAddress"] + "\n" + drTemp["CityName"] + ", " + DDA.DataAccess.Location_da.GetStateAbbreviation(Convert.ToInt32(drTemp["fk_BillingStateID"])) + " " + drTemp["fk_ZipID"];
            dr["Telephone"] = drTemp["Phone"];
            dr["Telefax"] = drTemp["Fax"];


            // Contract stuff
            DDA.DataAccess.Contract_da.GetContract(contractNumber);

            dr["ContractDate"] = DDA.DataObjects.AppData.CurrentContract.ContractDate;
            dr["ModifiedDate"] = DDA.DataObjects.AppData.CurrentContract.ModifyDate;

            ArrayList counties = DDA.DataObjects.AppData.CurrentContract.Counties;

            DataSet dsCats = DDA.DataAccess.Contract_da.GetContractCategories(contractNumber);
            
            string countyNames;
            countyNames = "";

            string state, county;
            for (int i = 0; i < counties.Count; i++)
            {
                county = DDA.DataAccess.Location_da.GetCountyName(Convert.ToInt32(counties[i].ToString()));
                state = DDA.DataAccess.Location_da.GetCountyStateAbbreviation(Convert.ToInt32(counties[i].ToString()));

                if (countyNames != "")
                    countyNames = countyNames + " - ";
    
                countyNames = countyNames + county + ", " + state;

            }

            dr["Territory"] = countyNames;

            string cats;
            cats = "";
            for (int i = 0; i < dsCats.Tables[0].Rows.Count; i++)
            {
                if (cats != "")
                    cats = cats + ", ";

                cats = cats + dsCats.Tables[0].Rows[i]["CategoryName"];

            }
            dr["Categories"] = cats;

            tmpData.MainDistributor.Rows.Add(dr);

            PopulateSalesLocationData();



            //dt.Rows.Add(names);
            // ds = objArray.ToDataSet();

            //rds = new ReportDataSource("rptFCR_ContactName_Contacts", dt);
            //reportViewer1.LocalReport.DataSources.Add(rds);


            //dt = new DataTable();
            //rds = new ReportDataSource("rptCIV_SingleItemData_dtCounty", dt);
            //reportViewer1.LocalReport.DataSources.Add(rds);

            //rds = new ReportDataSource("rptCIV_SingleItemData_dtState", dt);
            //reportViewer1.LocalReport.DataSources.Add(rds);
            //rds = new ReportDataSource("rptCIV_SingleItemData_dtBranches", dt);
            //reportViewer1.LocalReport.DataSources.Add(rds);
            //rds = new ReportDataSource("rptCIV_SingleItemData_dtCategory", dt);
            //reportViewer1.LocalReport.DataSources.Add(rds);
            //rds = new ReportDataSource("rptCIV_SingleItemData_dtSingleData", dt);
            //reportViewer1.LocalReport.DataSources.Add(rds);
        }

        private static void PopulateSalesLocationData()
        {
            // distID, contractNumber

            ArrayList branches = DDA.DataObjects.AppData.CurrentContract.Branches;
            
            // branches = branchID
            DataSet ds;
            
            int tempDistID;

            string names, titles;

            for (int i = 0; i < branches.Count; i++)
            {
                names = "";
                titles = "";

                tempDistID = Convert.ToInt32(branches[i].ToString());

                ds = DDA.DataAccess.Distributor_da.GetDistributorInformation(tempDistID);

                DataRow drTemp = ds.Tables[0].Rows[0];
                DataRow dr = tmpData.Branches.NewRow();

                DDA.DataObjects.Distributor.GetContactNames_Titles(tempDistID, ref names, ref titles);

                if (names == "")
                    names = "(none)";

                if (titles == "")
                    titles = "(none)";

                dr["ContactNames"] = names;
                dr["ContactTitles"] = titles;


                dr["SAP"] = "SAP " + drTemp["SAP"];
                dr["BillingAddress"] = drTemp["BillingAddress"] + "\n" + drTemp["BillingCityName"] + ", " + DDA.DataAccess.Location_da.GetStateAbbreviation(Convert.ToInt32(drTemp["fk_BillingStateID"])) + " " + drTemp["fk_BillingZipID"];
                string stateFullName = drTemp["fk_BillingStateID"].ToString();
                dr["ShippingAddress"] = drTemp["ShippingAddress"] + "\n" + drTemp["CityName"] + ", " + DDA.DataAccess.Location_da.GetStateAbbreviation(Convert.ToInt32(drTemp["fk_BillingStateID"])) + " " + drTemp["fk_ZipID"];
                dr["Telephone"] = drTemp["Phone"];
                dr["Telefax"] = drTemp["Fax"];

                tmpData.Branches.Rows.Add(dr);

            }

        }



        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}