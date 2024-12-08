using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Local_Canteen_Optimizer.Commands;
using Local_Canteen_Optimizer.DAO.ProductDAO;
using Local_Canteen_Optimizer.Helper;
using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.Service;
using Local_Canteen_Optimizer.View;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.System;

namespace Local_Canteen_Optimizer.ViewModel
{
    class ProductViewModel : BaseViewModel
    {
        private IProductDao _dao = null;
        public string Keyword { get; set; } = "";
        public bool NameAcending { get; set; } = true;
        public int CurrentPage { get; set; } = 0;
        public int RowsPerPage { get; set; } = 10;
        public int TotalPages { get; set; } = 0;
        public int TotalItems { get; set; } = 0;
        public ObservableCollection<FoodModel> FoodItems { get; set; }
        public ObservableCollection<FoodModel> allFoodItems { get; set; }
        public ICommand DeleteFoodCommand { get; set; }

        public async Task Init()
        {
            try {
                _dao = new ProductDAOImp();
                FoodItems = new ObservableCollection<FoodModel>();
                allFoodItems = new ObservableCollection<FoodModel>();
                DeleteFoodCommand = new RelayCommand<FoodModel>(async (food) => await ConfirmAndAddFoodItem(food));
                await LoadProductsAsync(); 
            } catch{
                await MessageHelper.ShowErrorMessage("Can't get any products", App.m_window.Content.XamlRoot);
            }
        }
        public async Task Load(int page)
        {
            CurrentPage = page;
            await LoadProductsAsync();
        }

        public async Task LoadProductSort(bool nameAcending)
        {
            NameAcending = nameAcending;
            await LoadProductsAsync();
        }

        public async Task LoadProductsAsync()
        {
            var (totalItems,products) = await _dao.GetProductsAsync(CurrentPage, RowsPerPage, Keyword, NameAcending, null, null);
            FoodItems.Clear();
            foreach (var item in products)
            {
                FoodItems.Add(item);
            }
            OnPropertyChanged(nameof(FoodItems));

            TotalItems = totalItems;
            TotalPages = (TotalItems / RowsPerPage) + ((TotalItems % RowsPerPage == 0) ? 0 : 1);

        }

        public async Task LoadAllProductsAsync()
        {
            var (totalItems, products) = await _dao.GetProductsAsync(null, null, null, true, null, null);
            allFoodItems.Clear();
            foreach (var item in products)
            {
                allFoodItems.Add(item);
            }
        }

        //public void UpdatePageOptions()
        //{
        //    PageOptions.Clear();
        //    for (int i = 1; i <= TotalPages; i++)
        //    {
        //        PageOptions.Add($"{i}/{TotalPages}");
        //    }
        //}

        public async Task AddFoodItem(FoodModel food)
        {
            FoodModel newFood = await _dao.AddProductAsync(food);
            if (newFood != null)
            {
                FoodItems.Add(newFood);
            } else
            {
                throw new Exception("Fail to add new product");
            }
        }
        public async Task UpdateProduct(FoodModel product)
        {
            FoodModel updateFood = await _dao.UpdateProductAsync(product);
            if(updateFood != null)
            {        
                // Tìm và cập nhật sản phẩm trong danh sách
                var existingProductIndex = FoodItems.IndexOf(FoodItems.FirstOrDefault(p => p.Name == product.Name));
                if (existingProductIndex >= 0)
                {
                    FoodItems[existingProductIndex] = new FoodModel
                    {
                        ProductID = product.ProductID,
                        Name = product.Name,
                        ImageSource = product.ImageSource,
                        Price = product.Price,
                        Quantity = product.Quantity
                    };
                    await MessageHelper.ShowSuccessMessage("Update product successful", App.m_window.Content.XamlRoot);
                }
                else
                {
                    await MessageHelper.ShowErrorMessage("Fail to update product", App.m_window.Content.XamlRoot);
                }
            } else
            {
                await MessageHelper.ShowErrorMessage("Fail to update product", App.m_window.Content.XamlRoot);
            }
        }
        private async Task ConfirmAndAddFoodItem(FoodModel food)
        {
            if (food == null)
            {
                // Hiển thị thông báo lỗi nếu cần
                return;
            }

            // Hiển thị hộp thoại xác nhận
            bool isConfirmed = await MessageHelper.ShowConfirmationDialog(
                $"Do you want to delete: {food.Name}?",
                "Confirm delete product",
                App.m_window.Content.XamlRoot
            );

            if (isConfirmed)
            {
                await DeleteProduct(food);
            }
        }
        public async Task DeleteProduct(FoodModel product)
        {
            bool isRemoved = await _dao.RemoveProductAsync(int.Parse(product.ProductID));
            if (isRemoved)
            {
                if (FoodItems.Contains(product))
                {
                    FoodItems.Remove(product);
                    await MessageHelper.ShowSuccessMessage("Remove product successful", App.m_window.Content.XamlRoot);
                }
            } else
            {
                await MessageHelper.ShowErrorMessage("Fail to remove product", App.m_window.Content.XamlRoot);
            }
        }

