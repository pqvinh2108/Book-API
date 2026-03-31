using System.Net.Http.Headers;
using System.Text.Json;

namespace BookClient;

public partial class Form1 : Form
{
    private DataGridView dgvBooks;
    private TextBox txtSearch, txtTitle, txtAuthor, txtPrice;
    private ComboBox cboCategory;
    private PictureBox picBook;
    private Button btnSearch, btnAdd, btnUpdate, btnDelete, btnChooseImage;
    private string selectedImagePath = "";
    private int selectedBookId = 0;

    private readonly HttpClient _client;

    public Form1()
    {
        InitializeCustomComponent();
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:9999/api/") };
        LoadCategories();
        LoadBooks();
    }

    private void InitializeCustomComponent()
    {
        this.Size = new Size(1000, 600);
        this.Text = "Quản lý Sách (Book API Client)";

        // Panel trái (Danh sách và tìm kiếm)
        var pnlLeft = new Panel { Dock = DockStyle.Left, Width = 600, Padding = new Padding(10) };
        
        var pnlTopLeft = new Panel { Dock = DockStyle.Top, Height = 40 };
        txtSearch = new TextBox { Width = 300, Location = new Point(10, 10) };
        btnSearch = new Button { Text = "Tìm kiếm", Location = new Point(320, 8), Width = 80 };
        btnSearch.Click += BtnSearch_Click;
        pnlTopLeft.Controls.Add(txtSearch);
        pnlTopLeft.Controls.Add(btnSearch);

        dgvBooks = new DataGridView { Dock = DockStyle.Fill, AutoGenerateColumns = false, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, ReadOnly = true };
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "bookID", HeaderText = "ID", Width = 50 });
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "title", HeaderText = "Tiêu đề", Width = 150 });
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "author", HeaderText = "Tác giả", Width = 120 });
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "price", HeaderText = "Giá", Width = 80 });
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "categoryName", HeaderText = "Thể loại", Width = 100 });
        dgvBooks.CellClick += DgvBooks_CellClick;

        pnlLeft.Controls.Add(dgvBooks);
        pnlLeft.Controls.Add(pnlTopLeft);

        // Panel phải (Thêm/Sửa/Xóa)
        var pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

        int startY = 20;
        pnlRight.Controls.Add(new Label { Text = "Tiêu đề:", Location = new Point(20, startY), AutoSize = true });
        txtTitle = new TextBox { Location = new Point(100, startY), Width = 200 };
        pnlRight.Controls.Add(txtTitle);

        startY += 40;
        pnlRight.Controls.Add(new Label { Text = "Tác giả:", Location = new Point(20, startY), AutoSize = true });
        txtAuthor = new TextBox { Location = new Point(100, startY), Width = 200 };
        pnlRight.Controls.Add(txtAuthor);

        startY += 40;
        pnlRight.Controls.Add(new Label { Text = "Giá:", Location = new Point(20, startY), AutoSize = true });
        txtPrice = new TextBox { Location = new Point(100, startY), Width = 200 };
        pnlRight.Controls.Add(txtPrice);

        startY += 40;
        pnlRight.Controls.Add(new Label { Text = "Thể loại:", Location = new Point(20, startY), AutoSize = true });
        cboCategory = new ComboBox { Location = new Point(100, startY), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        pnlRight.Controls.Add(cboCategory);

        startY += 40;
        pnlRight.Controls.Add(new Label { Text = "Hình ảnh:", Location = new Point(20, startY), AutoSize = true });
        btnChooseImage = new Button { Text = "Chọn ảnh", Location = new Point(100, startY), Width = 80 };
        btnChooseImage.Click += BtnChooseImage_Click;
        pnlRight.Controls.Add(btnChooseImage);

        startY += 35;
        picBook = new PictureBox { Location = new Point(100, startY), Width = 200, Height = 150, SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };
        pnlRight.Controls.Add(picBook);

        startY += 170;
        btnAdd = new Button { Text = "Thêm mới", Location = new Point(30, startY), Width = 80 };
        btnAdd.Click += BtnAdd_Click;
        btnUpdate = new Button { Text = "Cập nhật", Location = new Point(130, startY), Width = 80 };
        btnUpdate.Click += BtnUpdate_Click;
        btnDelete = new Button { Text = "Xóa", Location = new Point(230, startY), Width = 80 };
        btnDelete.Click += BtnDelete_Click;

        pnlRight.Controls.Add(btnAdd);
        pnlRight.Controls.Add(btnUpdate);
        pnlRight.Controls.Add(btnDelete);

        this.Controls.Add(pnlRight);
        this.Controls.Add(pnlLeft);
    }

    private async void LoadCategories()
    {
        try
        {
            var response = await _client.GetAsync("Categories");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cats = JsonSerializer.Deserialize<List<CategoryDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                cboCategory.DataSource = cats;
                cboCategory.DisplayMember = "CategoryName";
                cboCategory.ValueMember = "CategoryID";
            }
        }
        catch (Exception ex) { MessageBox.Show("Lỗi tải danh mục: " + ex.Message); }
    }

    private async void LoadBooks(string search = "")
    {
        try
        {
            var response = await _client.GetAsync($"Books?search={search}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var books = JsonSerializer.Deserialize<List<BookVM>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                dgvBooks.DataSource = books;
            }
        }
        catch (Exception ex) { MessageBox.Show("Lỗi tải sách: " + ex.Message); }
    }

    private async void LoadImage(string targetFileName)
    {
        if (string.IsNullOrEmpty(targetFileName))
        {
            picBook.Image = null;
            return;
        }

        try
        {
            // Tải ảnh từ thư mục API Content/ImageBooks
            var response = await _client.GetAsync($"http://localhost:9999/Content/ImageBooks/{targetFileName}");
            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                picBook.Image = Image.FromStream(stream);
            }
            else
            {
                picBook.Image = null;
            }
        }
        catch
        {
            picBook.Image = null;
        }
    }

    private void BtnSearch_Click(object? sender, EventArgs e) => LoadBooks(txtSearch.Text);

    private void BtnChooseImage_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp" };
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            selectedImagePath = ofd.FileName;
            picBook.Image = Image.FromFile(selectedImagePath);
        }
    }

    private void DgvBooks_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            var book = (BookVM)dgvBooks.Rows[e.RowIndex].DataBoundItem;
            selectedBookId = book.bookID;
            txtTitle.Text = book.title;
            txtAuthor.Text = book.author;
            txtPrice.Text = book.price.ToString();
            cboCategory.SelectedValue = book.categoryID;
            selectedImagePath = "";
            LoadImage(book.imageFileName);
        }
    }

    private async void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (!decimal.TryParse(txtPrice.Text, out decimal price))
        {
            MessageBox.Show("Vui lòng nhập Giá (Price) là một con số hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var formContent = new MultipartFormDataContent();
        formContent.Add(new StringContent(txtTitle.Text ?? ""), "Title");
        formContent.Add(new StringContent(txtAuthor.Text ?? ""), "Author");
        formContent.Add(new StringContent(price.ToString(System.Globalization.CultureInfo.InvariantCulture)), "Price");
        
        if (cboCategory.SelectedValue != null)
            formContent.Add(new StringContent(cboCategory.SelectedValue.ToString()), "CategoryID");

        if (!string.IsNullOrEmpty(selectedImagePath) && File.Exists(selectedImagePath))
        {
            var fileBytes = File.ReadAllBytes(selectedImagePath);
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            formContent.Add(fileContent, "ImageFile", Path.GetFileName(selectedImagePath));
        }

        var response = await _client.PostAsync("Books", formContent);
        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Thêm thành công!");
            LoadBooks();
        }
        else 
        {
            var err = await response.Content.ReadAsStringAsync();
            MessageBox.Show($"Lỗi khi thêm: {response.ReasonPhrase}\nChi tiết: {err}");
        }
    }

    private async void BtnUpdate_Click(object? sender, EventArgs e)
    {
        if (selectedBookId == 0) return;

        if (!decimal.TryParse(txtPrice.Text, out decimal price))
        {
            MessageBox.Show("Vui lòng nhập Giá (Price) là một con số hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var formContent = new MultipartFormDataContent();
        formContent.Add(new StringContent(txtTitle.Text ?? ""), "Title");
        formContent.Add(new StringContent(txtAuthor.Text ?? ""), "Author");
        formContent.Add(new StringContent(price.ToString(System.Globalization.CultureInfo.InvariantCulture)), "Price");
        
        if (cboCategory.SelectedValue != null)
            formContent.Add(new StringContent(cboCategory.SelectedValue.ToString()), "CategoryID");

        if (!string.IsNullOrEmpty(selectedImagePath) && File.Exists(selectedImagePath))
        {
            var fileBytes = File.ReadAllBytes(selectedImagePath);
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            formContent.Add(fileContent, "ImageFile", Path.GetFileName(selectedImagePath));
        }

        var response = await _client.PutAsync($"Books/{selectedBookId}", formContent);
        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Cập nhật thành công!");
            LoadBooks();
        }
        else 
        {
            var err = await response.Content.ReadAsStringAsync();
            MessageBox.Show($"Lỗi khi cập nhật: {response.ReasonPhrase}\nChi tiết: {err}");
        }
    }

    private async void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (selectedBookId == 0) return;

        if (MessageBox.Show("Xóa sách này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            var response = await _client.DeleteAsync($"Books/{selectedBookId}");
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Xóa thành công!");
                LoadBooks();
                selectedBookId = 0;
            }
            else MessageBox.Show("Lỗi khi xóa.");
        }
    }
}

public class CategoryDTO
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
}

public class BookVM
{
    public int bookID { get; set; }
    public string title { get; set; }
    public string author { get; set; }
    public decimal price { get; set; }
    public string imageFileName { get; set; }
    public int categoryID { get; set; }
    public string categoryName { get; set; }
}
