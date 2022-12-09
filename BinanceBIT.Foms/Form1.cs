namespace BinanceBIT.Foms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            AsksView.Columns.Add("Price","Price");
            //AsksView.Columns.Add(new DataGridViewColumn());
            AsksView.Columns[0].Name = "Price";
            //BidsView.Columns[0].Width = 100;
            //AsksView.Columns.Add(new ColumnHeader());
            AsksView.Columns[1].Name = "Quantity";
            //BidsView.Columns[0].Width = 100;

            //AsksView.Items.Add(;
            ListViewItem lv = new("Price", 0);
            //AsksView.Items.Add(lv);
      }

        private void AsksView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}