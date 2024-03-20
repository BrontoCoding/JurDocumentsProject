using JurDocsClient;
using LexExchangeApi.Clients;

namespace JurDocsWinForms
{
    public partial class MainForm : Form
    {
        private const string _noLoginStripStatus = "�������� ������������, � ������� �����...";
        private bool _isLogin = false;
        private UserResponse? _currentUser = null;


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = _noLoginStripStatus;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                MessageBox.Show("Click by " + e.RowIndex);
                //TODO - Button Clicked - Execute Code Here
            }
        }

        private async void Login_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LoginText.Text))
            {
                toolStripStatusLabel1.Text = _noLoginStripStatus;
                _isLogin = false;
                return;
            }

            var client = JurClientService.JurDocsClientFactory();

            var user = await client.UsersAsync(new UserRequest { UserName = LoginText.Text });

            if (user.StatusCode == 200 && user.Result != null)
            {

                toolStripStatusLabel1.Text = "OK";
                _isLogin = true;
                _currentUser = user.Result;
                //LoginText.Items.Clear();
            }
        }


        private void testDataGrid()
        {
            var z = new List<FileTableList>();

            for (int i = 0; i < 10; i++)
                z.Add(new FileTableList { Id = i, DocType = "d1" + i, BtnText = "btn" + i });

            this.dataGridView1.DataSource = z;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fileTableListBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!_isLogin)
                return;

            testDataGrid();

            //Process.Start("explorer.exe", @"C:\Work\TFS\JurDocumentsProject\Temp1");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("����� �����������", "���������");
                return;
            }

            var client = JurClientService.JurDocsClientFactory();
            var response = await client.GetListDocumentsAsync("�������");

            var fileTableLists = new List<FileTableList>();

            var k = 1;
            foreach (var item in response.Result)
            {
                fileTableLists.Add(new FileTableList { Id = k, DocType = "�������", BtnText = "�������", FileName = item });
                k++;
            }
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = fileTableLists;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("����� �����������", "���������");
                return;
            }

            var client = JurClientService.JurDocsClientFactory();
            var response = await client.GetListDocumentsAsync("��������");

            var fileTableLists = new List<FileTableList>();

            var k = 1;
            foreach (var item in response.Result)
            {
                fileTableLists.Add(new FileTableList { Id = k, DocType = "��������", BtnText = "�������", FileName = item });
                k++;
            }
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = fileTableLists;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            const string DocName = "�������";

            if (_currentUser == null)
            {
                MessageBox.Show("����� �����������", "���������");
                return;
            }

            var client = JurClientService.JurDocsClientFactory();
            var response = await client.GetListDocumentsAsync(DocName);

            var fileTableLists = new List<FileTableList>();

            var k = 1;
            foreach (var item in response.Result)
            {
                fileTableLists.Add(new FileTableList
                {
                    Id = k,
                    DocType = DocName,
                    BtnText = "�������",
                    FileName = item
                });
                k++;
            }
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = fileTableLists;
        }
    }
}
