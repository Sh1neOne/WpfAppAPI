using WpfAppAPI.Models;

namespace WebApplication.Entities
{
    public class Product : BaseModel
    {
        private string name;
        private string description;
        private byte[] image;

        public int Id { get; set; }

        public string Name
        {
            get => name;
            set
            {
                name = value; 
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        public byte[] Image
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }
    }
}
