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
    /// <summary>
    /// ViewModel class for managing products.
    /// </summary>
    class ProductViewModel : BaseViewModel
    {
        private IProductDao _dao = null;

        /// <summary>
        /// Gets or sets the keyword for searching products.
        /// </summary>
        public string Keyword { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether the product names are sorted in ascending order.
        /// </summary>
        public bool NameAcending { get; set; } = true;

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of rows per page.
        /// </summary>
        public int RowsPerPage { get; set; } = 10;

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; } = 0;

        /// <summary>
        /// Gets or sets the total number of items.
        /// </summary>
        public int TotalItems { get; set; } = 0;

        /// <summary>
        /// Gets or sets the collection of food items.
        /// </summary>
        public ObservableCollection<FoodModel> FoodItems { get; set; }

        /// <summary>
        /// Gets or sets the collection of all food items.
        /// </summary>
        public ObservableCollection<FoodModel> allFoodItems { get; set; }

        /// <summary>
        /// Gets or sets the command for deleting a food item.
        /// </summary>
        public ICommand DeleteFoodCommand { get; set; }

        /// <summary>
        /// Initializes the ViewModel.
        /// </summary>
        public async Task Init()
        {
            try
            {
                _dao = new ProductDAOImp();
                FoodItems = new ObservableCollection<FoodModel>();
                allFoodItems = new ObservableCollection<FoodModel>();
                DeleteFoodCommand = new RelayCommand<FoodModel>(async (food) => await ConfirmAndAddFoodItem(food));
                await LoadProductsAsync();
            }
            catch
            {
                await MessageHelper.ShowErrorMessage("Can't get any products", App.m_window.Content.XamlRoot);
            }
        }

        /// <summary>
        /// Loads the products for the specified page.
        /// </summary>
        /// <param name="page">The page number to load.</param>
        public async Task Load(int page)
        {
            CurrentPage = page;
            await LoadProductsAsync();
        }

        /// <summary>
        /// Loads the products sorted by name.
        /// </summary>
        /// <param name="nameAcending">Indicates whether the names should be sorted in ascending order.</param>
        public async Task LoadProductSort(bool nameAcending)
        {
            NameAcending = nameAcending;
            await LoadProductsAsync();
        }

        /// <summary>
        /// Loads the products asynchronously.
        /// </summary>
        public async Task LoadProductsAsync()
        {
            var (totalItems, products) = await _dao.GetProductsAsync(CurrentPage, RowsPerPage, Keyword, NameAcending, null, null);
            FoodItems.Clear();
            foreach (var item in products)
            {
                FoodItems.Add(item);
            }
            OnPropertyChanged(nameof(FoodItems));

            TotalItems = totalItems;
            TotalPages = (TotalItems / RowsPerPage) + ((TotalItems % RowsPerPage == 0) ? 0 : 1);
        }

        /// <summary>
        /// Loads all products asynchronously.
        /// </summary>
        public async Task LoadAllProductsAsync()
        {
            var (totalItems, products) = await _dao.GetProductsAsync(null, null, null, true, null, null);
            allFoodItems.Clear();
            foreach (var item in products)
            {
                allFoodItems.Add(item);
            }
        }

        /// <summary>
        /// Adds a new food item.
        /// </summary>
        /// <param name="food">The food item to add.</param>
        public async Task AddFoodItem(FoodModel food)
        {
            FoodModel newFood = await _dao.AddProductAsync(food);
            if (newFood != null)
            {
                FoodItems.Add(newFood);
            }
            else
            {
                throw new Exception("Fail to add new product");
            }
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">The product to update.</param>
        public async Task UpdateProduct(FoodModel product)
        {
            FoodModel updateFood = await _dao.UpdateProductAsync(product);
            if (updateFood != null)
            {
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
            }
            else
            {
                await MessageHelper.ShowErrorMessage("Fail to update product", App.m_window.Content.XamlRoot);
            }
        }

        /// <summary>
        /// Confirms and adds a food item.
        /// </summary>
        /// <param name="food">The food item to confirm and add.</param>
        private async Task ConfirmAndAddFoodItem(FoodModel food)
        {
            if (food == null)
            {
                return;
            }

            bool isConfirmed = await MessageHelper.ShowConfirmationDialog(
                $"Are you sure you want to delete: {food.Name}?",
                "Confirm customer deletion",
                App.m_window.Content.XamlRoot
            );

            if (isConfirmed)
            {
                await DeleteProduct(food);
            }
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="product">The product to delete.</param>
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
            }
            else
            {
                await MessageHelper.ShowErrorMessage("Fail to remove product", App.m_window.Content.XamlRoot);
            }
        }

        /// <summary>
        /// Picks an Excel file asynchronously.
        /// </summary>
        /// <returns>The file path of the picked Excel file.</returns>
        public async Task<string> PickExcelFileAsync()
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".xlsx");
            picker.FileTypeFilter.Add(".xls");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSingleFileAsync();
            return file?.Path;
        }

        /// <summary>
        /// Imports products from an Excel file.
        /// </summary>
        /// <param name="filePath">The file path of the Excel file.</param>
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

        /// <summary>
        /// Gets the cell value from a spreadsheet document.
        /// </summary>
        /// <param name="document">The spreadsheet document.</param>
        /// <param name="cell">The cell to get the value from.</param>
        /// <returns>The cell value as a string.</returns>
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

        /// <summary>
        /// Picks a save file asynchronously.
        /// </summary>
        /// <returns>The file path of the picked save file.</returns>
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

        /// <summary>
        /// Exports data to an Excel file.
        /// </summary>
        /// <param name="filePath">The file path to save the Excel file.</param>
        /// <param name="data">The data to export.</param>
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

        /// <summary>
        /// Creates a text cell.
        /// </summary>
        /// <param name="cellReference">The cell reference.</param>
        /// <param name="cellValue">The cell value.</param>
        /// <returns>The created text cell.</returns>
        private Cell CreateTextCell(string cellReference, string cellValue)
        {
            return new Cell()
            {
                CellReference = cellReference,
                DataType = CellValues.String,
                CellValue = new CellValue(cellValue)
            };
        }

        /// <summary>
        /// Creates a number cell.
        /// </summary>
        /// <param name="cellReference">The cell reference.</param>
        /// <param name="cellValue">The cell value.</param>
        /// <returns>The created number cell.</returns>
        private Cell CreateNumberCell(string cellReference, string cellValue)
        {
            return new Cell()
            {
                CellReference = cellReference,
                DataType = CellValues.Number,
                CellValue = new CellValue(cellValue)
            };
        }
    }
}