        public async Task<string> PickExcelFileAsync()
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".xlsx");
            picker.FileTypeFilter.Add(".xls");

            // Kết nối với cửa sổ ứng dụng
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSingleFileAsync();
            return file?.Path;
        }

        public async Task ImportProductsFromExcel(string filePath)
        {
            try
            {
                string absolutePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(absolutePath, false))
                {
                    WorkbookPart workbookPart = document.WorkbookPart;
                    Sheet sheet = workbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                    WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    List<FoodModel> products = new List<FoodModel>();

                    foreach (Row row in sheetData.Elements<Row>().Skip(1)) // Skip header row
                    {
                        FoodModel product = new FoodModel
                        {
                            Name = GetCellValue(document, row.Elements<Cell>().ElementAt(0)),
                            ImageSource = GetCellValue(document, row.Elements<Cell>().ElementAt(1)),
                            Price = double.Parse(GetCellValue(document, row.Elements<Cell>().ElementAt(2))),
                            Quantity = int.Parse(GetCellValue(document, row.Elements<Cell>().ElementAt(3)))
                        };
                        products.Add(product);
                    }

                    foreach (var product in products)
                    {
                        await AddFoodItem(product);
                    }
                    await MessageHelper.ShowSuccessMessage("Import product successful", App.m_window.Content.XamlRoot);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await MessageHelper.ShowErrorMessage("Failed to import products", App.m_window.Content.XamlRoot);
            }
        }

        private string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue == null)
            {
                return string.Empty;
            }

            string value = cell.CellValue.InnerXml;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[int.Parse(value)].InnerText;
            }
            return value;
        }

        public async Task<string> PickSaveFileAsync()
        {
            var picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.SuggestedFileName = "Products";
            picker.FileTypeChoices.Add("Excel Files", new List<string>() { ".xlsx" });

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSaveFileAsync();
            return file?.Path;
        }

        public async Task ExportToExcel(string filePath, ObservableCollection<FoodModel> data)
        {
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
                {
                    // Tạo workbook
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    // Tạo worksheet
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    // Tạo danh sách các sheet
                    Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());

                    // Thêm sheet vào workbook
                    Sheet sheet = new Sheet()
                    {
                        Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Products"
                    };
                    sheets.Append(sheet);

                    // Lấy dữ liệu sheet
                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    // Thêm header
                    Row headerRow = new Row();
                    headerRow.Append(
                        CreateTextCell("A1", "Name"),
                        CreateTextCell("B1", "Image Source"),
                        CreateTextCell("C1", "Price"),
                        CreateTextCell("D1", "Quantity")
                    );
                    sheetData.AppendChild(headerRow);

                    // Thêm dữ liệu
                    int rowIndex = 2;
                    foreach (var item in data)
                    {
                        Row dataRow = new Row();
                        dataRow.Append(
                            CreateTextCell($"A{rowIndex}", item.Name),
                            CreateTextCell($"B{rowIndex}", item.ImageSource),
                            CreateNumberCell($"C{rowIndex}", item.Price.ToString()),
                            CreateNumberCell($"D{rowIndex}", item.Quantity.ToString())
                        );
                        sheetData.AppendChild(dataRow);
                        rowIndex++;
                    }

                    // Lưu workbook
                    workbookPart.Workbook.Save();
                }
                await MessageHelper.ShowSuccessMessage("Data exported to Excel successfully!", App.m_window.Content.XamlRoot);
            }
            catch (Exception ex)
            {
                await MessageHelper.ShowErrorMessage($"Failed to export data: {ex.Message}", App.m_window.Content.XamlRoot);
            }
        }

        private Cell CreateTextCell(string cellReference, string cellValue)
        {
            return new Cell()
            {
                CellReference = cellReference,
                DataType = CellValues.String,
                CellValue = new CellValue(cellValue)
            };
        }

        private Cell CreateNumberCell(string cellReference, string cellValue)
        {
            return new Cell()
            {
                CellReference = cellReference,
                DataType = CellValues.Number,
                CellValue = new CellValue(cellValue)
            };
        }


        //public ProductViewModel()
        //{
        //    // Giả lập danh sách món ăn
        //    FoodItems = new ObservableCollection<FoodModel>
        //    {
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},

        //    };
        //}
    }
}
