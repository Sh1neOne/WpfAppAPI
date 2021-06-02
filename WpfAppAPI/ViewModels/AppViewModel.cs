using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using WebApplication.Entities;
using WebApplication.Interface;
using WebApplication.Service;
using WpfAppAPI.Services;

namespace WpfAppAPI.ViewModels
{
    public class AppViewModel : BaseViewModel
    {
        private ObservableCollection<Product> products;
        private Product selectedProduct;
        private string txtEditName;
        private string txtEditDescription;
        private byte[] imgEditImage;
        private ObservableCollection<string> themes;
        private string selectedTheme;
        public ICommand GetProducts { get; }
        public ICommand DeleteProduct { get; }
        public ICommand AddProduct { get; }
        public ICommand EditProduct { get; }
        public ICommand AddImage { get; }
        private readonly IProductData repository;

        public AppViewModel()
        {
            repository = new ProductRerositoryApi();
            GetProducts = new DelegateCommand(OnGetProducts);
            DeleteProduct = new DelegateCommand(OnDeleteProduct, OnDeleteProductCanExecute);
            AddProduct = new DelegateCommand(OnAddProduct);
            EditProduct = new DelegateCommand(OnEditProduct, OnEditProductCanExecute);
            AddImage = new DelegateCommand(OnAddImage);
            themes = new ObservableCollection<string>() {"light", "dark"};
            SelectedTheme = "light";
        }

        private void OnAddImage(object obj)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";
            if (openDialog.ShowDialog() == true)
            {
                var selectedImage = new BitmapImage(new Uri(openDialog.FileName));
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(selectedImage));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    ImgEditImage = ms.ToArray();
                }
            }

        }

        private void OnEditProduct(object obj)
        {
            SelectedProduct.Name = TxtEditName;
            SelectedProduct.Description = TxtEditDescription;
            SelectedProduct.Image = ImgEditImage;
            repository.SaveProduct(SelectedProduct);
        }

        private bool OnEditProductCanExecute(object arg) => SelectedProduct != null;

        private void OnAddProduct(object obj)
        {
            var newProduct = new Product()
            {
                Description = TxtEditDescription,
                Name = TxtEditName,
                Image = ImgEditImage
            };

            repository.SaveProduct(newProduct);
            Products.Add(newProduct);
        }

        private bool OnDeleteProductCanExecute(object arg)
        {
            return SelectedProduct != null;
        }  

        private void OnDeleteProduct(object obj)
        {
            repository.DeleteProduct(SelectedProduct.Id);
            Products.Remove(SelectedProduct);
        }

        private void OnThemeChange()
        {
            string style = "Themes/" + selectedTheme;
            // определяем путь к файлу ресурсов
            var uri = new Uri(style + ".xaml", UriKind.Relative);
            // загружаем словарь ресурсов
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            // очищаем коллекцию ресурсов приложения
            Application.Current.Resources.Clear();
            // добавляем загруженный словарь ресурсов
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
        private async void OnGetProducts(object obj)
        {
            try
            {
                var pr = await Task.Run(() => repository.GetProducts());
                Products =  new ObservableCollection<Product>(pr);
            }
            catch (Exception)
            {
                MessageBox.Show("Нет подключения к API");
            }
        }

        public ObservableCollection<Product> Products
        {
            get => products;
            set => Set(ref products, value);
        }

        public Product SelectedProduct
        {
            get => selectedProduct;
            set
            {
                Set(ref selectedProduct, value);
                TxtEditName = value?.Name;
                TxtEditDescription = value?.Description;
                ImgEditImage = value?.Image;
            }
        }

        public string TxtEditName
        {
            get => txtEditName; 
            set => Set(ref txtEditName,value);
        }

        public string TxtEditDescription
        {
            get => txtEditDescription;
            set => Set(ref txtEditDescription, value);
        }

        public byte[] ImgEditImage
        {
            get => imgEditImage;
            set => Set(ref imgEditImage, value);
        }

        public ObservableCollection<string> Themes
        {
            get => themes;
            set => themes = value;
        }

        public string SelectedTheme
        {
            get => selectedTheme;
            set
            {
                selectedTheme = value;
                OnThemeChange();
            }
        }
    }
}
