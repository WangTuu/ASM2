using LibraryManagement_Group2_Project.DAL;
using LibraryManagement_Group2_Project.DTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LibraryManagement_Group2_Project.GUI
{
    public partial class Statistic : Form
    {
        public static int check = 0;
        public static int check1 = 0;

        public Statistic()
        {
            InitializeComponent();
            check = 0;
            check1 = 0;
            cbo1.DataSource = MemberDAO.GetCbos();
            cbo2.DataSource = MemberDAO.GetCbos();
        }




        private void btnFilter_Click(object sender, EventArgs e)
        {
            var selectedValue = cbo1.SelectedValue;

            if (selectedValue != null)
            {
                int memberNumber;
                if (int.TryParse(selectedValue.ToString(), out memberNumber))
                {
                    DataTable dt = MemberDAO.GetTotalBooksBorrowed(memberNumber);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int totalBorrowed = (int)dt.Rows[0]["TotalBorrowed"];
                        txtBorrow.Text = $"{totalBorrowed}";
                    }
                    else
                    {
                        txtBorrow.Text = "No data found for the selected member.";
                    }
                }
                else
                {
                    MessageBox.Show("Invalid selection.");
                }
            }
        }



        private void btnFind2_Click(object sender, EventArgs e)
        {
            var selectedValue = cbo2.SelectedValue;

            if (selectedValue != null)
            {
                int memberNumber;
                if (int.TryParse(selectedValue.ToString(), out memberNumber))
                {
                    DateTime startDate = dtpStart.Value.Date;
                    DateTime endDate = dtpEnd.Value.Date;

                    if (endDate >= startDate)
                    {
                        int lateReturnsCount = MemberDAO.GetLateReturnsCount(memberNumber, startDate, endDate);

                        txtLate.Text = $"{lateReturnsCount}";
                    }
                    else
                    {
                        MessageBox.Show("End date must be after or equal to start date.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid selection.");
                }
            }
        }


        private void btnFind3_Click(object sender, EventArgs e)
        {
            DataTable dt = CirculatedCopyDAO.GetMonthlyBorrowingTrends();

            if (dt != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (DataRow row in dt.Rows)
                {
                    int month = (int)row["Month"];
                    int totalBorrowed = (int)row["TotalBorrowed"];

                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

                    sb.AppendLine($"{monthName}\t{totalBorrowed}");
                }

                txtRes.Text = sb.ToString();
            }
            else
            {
                MessageBox.Show("Error retrieving data.");
            }
        }

    }
}
