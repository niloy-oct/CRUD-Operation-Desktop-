using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Operations
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        void Cancel() 
        {
            txtFirstName.Text = txtLastName.Text = txtCity.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cancel();
            PopulateDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();

            using(DBEntities db = new DBEntities())
            {
                if (model.CustomerID == 0)//Insert
                    db.Customers.Add(model);
                else//Update
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            Cancel();
            PopulateDataGridView();
            MessageBox.Show("Successfully Submitted");
            
        }

        void PopulateDataGridView()
        {
           
            using (DBEntities db = new DBEntities())
            {
                dgbCustomer.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void dgbCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (dgbCustomer.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgbCustomer.CurrentRow.Cells["CustomerID"].Value);

                using (DBEntities db = new DBEntities())
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    txtFirstName.Text = model.FirstName;
                    txtLastName.Text = model.LastName ;
                    txtCity.Text = model.City;
                    txtAddress.Text = model.Address;


                }

                btnSave.Text = "Update";
                btnDelete.Enabled = true;
               
   
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you Sure To Delete Data ?", "CRUD OPERATION", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.Customers.Attach(model);
                    db.Customers.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Cancel();
                    MessageBox.Show("Deleted Succesfully");
                }
            }
        }
        
    }
}
